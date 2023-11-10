namespace PlrIntake;

using FluentValidation.AspNetCore;
using HealthChecks.ApplicationStatus.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SoapCore;
using System.Reflection;
using System.ServiceModel;
using System.Text.Json;

using PlrIntake.Data;
using PlrIntake.Features;
using PlrIntake.Features.Intake;
using PlrIntake.Infrastructure.HealthChecks;

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

        services.AddScoped<IIntakeService, IntakeService>();
        services.AddSoapServiceOperationTuner(new IntakeServiceOperationTuner(config));
        services.AddSoapCore();

        services.AddHealthChecks()
            .AddApplicationStatus(tags: new[] { HealthCheckTag.Liveness.Value })
            .AddNpgSql(config.ConnectionStrings.PlrDatabase, tags: new[] { HealthCheckTag.Readiness.Value });
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

        app.UseSerilogRequestLogging();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();

        var intakeBinding = new BasicHttpBinding
        {
            Security = new BasicHttpSecurity
            {
                Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                Transport = new HttpTransportSecurity
                {
                    ClientCredentialType = HttpClientCredentialType.Basic
                }
            }
        };

        app.UseEndpoints(endpoints =>
        {
            endpoints.UseSoapEndpoint<IIntakeService>("/api/PLRHL7", intakeBinding, SoapSerializer.XmlSerializer);
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health/liveness", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains(HealthCheckTag.Liveness)
            }).AllowAnonymous();
            endpoints.MapHealthChecks("/health/readiness", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains(HealthCheckTag.Readiness)
            }).AllowAnonymous();
        });
    }
}
