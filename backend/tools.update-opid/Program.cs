using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using System.Reflection;

using UpdateOpId;
using Pidp;
using Pidp.Data;


await Host.CreateDefaultBuilder(args)
    .UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
    .ConfigureServices((hostContext, services) =>
    {
        var config = InitializeConfiguration(services);

        services
            .AddSingleton<IClock>(SystemClock.Instance)
            .AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining<Startup>())
            .AddTransient<IUpdateOpIdService, UpdateOpIdService>()
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
