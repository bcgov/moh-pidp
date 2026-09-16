namespace Pidp.Infrastructure.Queue;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);
}

public class RabbitMqPublisher(IConnectionFactory connectionFactory, ILogger<RabbitMqPublisher> logger) : IRabbitMqPublisher
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory;
    private readonly ILogger<RabbitMqPublisher> _logger = logger;

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        var exchangeName = typeof(T).Name;
        
        // Use a new connection/channel per publish for simplicity, though a persistent 
        // connection pool is better for production. RabbitMQ.Client 7.0 is highly concurrent.
        await using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        channel.BasicReturnAsync += (sender, args) =>
        {
            _logger.LogWarning("Message to exchange {Exchange} with routing key {RoutingKey} was unroutable. ReplyCode: {ReplyCode}, ReplyText: {ReplyText}", 
                args.Exchange, args.RoutingKey, args.ReplyCode, args.ReplyText);
            return Task.CompletedTask;
        };

        await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Fanout, cancellationToken: cancellationToken);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var properties = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: string.Empty,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }
}
