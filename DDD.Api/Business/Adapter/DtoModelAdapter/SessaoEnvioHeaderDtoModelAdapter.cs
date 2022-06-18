using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.Business.Adapter.DtoModelAdapter;
public class SessaoEnvioHeaderDtoModelAdapter : SessaoEnvioHeaderDtoModel
{
    public SessaoEnvioHeaderDtoModelAdapter(SessaoBusnModel sessaoBusnModelAdaptee)
    {
        Id = sessaoBusnModelAdaptee.Id;
        Hex_verificacao = sessaoBusnModelAdaptee.CodigoVerificacao;
        Datahora_ultima_autenticacao = sessaoBusnModelAdaptee.UltimaAutenticacao;
    }
}