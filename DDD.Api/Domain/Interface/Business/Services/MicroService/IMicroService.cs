namespace DDD.Api.Domain.Interface.Business.Services.MicroService;
public interface IMicroService
{
    Task<T?> GetNaoAutenticadoAsync<T>(string url);
}