using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.Adapter.BusnModelAdapter;
public class SessaoBusnModelAdapter : SessaoBusnModel
{
    public SessaoBusnModelAdapter(Sessao sessaoAdaptee)
    {
        Id = sessaoAdaptee.Id.ParseZeroIfFails();
        CodigoVerificacao = sessaoAdaptee.hex_verificacao;
        IdUsuario = sessaoAdaptee.usuario_id.ParseZeroIfFails();
        UltimaAutenticacao = sessaoAdaptee.datahora_ultima_autenticacao;
    }

    public SessaoBusnModelAdapter(SessaoEnvioHeaderDtoModel sessaoEnvioHeaderAdaptee)
    {
        Id = sessaoEnvioHeaderAdaptee.Id ?? 0;
        CodigoVerificacao = sessaoEnvioHeaderAdaptee.Hex_verificacao ?? string.Empty;
        UltimaAutenticacao = sessaoEnvioHeaderAdaptee.Datahora_ultima_autenticacao ?? default(DateTime);
    }
}