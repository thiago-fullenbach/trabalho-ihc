using DDD.Api.BackEndApp.v1.ApiServices;

namespace DDD.Api.BackEndApp.InversionOfControl;
public static class BackEndInversionOfControl
{
    public static IServiceCollection AdicionarModuloBackEnd(this IServiceCollection services)
    {
        services.AddScoped<HeaderHandler>();
        return services;
    }
}