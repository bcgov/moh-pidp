namespace Pidp.Infrastructure.HttpClients.Mail;

public class ChesClient : BaseClient, IChesClient
{
    public ChesClient(HttpClient httpClient, ILogger<ChesClient> logger) : base(httpClient, logger) { }

    public async Task<Guid?> SendAsync(Email email)
    {
        var result = await this.PostAsync<EmailSuccessResponse>("email", new ChesEmailRequestParams(email));

        if (!result.IsSuccess)
        {
            // this.Logger.LogError($"CHES Response code: {(int)result.StatusCode}, response body:{responseJsonString}");
            return null;
        }

        return result.Value.Messages.Single().MsgId;
    }

    public async Task<string?> GetStatusAsync(Guid msgId)
    {
        var result = await this.GetAsync<IEnumerable<StatusResponse>>($"status?msgId={msgId}");

        if (!result.IsSuccess)
        {
            // this.Logger.LogError($"CHES Response code: {(int)response.StatusCode}, response body:{responseString}");
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
