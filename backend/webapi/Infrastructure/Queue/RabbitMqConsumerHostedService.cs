namespace Pidp.Infrastructure.Queue;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Pidp.Features.CommonHandlers;
using Pidp.Features.Parties;
using Pidp.Models.DomainEvents;
using static Pidp.Features.Parties.Demographics;

public class RabbitMqConsumerHostedService(
    IConnectionFactory connectionFactory,
    IServiceProvider serviceProvider,
    ILogger<RabbitMqConsumerHostedService> logger) : BackgroundService
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<RabbitMqConsumerHostedService> _logger = logger;
    private IConnection? _connection;
    private IChannel? _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _connection = await _connectionFactory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await SetupConsumerAsync<PartyEmailUpdated>("party-email-updated", "party-email-updated-bc-provider-queue", stoppingToken);
            await SetupConsumerAsync<UpdateBcProviderAttributesHandler.UpdateBcProviderAttributes>("update-bc-provider-attributes", "update-bc-provider-attributes-queue", stoppingToken);
            await SetupConsumerAsync<UpdateKeycloakAttributesHandler.UpdateKeycloakAttributes>("update-keycloak-attributes", "update-keycloak-attributes-queue", stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to RabbitMQ for consumers");
        }
    }

    private async Task SetupConsumerAsync<TMessage>(string exchangeName, string queueName, CancellationToken stoppingToken)
    {
        await _channel!.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Fanout, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: string.Empty, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<TMessage>(Encoding.UTF8.GetString(body));

                if (message != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    
                    if (typeof(TMessage) == typeof(PartyEmailUpdated))
                    {
                        var handler = scope.ServiceProvider.GetRequiredService<PartyEmailUpdatedBcProviderHandler>();
                        await handler.HandleAsync((message as PartyEmailUpdated)!);
                    }
                    else if (typeof(TMessage) == typeof(UpdateBcProviderAttributesHandler.UpdateBcProviderAttributes))
                    {
                        var handler = scope.ServiceProvider.GetRequiredService<UpdateBcProviderAttributesHandler>();
                        await handler.HandleAsync((message as UpdateBcProviderAttributesHandler.UpdateBcProviderAttributes)!);
                    }
                    else if (typeof(TMessage) == typeof(UpdateKeycloakAttributesHandler.UpdateKeycloakAttributes))
                    {
                        var handler = scope.ServiceProvider.GetRequiredService<UpdateKeycloakAttributesHandler>();
                        await handler.HandleAsync((message as UpdateKeycloakAttributesHandler.UpdateKeycloakAttributes)!);
                    }
                }

                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from queue {QueueName}", queueName);
                
                // Extremely basic retry/failure handling (MassTransit used to handle redelivery natively)
                await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken: stoppingToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken: stoppingToken);
        await base.StopAsync(stoppingToken);
    }
}
