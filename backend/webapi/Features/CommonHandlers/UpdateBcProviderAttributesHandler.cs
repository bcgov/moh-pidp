namespace Pidp.Features.CommonHandlers;

using static Pidp.Features.CommonHandlers.UpdateBcProviderAttributesHandler;
using Pidp.Infrastructure.HttpClients.BCProvider;

public class UpdateBcProviderAttributesHandler(IBCProviderClient client, ILogger<UpdateBcProviderAttributesHandler> logger)
{
    public class UpdateBcProviderAttributes(string upn, Dictionary<string, object> attributes)
    {
        public string Upn { get; set; } = upn;
        public Dictionary<string, object> Attributes { get; set; } = attributes;
    }

    private readonly IBCProviderClient client = client;
    private readonly ILogger<UpdateBcProviderAttributesHandler> logger = logger;

    public async Task HandleAsync(UpdateBcProviderAttributes message)
    {
        if (!await this.client.UpdateAttributes(message.Upn, message.Attributes))
        {
            this.logger.LogUpdateBcProviderAttributesFailed(message.Upn);
            throw new InvalidOperationException("Error comunicating with Azure AD");
        }
    }
}

public static partial class UpdateBcProviderAttributesHandlerLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when updating attributes to User {upn} in Azure AD.")]
    public static partial void LogUpdateBcProviderAttributesFailed(this ILogger<UpdateBcProviderAttributesHandler> logger, string upn);
}
