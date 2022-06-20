using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Domain.Interface.Infra.Repositories;
public interface ILocalRepository : IRepositoryBase<Local>
{
    Task<Local?> SelectLocalSeJaCadastradoOrDefaultAsync(Local local);
}