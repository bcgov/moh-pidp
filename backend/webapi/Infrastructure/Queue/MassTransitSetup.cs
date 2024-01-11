namespace Pidp.Infrastructure.Queue;

using MassTransit;
using Pidp;
using Pidp.Features.CommonDomainEventHandlers;
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
            x.AddConsumer<UpdateAttributesBcProviderConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(config.RabbitMQ.HostAddress));

                // Configure redelivery
                cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromHours(1), TimeSpan.FromHours(3), TimeSpan.FromHours(6), TimeSpan.FromHours(12)));
                // Configure retry policy
                cfg.UseMessageRetry(r => r.Interval(2, TimeSpan.FromSeconds(5)));
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

                cfg.ReceiveEndpoint("update-attributes-bc-provider-queue", ep =>
                {
                    ep.PublishFaults = false;
                    ep.Bind("party-email-updated");
                    ep.ConfigureConsumer<UpdateAttributesBcProviderConsumer>(context);
                });
            });
        });

        return services;
    }
}
