namespace Pidp;

using FluentValidation.AspNetCore;
using HealthChecks.ApplicationStatus.DependencyInjection;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text.Json;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Features;
using Pidp.Infrastructure;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HealthChecks;
using Pidp.Infrastructure.HttpClients;
using Pidp.Infrastructure.Services;
using Pidp.Infrastructure.Queue;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => this.Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        var config = this.InitializeConfiguration(services);

        services
            .AddAutoMapper(typeof(Startup))
            .AddHostedService<PlrStatusUpdateSchedulingService>()
            .AddHttpClients(config)
            .AddKeycloakAuth(config)
            .AddRabbitMQ(config)
            .AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining<Startup>())
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<IPidpAuthorizationService, PidpAuthorizationService>()
            .AddScoped<IPlrStatusUpdateService, PlrStatusUpdateService>()
            .AddSingleton<IClock>(SystemClock.Instance)
            .AddSingleton<BackgroundWorkerHealthCheck>();

        services.AddControllers(options => options.Conventions.Add(new RouteTokenTransformerConvention(new KabobCaseParameterTransformer())))
            .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>())
            .AddJsonOptions(options => options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb))
            .AddHybridModelBinder();

        services.AddDbContext<PidpDbContext>(options => options
            .UseNpgsql(config.ConnectionStrings.PidpDatabase, npg => npg.UseNodaTime())
            .EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: false)
            .UseProjectables());

        services.Scan(scan => scan
            .FromAssemblyOf<Startup>()
            .AddClasses(classes => classes.AssignableTo<IRequestHandler>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        services.AddHealthChecks()
            .AddApplicationStatus(tags: new[] { HealthCheckTag.Liveness.Value })
            .AddCheck<BackgroundWorkerHealthCheck>("PlrStatusUpdateSchedulingService", tags: new[] { HealthCheckTag.BackgroundServices.Value })
            // .AddDbContextCheck<PidpDbContext>(tags: new[] { HealthCheckTag.Readiness.Value });
            .AddRabbitMQ(new Uri(config.RabbitMQ.HostAddress), tags: new[] { HealthCheckTag.Readiness.Value });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "PIdP Web API", Version = "v1" });
            options.AddSecurityDefinition("Bearer Auth", new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Description = "Standard JWT Authorization header using the Bearer scheme.",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Scheme = "Bearer",
                Type = SecuritySchemeType.Http,
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
            options.CustomSchemaIds(x => x.FullName);
        });
        services.AddFluentValidationRulesToSwagger();
    }

    private PidpConfiguration InitializeConfiguration(IServiceCollection services)
    {
        var config = new PidpConfiguration();
        this.Configuration.Bind(config);

        services.AddSingleton(config);

        Log.Logger.Information("### App Version:{0} ###", Assembly.GetExecutingAssembly().GetName().Version);
        Log.Logger.Information("### PIdP Configuration:{0} ###", JsonSerializer.Serialize(config));

        return config;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "PIdP Web API"));

        app.UseSerilogRequestLogging(options => options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            var userId = httpContext.User.GetUserId();
            if (!userId.Equals(Guid.Empty))
            {
                diagnosticContext.Set("User", userId);
            }
        });
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health/background-services", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains(HealthCheckTag.BackgroundServices)
            }).AllowAnonymous();
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
