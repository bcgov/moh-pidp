namespace Pidp.Infrastructure.HttpClients;

using IdentityModel.Client;

public class BearerTokenHandler<T> : DelegatingHandler where T : ClientCredentialsTokenRequest
{
    private readonly ClientCredentialsTokenRequest credentials;
    private readonly IAccessTokenClient tokenClient;

    public BearerTokenHandler(T credentials, IAccessTokenClient accessTokenClient)
    {
        this.credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        this.tokenClient = accessTokenClient ?? throw new ArgumentNullException(nameof(accessTokenClient));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // TODO: we could do access token caching/refreshing/etc. here
        var accessToken = await this.tokenClient.GetAccessTokenAsync(this.credentials);

        request.SetBearerToken(accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
