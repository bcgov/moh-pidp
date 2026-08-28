namespace Pidp.Infrastructure.Queue;

using RabbitMQ.Client;
using Pidp;
using Pidp.Features.CommonHandlers;
using static Pidp.Features.Parties.Demographics;

public static class RabbitMqSetup
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, PidpConfiguration config)
    {
        // 1. Connection Factory Singleton
        services.AddSingleton<IConnectionFactory>(sp =>
        {
            return new ConnectionFactory
            {
                Uri = new Uri(config.RabbitMQ.HostAddress)
            };
        });

        // 2. Publisher Scoped
        services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();

        // 3. Handlers
        services.AddScoped<PartyEmailUpdatedBcProviderHandler>();
        services.AddScoped<UpdateBcProviderAttributesHandler>();
        services.AddScoped<UpdateKeycloakAttributesHandler>();

        // 4. Background Service
        services.AddHostedService<RabbitMqConsumerHostedService>();

        return services;
    }
}
