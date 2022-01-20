namespace Pidp.Infrastructure.HttpClients;

public static class HttpClientSetup
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, PidpConfiguration config)
    {
        return services;
    }
}
