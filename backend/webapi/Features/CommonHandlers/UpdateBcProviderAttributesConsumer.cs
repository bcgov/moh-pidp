namespace Pidp.Features.CommonHandlers;

using MassTransit;

using static Pidp.Features.CommonHandlers.UpdateBcProviderAttributesConsumer;
using Pidp.Infrastructure.HttpClients.BCProvider;

public class UpdateBcProviderAttributesConsumer(IBCProviderClient client, ILogger<UpdateBcProviderAttributesConsumer> logger) : IConsumer<UpdateBcProviderAttributes>
{
    public class UpdateBcProviderAttributes(string upn, Dictionary<string, object> attributes)
    {
        public string Upn { get; set; } = upn;
        public Dictionary<string, object> Attributes { get; set; } = attributes;
    }

    private readonly IBCProviderClient client = client;
    private readonly ILogger<UpdateBcProviderAttributesConsumer> logger = logger;

    public async Task Consume(ConsumeContext<UpdateBcProviderAttributes> context)
    {
        if (!await this.client.UpdateAttributes(context.Message.Upn, context.Message.Attributes))
        {
            this.logger.LogUpdateBcProviderAttributesFailed(context.Message.Upn);
            throw new InvalidOperationException("Error comunicating with Azure AD");
        }
    }
}

public static partial class UpdateBcProviderAttributesConsumerLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when updating attributes to User {upn} in Azure AD.")]
    public static partial void LogUpdateBcProviderAttributesFailed(this ILogger<UpdateBcProviderAttributesConsumer> logger, string upn);
}
