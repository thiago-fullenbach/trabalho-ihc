using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Domain.Interface.Infra.Repositories;
public interface IAcessoRepository : IRepositoryBase<Acesso>
{
    Task<List<Acesso>> SelectByUsuarioAsync(int idUsuario);
}