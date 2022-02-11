namespace Pidp.Infrastructure.HttpClients.Mail;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class ChesClient : BaseClient, IChesClient
{
    public ChesClient(HttpClient httpClient, ILogger<ChesClient> logger) : base(httpClient, logger, PropertySerialization.CamelCase) { }

    public async Task<Guid?> SendAsync(Email email)
    {
        try
        {
            var response = await this.Client.PostAsync("email", this.CreateStringContent(new ChesEmailRequestParams(email)));
            var responseJsonString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var successResponse = JsonSerializer.Deserialize<EmailSuccessResponse>(responseJsonString);
                return successResponse?.Messages.Single().MsgId;
            }

            this.Logger.LogError($"CHES Response code: {(int)response.StatusCode}, response body:{responseJsonString}");
            return null;
        }
        catch (Exception ex)
        {
            this.Logger.LogError($"CHES Exception: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> GetStatusAsync(Guid msgId)
    {
        try
        {
            var response = await this.Client.GetAsync($"status?msgId={msgId}");
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var statusResponse = JsonSerializer.Deserialize<IEnumerable<StatusResponse>>(responseString);
                return statusResponse?.Single().Status;
            }

            this.Logger.LogError($"CHES Response code: {(int)response.StatusCode}, response body:{responseString}");
            return null;
        }
        catch (Exception ex)
        {
            this.Logger.LogError($"CHES Exception: {ex.Message}");
            throw new Exception("Error occurred when calling CHES Email API. Try again later.", ex);
        }
    }

    public async Task<bool> HealthCheckAsync()
    {
        try
        {
            var response = await this.Client.GetAsync("health");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            this.Logger.LogError($"Error occurred when calling CHES Healthcheck: {ex.Message}");
            return false;
        }
    }
}
