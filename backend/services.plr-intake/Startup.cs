namespace PlrIntake;

using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using System.Text.Json;

using PlrIntake.Data;
using PlrIntake.Features;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => this.Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        var config = this.InitializeConfiguration(services);

        services.AddControllers()
            .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>());

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<PlrDbContext>(options => options
            .UseNpgsql(config.ConnectionStrings.PlrDatabase)
            .EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: false));

        services.Scan(scan => scan
            .FromAssemblyOf<Startup>()
            .AddClasses(classes => classes.AssignableTo<IRequestHandler>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        // TODO Healthchecks
        // services
        //     .AddHealthChecks()
        //     .AddDbContextCheck<PlrIntakeDbContext>("DbContextHealthCheck")
        //     .AddNpgSql(connectionString);
    }

    private PlrIntakeConfiguration InitializeConfiguration(IServiceCollection services)
    {
        var config = new PlrIntakeConfiguration();
        this.Configuration.Bind(config);

        services.AddSingleton(config);

        Log.Logger.Information("### App Version:{0} ###", Assembly.GetExecutingAssembly().GetName().Version);
        Log.Logger.Information("### PLR Intake API Configuration:{0} ###", JsonSerializer.Serialize(config));

        return config;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            // endpoints.MapHealthChecks("/health");
        });
    }
}
