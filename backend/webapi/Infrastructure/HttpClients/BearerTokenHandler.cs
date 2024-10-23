namespace Pidp.Infrastructure.HttpClients;

using IdentityModel.Client;

public class BearerTokenHandler<T>(T credentials, IAccessTokenClient accessTokenClient) : DelegatingHandler where T : ClientCredentialsTokenRequest
{
    private readonly ClientCredentialsTokenRequest credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
    private readonly IAccessTokenClient tokenClient = accessTokenClient ?? throw new ArgumentNullException(nameof(accessTokenClient));

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // TODO: we could do access token caching/refreshing/etc. here
        var accessToken = await this.tokenClient.GetAccessTokenAsync(this.credentials);

        request.SetBearerToken(accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
