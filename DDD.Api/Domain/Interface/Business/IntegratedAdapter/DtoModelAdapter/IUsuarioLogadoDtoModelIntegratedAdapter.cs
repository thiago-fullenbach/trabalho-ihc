using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.Domain.Interface.Business.IntegratedAdapter.DtoModelAdapter;
public interface IUsuarioLogadoDtoModelIntegratedAdapter
{
    UsuarioLogadoDtoModel ToUsuarioLogadoDtoModelSessao(UsuarioBusnModel usuarioBusnModelAdaptee);
}