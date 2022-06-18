using DDD.Api.Business.Adapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.IntegratedAdapter.BusnModelAdapter;
public class UsuarioBusnModelIntegratedAdapter : IUsuarioBusnModelIntegratedAdapter
{
    private readonly IAcessoRepository _acessoRepository;
    public UsuarioBusnModelIntegratedAdapter(IAcessoRepository acessoRepository)
    {
        _acessoRepository = acessoRepository;
    }
    public async Task<UsuarioBusnModel> ToUsuarioDetalhadoAsync(Usuario usuarioAdaptee)
    {
        UsuarioBusnModel usuarioDetalhado = new UsuarioBusnModelAdapter(usuarioAdaptee);
        var acessosUsuarioDetalhado = await _acessoRepository.SelectByUsuarioAsync(usuarioAdaptee.Id.ParseZeroIfFails());
        usuarioDetalhado.Acessos = new List<AcessoBusnModel>();
        foreach (var acessoUsuarioDetalhado in acessosUsuarioDetalhado)
        {
            AcessoBusnModel acessoBusn = new AcessoBusnModelAdapter(acessoUsuarioDetalhado);
            usuarioDetalhado.Acessos.Add(acessoBusn);
        }
        return usuarioDetalhado;
    }
    public UsuarioBusnModel ToNovoUsuario(NovoUsuarioDtoModel novoUsuarioAdaptee)
    {
        UsuarioBusnModel novoUsuario = new UsuarioBusnModelAdapter(novoUsuarioAdaptee);
        novoUsuario.Acessos = new List<AcessoBusnModel>();
        if (novoUsuarioAdaptee.Acessos == null)
        {
            novoUsuarioAdaptee.Acessos = new List<AcessoUsuarioDtoModel>();
        }
        foreach (var acessoAdaptee in novoUsuarioAdaptee.Acessos)
        {
            AcessoBusnModel acessoBusn = new AcessoBusnModelAdapter(acessoAdaptee);
            novoUsuario.Acessos.Add(acessoBusn);
        }
        return novoUsuario;
    }

    public UsuarioBusnModel ToAlterarUsuario(EditarUsuarioDtoModel editarUsuarioAdaptee)
    {
        UsuarioBusnModel alterarUsuario = new UsuarioBusnModelAdapter(editarUsuarioAdaptee);
        alterarUsuario.Acessos = new List<AcessoBusnModel>();
        if (editarUsuarioAdaptee.Acessos == null)
        {
            editarUsuarioAdaptee.Acessos = new List<AcessoUsuarioDtoModel>();
        }
        foreach (var acessoAdaptee in editarUsuarioAdaptee.Acessos)
        {
            AcessoBusnModel acessoBusn = new AcessoBusnModelAdapter(acessoAdaptee);
            alterarUsuario.Acessos.Add(acessoBusn);
        }
        return alterarUsuario;
    }
}