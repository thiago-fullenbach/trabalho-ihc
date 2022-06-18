using DDD.Api.Business.Adapter.DtoModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.DtoModelAdapter;
using DDD.Api.Domain.Interface.Infra.Configuration.BackEndApp;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.Business.IntegratedAdapter.DtoModelAdapter;
public class UsuarioPesquisadoDtoModelIntegratedAdapter : IUsuarioPesquisadoDtoModelIntegratedAdapter
{
    public UsuarioPesquisadoDtoModel ToUsuarioListado(UsuarioBusnModel usuarioBusnAdaptee, int idUsuarioAdminRoot)
    {
        UsuarioPesquisadoDtoModel usuarioListado = new UsuarioPesquisadoDtoModelAdapter(usuarioBusnAdaptee);
        usuarioListado.Eh_admin_root = usuarioListado.Id == idUsuarioAdminRoot;
        return usuarioListado;
    }
}