using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using System.Reflection;

using EndorsementReminder;
using Pidp;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;


await Host.CreateDefaultBuilder(args)
    .UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
    .ConfigureServices((hostContext, services) =>
    {
        var config = InitializeConfiguration(services);

        services.AddHttpClient<IAccessTokenClient, AccessTokenClient>();
        services
            .AddTransient<ISmtpEmailClient, SmtpEmailClient>()
            .AddHttpClientWithBaseAddress<IChesClient, ChesClient>(config.ChesClient.Url)
                .WithBearerToken(new ChesClientCredentials
                {
                    Address = config.ChesClient.TokenUrl,
                    ClientId = config.ChesClient.ClientId,
                    ClientSecret = config.ChesClient.ClientSecret
                });

        services
            .AddSingleton<IClock>(SystemClock.Instance)
            .AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining<Startup>())
            .AddTransient<IEmailService, EmailService>()
            .AddTransient<IEndorsementMaintenanceService, EndorsementMaintenanceService>()
            .AddHostedService<HostedServiceWrapper>()
            .AddDbContext<PidpDbContext>(options => options
                .UseNpgsql(config.ConnectionStrings.PidpDatabase, npg => npg.UseNodaTime())
                .EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: false)
                .UseProjectables());
    })
    .RunConsoleAsync();

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
