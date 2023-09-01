namespace Pidp.Infrastructure.Queue;

using MassTransit;
using Pidp;
using static Pidp.Features.Parties.Demographics;

public static class MassTransitSetup
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, PidpConfiguration config)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<PartyEmailUpdatedKeycloakConsumer>();
            x.AddConsumer<PartyEmailUpdatedBcProviderConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(config.RabbitMQ.HostAddress));

                // retry policy
                cfg.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30)));
                cfg.UseInMemoryOutbox();

                cfg.ReceiveEndpoint("party-email-updated-keycloak-queue", ep =>
                {
                    ep.PublishFaults = false;
                    ep.Bind("party-email-updated");
                    ep.ConfigureConsumer<PartyEmailUpdatedKeycloakConsumer>(context);
                });

                cfg.ReceiveEndpoint("party-email-updated-bc-provider-queue", ep =>
                {
                    ep.PublishFaults = false;
                    ep.Bind("party-email-updated");
                    ep.ConfigureConsumer<PartyEmailUpdatedBcProviderConsumer>(context);
                });
            });
        });

        return services;
    }
}
