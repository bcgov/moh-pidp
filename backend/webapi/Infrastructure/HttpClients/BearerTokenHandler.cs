namespace Pidp.Infrastructure.HttpClients;

using IdentityModel.Client;

public class BearerTokenHandler<T> : DelegatingHandler where T : ClientCredentialsTokenRequest
{
    private readonly IAccessTokenClient tokenClient;
    private readonly ClientCredentialsTokenRequest credentials;

    public BearerTokenHandler(IAccessTokenClient accessTokenClient, T credentials)
    {
        this.tokenClient = accessTokenClient ?? throw new ArgumentNullException(nameof(accessTokenClient));
        this.credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // TODO: we could do access token caching/refreshing/etc. here
        var accessToken = await this.tokenClient.GetAccessTokenAsync(this.credentials);

        request.SetBearerToken(accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
