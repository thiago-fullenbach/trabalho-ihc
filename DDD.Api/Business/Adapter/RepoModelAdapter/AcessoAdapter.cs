using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.Adapter.RepoModelAdapter;
public class AcessoAdapter : Acesso
{
    public AcessoAdapter(AcessoBusnModel acessoBusnModelAdaptee)
    {
        usuario_id = acessoBusnModelAdaptee.IdUsuario.ToString();
        recurso_cod_en = acessoBusnModelAdaptee.CodigoRecurso;
        eh_habilitado = acessoBusnModelAdaptee.EhHabilitado;
    }
}