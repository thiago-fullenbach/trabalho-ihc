using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Domain.Interface.Infra.Repositories;
public interface IPresencaRepository : IRepositoryBase<Presenca>
{
    Task<bool> NovaPresencaUsuarioEhEntradaAsync(int idUsuario);
}