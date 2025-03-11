namespace Pidp.Infrastructure.Queue;

using MassTransit;
using Pidp;
using Pidp.Features.CommonHandlers;
using Pidp.Infrastructure.Services;
using static Pidp.Features.Parties.Demographics;

public static class MassTransitSetup
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, PidpConfiguration config)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<PartyEmailUpdatedBcProviderConsumer>();
            x.AddConsumer<UpdateBcProviderAttributesConsumer>();
            x.AddConsumer<UpdateKeycloakAttributesConsumer>();
            x.AddSagaStateMachine<BCProviderSagaService, BCProviderSagaState>()
                .InMemoryRepository();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(config.RabbitMQ.HostAddress));

                // Configure redelivery
                cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromHours(1), TimeSpan.FromHours(3), TimeSpan.FromHours(6), TimeSpan.FromHours(12)));
                // Configure retry policy
                cfg.UseMessageRetry(r => r.Interval(2, TimeSpan.FromSeconds(5)));
                cfg.UseInMemoryOutbox(context);

                cfg.ReceiveEndpoint("party-email-updated-bc-provider-queue", ep =>
                {
                    ep.PublishFaults = false;
                    ep.Bind("party-email-updated");
                    ep.ConfigureConsumer<PartyEmailUpdatedBcProviderConsumer>(context);
                });

                cfg.ReceiveEndpoint("update-bc-provider-attributes-queue", ep =>
                {
                    ep.PublishFaults = false;
                    ep.Bind("update-bc-provider-attributes");
                    ep.ConfigureConsumer<UpdateBcProviderAttributesConsumer>(context);
                });

                cfg.ReceiveEndpoint("update-keycloak-attributes-queue", ep =>
                {
                    ep.PublishFaults = false;
                    ep.Bind("update-keycloak-attributes");
                    ep.ConfigureConsumer<UpdateKeycloakAttributesConsumer>(context);
                });

                cfg.ReceiveEndpoint("bc-provider-saga", ep => ep.ConfigureSaga<BCProviderSagaState>(context));
            });
        });

        return services;
    }
}
