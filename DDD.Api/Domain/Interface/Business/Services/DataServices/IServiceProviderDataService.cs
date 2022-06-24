namespace DDD.Api.Domain.Interface.Business.Services.DataServices;
public interface IServiceProviderDataService
{
    IServiceProvider? GetUltimoServiceProvider();
    T? GetService<T>();
    void SetUltimoServiceProvider(IServiceProvider services);
}