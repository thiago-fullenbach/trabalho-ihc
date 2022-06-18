using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Domain.Interface.Infra.Repositories;
public interface IUsuarioRepository : IRepositoryBase<Usuario>
{
    Task InsertAcessosAsync(int idUsuario, List<Acesso> acessos);
    Task UpdateAcessosAsync(int idUsuario, List<Acesso> acessos);
    Task<Usuario?> SelectByLoginOrDefaultAsync(string login);
    Task<Usuario?> SelectByCPFOrDefaultAsync(string cpf);
    Task ExcluirSessoesAsync(int idUsuario);
}