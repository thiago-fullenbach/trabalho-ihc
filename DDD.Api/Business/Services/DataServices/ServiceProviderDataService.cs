using DDD.Api.Business.Services.DataServices.OutsideDI;
using DDD.Api.Domain.Interface.Business.Services.DataServices;

namespace DDD.Api.Business.Services.DataServices;
public class ServiceProviderDataService : IServiceProviderDataService
{
    private IServiceProvider? _serviceProvider;
    public IServiceProvider? GetUltimoServiceProvider()
    {
        return _serviceProvider;
    }

    public T? GetService<T>()
    {
        return _serviceProvider == null ? default(T) : _serviceProvider.GetService<T>();
    }

    public void SetUltimoServiceProvider(IServiceProvider services)
    {
        _serviceProvider = services;
    }
}