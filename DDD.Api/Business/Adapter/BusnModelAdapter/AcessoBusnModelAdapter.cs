using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.Adapter.BusnModelAdapter;
public class AcessoBusnModelAdapter : AcessoBusnModel
{
    public AcessoBusnModelAdapter(Acesso acessoAdaptee)
    {
        IdUsuario = acessoAdaptee.Id.ParseZeroIfFails();
        CodigoRecurso = acessoAdaptee.recurso_cod_en;
        EhHabilitado = acessoAdaptee.eh_habilitado ?? false;
    }

    public AcessoBusnModelAdapter(AcessoUsuarioDtoModel acessoUsuarioDtoAdaptee)
    {
        CodigoRecurso = acessoUsuarioDtoAdaptee.Recurso_cod_en;
        EhHabilitado = acessoUsuarioDtoAdaptee.Eh_habilitado ?? false;
    }
}