using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using Serilog;
using Serilog.Events;
using System.Reflection;

using DoWork;
using DoWork.Services.CredentialDeletionService;
using Pidp;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose)
    .WriteTo.File("logs/resync-service.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Verbose)
    .CreateLogger();

try
{
    await Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)
    .ConfigureServices((hostContext, services) =>
    {
        var config = InitializeConfiguration(services);

        services
            .AddHttpClients(config)
            // .AddRateLimitedKeycloakClient(config)
            .AddSingleton<IClock>(SystemClock.Instance)
            .AddMediator()
            .AddTransient<ICredentialDeletionService, CredentialDeletionService>()
            .AddTransient<DoWork.Services.ResyncService.IResyncService, DoWork.Services.ResyncService.ResyncService>()
            .AddTransient<IDoWorkService, DoWorkService>()
            .AddHostedService<HostedServiceWrapper>()
            .AddDbContext<PidpDbContext>(options => options
                .UseNpgsql(config.ConnectionStrings.PidpDatabase, npg => npg.UseNodaTime())
                .EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: false)
                .UseProjectables());
    })
    .RunConsoleAsync();
}
finally
{
    Log.CloseAndFlush();
}

static PidpConfiguration InitializeConfiguration(IServiceCollection services)
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    var configuration = builder.Build();

    var config = new PidpConfiguration();
    configuration.Bind(config);
    services.AddSingleton(config);

    return config;
}
