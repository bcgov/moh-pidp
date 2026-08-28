using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using System.Reflection;

using DoWork;
using DoWork.Services.BulkEformRoles;
using DoWork.Services.CredentialDeletionService;
using Pidp;
using Pidp.Data;
using Pidp.Features;
using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.HttpClients;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;


// Parsed before the host is built so an invalid invocation fails immediately, with nothing
// connected and nothing touched.
if (!BulkRunOptions.TryParse(args, out var bulkRunOptions, out var argumentError))
{
    Console.WriteLine($"ERROR: {argumentError}");
    Console.WriteLine();
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run                                                 dry run - report only, writes nothing");
    Console.WriteLine("  dotnet run -- --access-type ImmsBCEforms                    dry run for one card");
    Console.WriteLine("  dotnet run -- --access-type ImmsBCEforms --apply --from <report.csv>");
    Console.WriteLine("                                                             act on the Parties in a reviewed report");
    return 1;
}

await Host.CreateDefaultBuilder(args)
    .UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)
    .ConfigureServices((hostContext, services) =>
    {
        var config = InitializeConfiguration(services);

        services
            .AddHttpClients(config)
            // .AddRateLimitedKeycloakClient(config)
            .AddSingleton(bulkRunOptions)
            .AddSingleton<IClock>(SystemClock.Instance)
            // Must match the lifetime webapi's Startup.cs configures: the Mediator source generator
            // emits code for one lifetime, and the generated registration throws on a mismatch.
            .AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped)
            .AddTransient<ICredentialDeletionService, CredentialDeletionService>()
            .AddTransient<DoWork.Services.DataDriftFixService.IDataDriftFixService, DoWork.Services.DataDriftFixService.DataDriftFixService>()
            .AddTransient<DoWork.Services.KeycloakClientValidationService.IKeycloakClientValidationService, DoWork.Services.KeycloakClientValidationService.KeycloakClientValidationService>()
            .AddTransient<DoWork.Services.RemoveCollegeLicenseInfoService.IRemoveCollegeLicenseInfoService, DoWork.Services.RemoveCollegeLicenseInfoService.RemoveCollegeLicenseInfoService>()
            .AddTransient<IAccessRoleCleanupService, AccessRoleCleanupService>()
            .AddTransient<IInfantRsvBulkGrantService, InfantRsvBulkGrantService>()
            .AddScoped<IAccessRequestRevocationPolicy, ImmsBCEformsRevocationPolicy>()
            .AddScoped<IAccessRequestRevocationPolicy, InfantRsvEformsRevocationPolicy>()
            .AddScoped<IAccessRequestRevocationPolicy, NpdpEformsRevocationPolicy>()
            .AddScoped<IAccessRequestRevocationPolicy, SAEformsRevocationPolicy>()
            .AddTransient<IDoWorkService, DoWorkService>()
            .AddScoped<IPlrClient, PlrRecordDbClient>()
            .AddHostedService<HostedServiceWrapper>()
            .AddDbContext<PidpDbContext>(options => options
                .UseNpgsql(config.ConnectionStrings.PidpDatabase, npg => npg.UseNodaTime())
                .EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: false)
                .UseProjectables());

        // Mirrors webapi's Startup.cs: registers the command handlers against ICommandHandler<,> so the
        // bulk grant can call the same handler a portal request does.
        services.Scan(scan => scan
            .FromAssemblyOf<PidpConfiguration>()
            .AddClasses(classes => classes.AssignableTo<IRequestHandler>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        // The dry-run seam. A RecordingRevocationService always sits in front of the policies so the tool
        // can report their decisions; only what sits BEHIND it changes. Binding it here means that without
        // --apply the real service is never constructed, so no code path can reach Keycloak or delete an
        // Access Request.
        if (bulkRunOptions.Apply)
        {
            services.AddScoped<AccessRequestRevocationService>();
            services.AddScoped<IAccessRequestRevocationService>(sp =>
                new RecordingRevocationService(sp.GetRequiredService<AccessRequestRevocationService>()));
        }
        else
        {
            services.AddScoped<IAccessRequestRevocationService>(_ => new RecordingRevocationService());
        }
    })
    .RunConsoleAsync();

return Environment.ExitCode;

static PidpConfiguration InitializeConfiguration(IServiceCollection services)
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") is null or "Development")
    {
        builder.AddUserSecrets<Program>();
    }

    var configuration = builder.Build();

    var config = new PidpConfiguration();
    configuration.Bind(config);
    services.AddSingleton(config);

    return config;
}
