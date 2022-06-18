using DDD.Api.Business.Adapter.DtoModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.DtoModelAdapter;
using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.Business.IntegratedAdapter.DtoModelAdapter;
public class DetalheUsuarioDtoModelIntegratedAdapter : IDetalheUsuarioDtoModelIntegratedAdapter
{
    public DetalheUsuarioDtoModel ToUsuarioDetalhadoComAcessos(UsuarioBusnModel usuarioBusnAdaptee, int idUsuarioAdminRoot)
    {
        DetalheUsuarioDtoModel usuarioDetalhado = new DetalheUsuarioDtoModelAdapter(usuarioBusnAdaptee);
        usuarioDetalhado.Eh_admin_root = usuarioDetalhado.Id == idUsuarioAdminRoot;
        usuarioDetalhado.Acessos = new List<AcessoUsuarioDtoModel>();
        foreach (var acessoAdaptee in usuarioBusnAdaptee.Acessos)
        {
            AcessoUsuarioDtoModel acessoUsuarioDto = new AcessoUsuarioDtoModelAdapter(acessoAdaptee);
            usuarioDetalhado.Acessos.Add(acessoUsuarioDto);
        }
        return usuarioDetalhado;
    }
}