using DDD.Api.Business.Adapter.DtoModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.DtoModelAdapter;
using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.Business.IntegratedAdapter.DtoModelAdapter;
public class UsuarioLogadoDtoModelIntegratedAdapter : IUsuarioLogadoDtoModelIntegratedAdapter
{
    public UsuarioLogadoDtoModel ToUsuarioLogadoDtoModelSessao(UsuarioBusnModel usuarioBusnModelAdaptee)
    {
        UsuarioLogadoDtoModel usuarioLogadoDto = new UsuarioLogadoDtoModelAdapter(usuarioBusnModelAdaptee);
        usuarioLogadoDto.Acessos = new List<AcessoUsuarioLogadoDtoModel>();
        foreach (var acessoAdaptee in usuarioBusnModelAdaptee.Acessos)
        {
            AcessoUsuarioLogadoDtoModel acessoUsuarioLogadoDto = new AcessoUsuarioLogadoDtoModelAdapter(acessoAdaptee);
            usuarioLogadoDto.Acessos.Add(acessoUsuarioLogadoDto);
        }
        return usuarioLogadoDto;
    }
}