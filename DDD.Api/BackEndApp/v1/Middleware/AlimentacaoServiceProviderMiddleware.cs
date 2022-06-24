using DDD.Api.Domain.Interface.Business.Services.DataServices;

namespace DDD.Api.BackEndApp.v1.Middleware;
public class AlimentacaoServiceProviderMiddleware
{
    public static async Task ProcessarAsync(HttpContext context, Func<Task> next)
    {
        var services = context.RequestServices;
        var serviceProviderDataService = services.GetService<IServiceProviderDataService>();
        serviceProviderDataService.SetUltimoServiceProvider(services);
        await next();
    }
}