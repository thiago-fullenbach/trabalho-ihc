using DDD.Api.Domain.Models.BusnModel;

namespace DDD.Api.Domain.Interface.Business.Services;
public interface IPresencaService
{
    Task<bool> NovaPresencaEhEntradaAsync();
    bool PodeVisualizarPresenca(PresencaBusnModel presenca);
    Task<List<PresencaBusnModel>> ListarTodasAsync();
    Task CreateAsync(PresencaBusnModel presenca);
    Task DarVistoAsync(int id);
}