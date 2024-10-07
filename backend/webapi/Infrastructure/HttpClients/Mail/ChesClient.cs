namespace Pidp.Infrastructure.HttpClients.Mail;

public class ChesClient(HttpClient httpClient, ILogger<ChesClient> logger) : BaseClient(httpClient, logger), IChesClient
{

    public async Task<Guid?> SendAsync(Email email)
    {
        var result = await this.PostAsync<EmailSuccessResponse>("email", new ChesEmailRequestParams(email));
        if (!result.IsSuccess)
        {
            return null;
        }

        return result.Value.Messages.Single().MsgId;
    }

    public async Task<string?> GetStatusAsync(Guid msgId)
    {
        var result = await this.GetWithQueryParamsAsync<IEnumerable<StatusResponse>>("status", new { msgId });
        if (!result.IsSuccess)
        {
            return null;
        }

        return result.Value.Single().Status;
    }

    public async Task<bool> HealthCheckAsync()
    {
        var result = await this.SendCoreAsync(HttpMethod.Get, "health", null, default);
        return result.IsSuccess;
    }
}
