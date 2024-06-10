namespace Pidp.Features.CommonHandlers;

using MassTransit;

using static Pidp.Features.CommonHandlers.UpdateBcProviderAttributesConsumer;
using Pidp.Infrastructure.HttpClients.BCProvider;

public class UpdateBcProviderAttributesConsumer : IConsumer<UpdateBcProviderAttributes>
{
    public class UpdateBcProviderAttributes
    {
        public string Upn { get; set; }
        public Dictionary<string, object> Attributes { get; set; }

        public UpdateBcProviderAttributes(string upn, Dictionary<string, object> attributes)
        {
            this.Upn = upn;
            this.Attributes = attributes;
        }
    }

    private readonly IBCProviderClient client;
    private readonly ILogger<UpdateBcProviderAttributesConsumer> logger;

    public UpdateBcProviderAttributesConsumer(IBCProviderClient client, ILogger<UpdateBcProviderAttributesConsumer> logger)
    {
        this.client = client;
        this.logger = logger;
    }

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
