namespace Pidp;

using System.Reflection;
using System.Text.Json;
using System.Threading.RateLimiting;
using FluentValidation;
using HealthChecks.ApplicationStatus.DependencyInjection;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Pidp.Data;
using Pidp.Extensions;
using Pidp.Features;
using Pidp.Features.AccessRequests;
using Pidp.Infrastructure;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HealthChecks;
using Pidp.Infrastructure.HttpClients;
using Pidp.Infrastructure.Queue;
using Pidp.Infrastructure.Services;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Swashbuckle.AspNetCore.Filters;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        var config = this.InitializeConfiguration(services);

        MapsterSetup.Configure();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                var origins = config.CorsAllowedOrigins.Split(',').Select(o => o.Trim()).ToArray();
                builder.WithOrigins(origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
            });
        });

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Strict;
            options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
            options.Secure = CookieSecurePolicy.Always;
        });

        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 2500,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 10
                    }));
        });

        services
            .AddHostedService<PlrStatusUpdateSchedulingService>()
            .AddHttpClients(config)
            .AddHttpContextAccessor()
            .AddKeycloakAuth(config)
            .AddRabbitMQ(config)
            .AddMediator(options => options.ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<IAccessRequestRevocationService, AccessRequestRevocationService>()
            .AddScoped<IAccessRequestRevocationPolicy, InfantRsvEformsRevocationPolicy>()
            .AddScoped<IAccessRequestRevocationPolicy, NpdpEformsRevocationPolicy>()
            .AddScoped<IPidpAuthorizationService, PidpAuthorizationService>()
            .AddScoped<IPlrStatusUpdateService, PlrStatusUpdateService>()
            .AddScoped<IBCProviderService, BCProviderService>()
            .AddScoped<IPharmacyStaffDeactivationService, PharmacyStaffDeactivationService>()
            .AddHostedService<PharmacyStaffDeactivationHostedService>()
            .AddSingleton<IClock>(SystemClock.Instance)
            .AddSingleton<BackgroundWorkerHealthCheck>();

        services.AddControllers(options => options.Conventions.Add(new RouteTokenTransformerConvention(new KabobCaseParameterTransformer())))
            .AddJsonOptions(options => options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb))
            .AddHybridModelBinder();

        services.AddValidatorsFromAssemblyContaining<Startup>()
            .AddFluentValidationAutoValidation(options =>
            {
                options.EnablePathBindingSourceAutomaticValidation = true;
                options.EnableFormBindingSourceAutomaticValidation = true;
            });

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
            .AddApplicationStatus(tags: [HealthCheckTag.Liveness.Value])
            .AddCheck<BackgroundWorkerHealthCheck>("PlrStatusUpdateSchedulingService", tags: [HealthCheckTag.BackgroundServices.Value])
            .AddCheck<RabbitMQHealthCheck>("RabbitMQHealthCheck", tags: [HealthCheckTag.BackgroundServices.Value])
            .AddDbContextCheck<PidpDbContext>(tags: [HealthCheckTag.Readiness.Value]);

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

            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "PIdP Web API"));
        }
        else
        {
            app.UseHsts();
        }

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
        app.UseCookiePolicy();
        app.UseRateLimiter();
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
