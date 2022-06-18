using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.Business.Adapter.DtoModelAdapter;
public class AcessoUsuarioLogadoDtoModelAdapter : AcessoUsuarioLogadoDtoModel
{
    public AcessoUsuarioLogadoDtoModelAdapter(AcessoBusnModel acessoBusnModelAdaptee)
    {
        Recurso_cod_en = acessoBusnModelAdaptee.CodigoRecurso;
        Eh_habilitado = acessoBusnModelAdaptee.EhHabilitado;
    }
}