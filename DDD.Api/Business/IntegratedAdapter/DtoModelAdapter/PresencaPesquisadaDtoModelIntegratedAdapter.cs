using DDD.Api.Business.Adapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.DtoModelAdapter;
using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.Business.IntegratedAdapter.DtoModelAdapter;
public class PresencaPesquisadaDtoModelIntegratedAdapter : IPresencaPesquisadaDtoModelIntegratedAdapter
{
    public PresencaPesquisadaDtoModel ToPresencaPesquisada(PresencaBusnModel presencaBusnAdaptee)
    {
        PresencaPesquisadaDtoModel presencaDto = new PresencaPesquisadaDtoModelAdapter(presencaBusnAdaptee);
        presencaDto.Usuario_nome = presencaBusnAdaptee.UsuarioPresente.Nome;
        presencaDto.Local_resumo = $"{presencaBusnAdaptee.Local.NomeLogradouro} {presencaBusnAdaptee.Local.NumeroLogradouro}";
        return presencaDto;
    }
}