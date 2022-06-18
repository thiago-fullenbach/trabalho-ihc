using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Domain.Interface.Infra.Repositories;
public interface ISessaoRepository : IRepositoryBase<Sessao>
{
    string NovoCodigoVerificacao();
}