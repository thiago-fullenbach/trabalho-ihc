using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Domain.Interface.Business.IntegratedAdapter.BusnModelAdapter;
public interface ISessaoBusnModelIntegratedAdapter
{
    Task<SessaoBusnModel> ToNovaSessaoAutenticadaAsync(Sessao sessaoAdaptee);
    Task<SessaoBusnModel> ToSessaoAutenticadaJaExistenteAsync(SessaoEnvioHeaderDtoModel sessaoEnvioHeaderAdaptee);
}