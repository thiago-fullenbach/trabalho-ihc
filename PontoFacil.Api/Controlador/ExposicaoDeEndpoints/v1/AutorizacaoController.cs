using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Middleware;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Controlador.Repositorio.Convert;
using PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1;
[ApiController]
[Route("api/v1/[controller]")]
public class AutorizacaoController : ControllerBase
{
    private readonly UsuarioRepositorio _usuarioRepositorio;
    private readonly SessaoRepositorio _sessaoRepositorio;
    private readonly SessaoConvertUnique _sessaoConvertUnique;
    private readonly UsuarioConvert _usuarioConvert;
    private readonly UsuarioConvertUnique _usuarioConvertUnique;
    private readonly PontoFacilContexto _contexto;
    public AutorizacaoController(UsuarioRepositorio usuarioRepositorio,
                                 SessaoRepositorio sessaoRepositorio,
                                 SessaoConvertUnique sessaoConvertUnique,
                                 UsuarioConvert usuarioConvert,
                                 UsuarioConvertUnique usuarioConvertUnique,
                                 PontoFacilContexto contexto)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _sessaoRepositorio = sessaoRepositorio;
        _sessaoConvertUnique = sessaoConvertUnique;
        _usuarioConvert = usuarioConvert;
        _usuarioConvertUnique = usuarioConvertUnique;
        _contexto = contexto;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody]LoginXSenhaDTO loginXSenha)
    {
        _usuarioRepositorio.ValidarLoginSenhaObrigatorios(loginXSenha);

        var usuario = _usuarioRepositorio.RecuperarUsuarioPeloLoginSenha(loginXSenha);
        var sessao = await _sessaoRepositorio.AbrirSessao(usuario.id);
        var asEnviarPeloHeader = _sessaoConvertUnique.ParaSessaoEnvioHeaderDTO(sessao);
        var detalhesUsuario = _usuarioConvert.ParaUsuarioLogadoDTO(usuario);
        HttpContext.Response.Headers["sessao"] = JsonConvert.SerializeObject(asEnviarPeloHeader);
        HttpContext.Response.Headers["usuario"] = JsonConvert.SerializeObject(detalhesUsuario);
        var json = new DevolvidoMensagensDTO();
        json.SetMensagemUnica(Mensagens.REQUISICAO_SUCESSO);
        return StatusCode((int)HttpStatusCode.OK, json);
    }

    [HttpPost]
    [Route("registrar")]
    public async Task<IActionResult> Registrar([FromBody]CadUsuarioCadastreSeDTO cadUsuario)
    {
        _usuarioRepositorio.ValidarCadastreSe(cadUsuario);

        var usuario = await _usuarioRepositorio.CriarUsuarioPeloCadastreSe(cadUsuario);
        var sessao = await _sessaoRepositorio.AbrirSessao(usuario.id);
        var asEnviarPeloHeader = _sessaoConvertUnique.ParaSessaoEnvioHeaderDTO(sessao);
        var detalhesUsuario = _usuarioConvert.ParaUsuarioLogadoDTO(usuario);
        HttpContext.Response.Headers["sessao"] = JsonConvert.SerializeObject(asEnviarPeloHeader);
        HttpContext.Response.Headers["usuario"] = JsonConvert.SerializeObject(detalhesUsuario);
        var json = new DevolvidoMensagensDTO();
        json.SetMensagemUnica(Mensagens.REQUISICAO_SUCESSO);
        return StatusCode((int)HttpStatusCode.OK, json);
    }

    [BatchExclusaoSessoes]
    [HttpPost]
    [Route("excluirSessoesExpiradas")]
    public async Task<IActionResult> ExcluirSessoesExpiradas()
    {
        await _sessaoRepositorio.ExcluirSessoesExpiradas();
        var json = new DevolvidoMensagensDTO();
        json.SetMensagemUnica(Mensagens.REQUISICAO_SUCESSO);
        return StatusCode((int)HttpStatusCode.OK, json);
    }
}
