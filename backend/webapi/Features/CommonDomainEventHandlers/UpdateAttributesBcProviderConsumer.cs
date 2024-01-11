namespace Pidp.Features.CommonDomainEventHandlers;

using MassTransit;
using Pidp.Infrastructure.HttpClients.BCProvider;
using static Pidp.Features.CommonDomainEventHandlers.UpdateAttributesBcProviderConsumer;

public class UpdateAttributesBcProviderConsumer : IConsumer<UpdateAttributesBcProvider>
{
    public class UpdateAttributesBcProvider
    {
        public string Upn { get; set; }
        public Dictionary<string, object> Attributes { get; set; }

        public UpdateAttributesBcProvider(string upn, Dictionary<string, object> attributes)
        {
            this.Upn = upn;
            this.Attributes = attributes;
        }
    }

    private readonly IBCProviderClient client;
    private readonly ILogger<UpdateAttributesBcProviderConsumer> logger;

    public UpdateAttributesBcProviderConsumer(
        IBCProviderClient client,
        ILogger<UpdateAttributesBcProviderConsumer> logger)
    {
        this.client = client;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<UpdateAttributesBcProvider> context)
    {
        if (!await this.client.UpdateAttributes(context.Message.Upn, context.Message.Attributes))
        {
            this.logger.LogUpdateAttributesBcProviderFailed(context.Message.Upn);
            throw new InvalidOperationException("Error Comunicating with Azure AD");
        }
    }
}

public static partial class UpdateAttributesBcProviderConsumerLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when updating attributes to User #{Upn} in Azure AD.")]
    public static partial void LogUpdateAttributesBcProviderFailed(this ILogger logger, string Upn);
}

