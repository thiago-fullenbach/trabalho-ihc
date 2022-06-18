using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Domain.Interface.Business.IntegratedAdapter.BusnModelAdapter;
public interface IUsuarioBusnModelIntegratedAdapter
{
    Task<UsuarioBusnModel> ToUsuarioDetalhadoAsync(Usuario usuarioAdaptee);
    UsuarioBusnModel ToNovoUsuario(NovoUsuarioDtoModel novoUsuarioAdaptee);
    UsuarioBusnModel ToAlterarUsuario(EditarUsuarioDtoModel editarUsuarioAdaptee);
}