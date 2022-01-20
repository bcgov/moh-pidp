namespace Pidp.Infrastructure.HttpClients;

using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.Plr;

public static class HttpClientSetup
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, PidpConfiguration config)
    {
        services.AddHttpClientWithBaseAddress<IPlrClient, PlrClient>(config.PlrClient.Url);

        return services;
    }
}
