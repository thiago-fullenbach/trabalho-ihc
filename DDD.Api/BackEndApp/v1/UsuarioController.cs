using System.Net;
using DDD.Api.BackEndApp.v1.ApiServices;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.DtoModelAdapter;
using DDD.Api.Domain.Interface.Business.Services;
using DDD.Api.Domain.Models.DtoModel;
using Microsoft.AspNetCore.Mvc;

namespace DDD.Api.BackEndApp.v1;
[ApiController]
[Route("api/v1/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly HeaderHandler _headerHandler;
    private readonly IAutorizacaoService _autorizacaoService;
    private readonly IUsuarioService _usuarioService;
    private readonly IUsuarioPesquisadoDtoModelIntegratedAdapter _usuarioPesquisadoDtoModelIntegratedAdapter;
    private readonly IDetalheUsuarioDtoModelIntegratedAdapter _detalheUsuarioDtoModelIntegratedAdapter;
    private readonly IUsuarioBusnModelIntegratedAdapter _usuarioBusnModelIntegratedAdapter;
    public UsuarioController(HeaderHandler headerHandler,
                             IAutorizacaoService autorizacaoService, 
                             IUsuarioService usuarioService,
                             IUsuarioPesquisadoDtoModelIntegratedAdapter usuarioPesquisadoDtoModelIntegratedAdapter,
                             IDetalheUsuarioDtoModelIntegratedAdapter detalheUsuarioDtoModelIntegratedAdapter,
                             IUsuarioBusnModelIntegratedAdapter usuarioBusnModelIntegratedAdapter)
    {
        _headerHandler = headerHandler;
        _autorizacaoService = autorizacaoService;
        _usuarioService = usuarioService;
        _usuarioPesquisadoDtoModelIntegratedAdapter = usuarioPesquisadoDtoModelIntegratedAdapter;
        _detalheUsuarioDtoModelIntegratedAdapter = detalheUsuarioDtoModelIntegratedAdapter;
        _usuarioBusnModelIntegratedAdapter = usuarioBusnModelIntegratedAdapter;
    }

    [HttpGet]
    [Route("listarTodos")]
    public async Task<IActionResult> ListarTodos()
    {
        await _headerHandler.EscreverNoDataServiceAsync(HttpContext.Request.Headers);
        await _autorizacaoService.ReautenticarSessaoAsync();
        var usuarios = await _usuarioService.ListarTodosAsync();
        int idAdminRoot = await _usuarioService.GetIdUsuarioAdminRootAsync();
        var usuariosDto = new List<UsuarioPesquisadoDtoModel>();
        foreach (var usuario in usuarios)
        {
            UsuarioPesquisadoDtoModel usuarioDto = _usuarioPesquisadoDtoModelIntegratedAdapter.ToUsuarioListado(usuario, idAdminRoot);
            usuariosDto.Add(usuarioDto);
        }
        await _autorizacaoService.ReautenticarSessaoAsync();
        _headerHandler.EscreverNoHeader(HttpContext.Response.Headers);
        return ControllerUtils.StatusCodeDevolvidoMensagens(this, (int)HttpStatusCode.OK, usuariosDto, $"Exibindo um total de usuários de {usuariosDto.Count}.");
    }
    
    [HttpGet]
    [Route("pegaPeloId")]
    public async Task<IActionResult> PegaPeloId(int id)
    {
        await _headerHandler.EscreverNoDataServiceAsync(HttpContext.Request.Headers);
        await _autorizacaoService.ReautenticarSessaoAsync();
        var usuario = await _usuarioService.GetByIdAsync(id);
        int idAdminRoot = await _usuarioService.GetIdUsuarioAdminRootAsync();
        DetalheUsuarioDtoModel usuarioDto = _detalheUsuarioDtoModelIntegratedAdapter.ToUsuarioDetalhadoComAcessos(usuario, idAdminRoot);
        await _autorizacaoService.ReautenticarSessaoAsync();
        _headerHandler.EscreverNoHeader(HttpContext.Response.Headers);
        return ControllerUtils.StatusCodeDevolvidoMensagens(this, (int)HttpStatusCode.OK, usuarioDto, $"Exibindo detalhes do usuário {usuarioDto.Nome}.");
    }
    
    [HttpPost]
    [Route("excluiPeloId")]
    public async Task<IActionResult> ExcluiPeloId(int id)
    {
        await _headerHandler.EscreverNoDataServiceAsync(HttpContext.Request.Headers);
        await _autorizacaoService.ReautenticarSessaoAsync();
        await _usuarioService.RemoveAsync(id);
        await _autorizacaoService.ReautenticarSessaoAsync();
        _headerHandler.EscreverNoHeader(HttpContext.Response.Headers);
        return ControllerUtils.StatusCodeDevolvidoMensagens(this, (int)HttpStatusCode.OK, null, $"Usuário excluído com sucesso.");
    }
    
    [HttpPost]
    [Route("insere")]
    public async Task<IActionResult> Insere([FromBody]NovoUsuarioDtoModel novoUsuario)
    {
        await _headerHandler.EscreverNoDataServiceAsync(HttpContext.Request.Headers);
        await _autorizacaoService.ReautenticarSessaoAsync();
        var usuarioBusn = _usuarioBusnModelIntegratedAdapter.ToNovoUsuario(novoUsuario);
        var usuarioBusnRetorno = await _usuarioService.CreateAsync(usuarioBusn);
        int idAdminRoot = await _usuarioService.GetIdUsuarioAdminRootAsync();
        DetalheUsuarioDtoModel usuarioDto = _detalheUsuarioDtoModelIntegratedAdapter.ToUsuarioDetalhadoComAcessos(usuarioBusnRetorno, idAdminRoot);
        await _autorizacaoService.ReautenticarSessaoAsync();
        _headerHandler.EscreverNoHeader(HttpContext.Response.Headers);
        return ControllerUtils.StatusCodeDevolvidoMensagens(this, (int)HttpStatusCode.OK, usuarioDto, $"Usuário incluído com sucesso.");
    }

    [HttpPost]
    [Route("atualiza")]
    public async Task<IActionResult> Atualiza([FromBody]EditarUsuarioDtoModel editarUsuario)
    {
        await _headerHandler.EscreverNoDataServiceAsync(HttpContext.Request.Headers);
        await _autorizacaoService.ReautenticarSessaoAsync();
        var usuarioBusn = _usuarioBusnModelIntegratedAdapter.ToAlterarUsuario(editarUsuario);
        var usuarioBusnRetorno = await _usuarioService.UpdateAsync(usuarioBusn);
        int idAdminRoot = await _usuarioService.GetIdUsuarioAdminRootAsync();
        DetalheUsuarioDtoModel usuarioDto = _detalheUsuarioDtoModelIntegratedAdapter.ToUsuarioDetalhadoComAcessos(usuarioBusnRetorno, idAdminRoot);
        await _autorizacaoService.ReautenticarSessaoAsync();
        _headerHandler.EscreverNoHeader(HttpContext.Response.Headers);
        return ControllerUtils.StatusCodeDevolvidoMensagens(this, (int)HttpStatusCode.OK, usuarioDto, $"Usuário alterado com sucesso.");
    }
}