﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using System.Reflection;

using DoWork;
using Pidp;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients;
using Pidp.Infrastructure.HttpClients.Keycloak;


await Host.CreateDefaultBuilder(args)
    .UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
    .ConfigureServices((hostContext, services) =>
    {
        var config = InitializeConfiguration(services);

        services.AddHttpClient<IAccessTokenClient, AccessTokenClient>();
        services.AddHttpClientWithBaseAddress<IKeycloakAdministrationClient, KeycloakAdministrationClient>(config.Keycloak.AdministrationUrl)
             .WithBearerToken(new KeycloakAdministrationClientCredentials
             {
                 Address = config.Keycloak.TokenUrl,
                 ClientId = config.Keycloak.AdministrationClientId,
                 ClientSecret = config.Keycloak.AdministrationClientSecret
             });

        services
            .AddSingleton<IClock>(SystemClock.Instance)
            .AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining<Startup>())
            .AddTransient<IDoWorkService, DoWorkService>()
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
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    var configuration = builder.Build();

    var config = new PidpConfiguration();
    configuration.Bind(config);
    services.AddSingleton(config);

    return config;
}
