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
    private readonly UsuariosRepositorio _usuariosRepositorio;
    private readonly SessoesRepositorio _sessoesRepositorio;
    private readonly SessaoConvertUnique _sessaoConvertUnique;
    private readonly UsuarioConvert _usuarioConvert;
    private readonly UsuarioConvertUnique _usuarioConvertUnique;
    private readonly PontoFacilContexto _contexto;
    public AutorizacaoController(UsuariosRepositorio usuariosRepositorio,
                                 SessoesRepositorio sessoesRepositorio,
                                 SessaoConvertUnique sessaoConvertUnique,
                                 UsuarioConvert usuarioConvert,
                                 UsuarioConvertUnique usuarioConvertUnique,
                                 PontoFacilContexto contexto)
    {
        _usuariosRepositorio = usuariosRepositorio;
        _sessoesRepositorio = sessoesRepositorio;
        _sessaoConvertUnique = sessaoConvertUnique;
        _usuarioConvert = usuarioConvert;
        _usuarioConvertUnique = usuarioConvertUnique;
        _contexto = contexto;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody]LoginXSenhaDTO loginXSenha)
    {
        _usuariosRepositorio.ValidarLoginSenhaObrigatorios(loginXSenha);

        var usuario = _usuariosRepositorio.RecuperarUsuarioPeloLoginSenha(loginXSenha);
        var sessao = await _sessoesRepositorio.AbrirSessao(usuario.id);
        var asEnviarPeloHeader = _sessaoConvertUnique.ParaSessaoEnviarPeloHeaderDTO(sessao);
        var detalhesUsuario = _usuarioConvert.ParaUsuarioLogadoDTO(usuario);
        HttpContext.Response.Headers["sessao"] = JsonConvert.SerializeObject(asEnviarPeloHeader);
        HttpContext.Response.Headers["usuario"] = JsonConvert.SerializeObject(detalhesUsuario);
        return StatusCode((int)HttpStatusCode.OK, new DevolvidoMensagemDTO { Mensagem = Mensagens.REQUISICAO_SUCESSO });
    }

    [HttpPost]
    [Route("registrar")]
    public async Task<IActionResult> Registrar([FromBody]CadUsuarioCadastreSeDTO cadUsuario)
    {
        _usuariosRepositorio.ValidarCadastreSe(cadUsuario);

        var usuario = await _usuariosRepositorio.CriarUsuarioPeloCadastreSe(cadUsuario);
        var sessao = await _sessoesRepositorio.AbrirSessao(usuario.id);
        var asEnviarPeloHeader = _sessaoConvertUnique.ParaSessaoEnviarPeloHeaderDTO(sessao);
        var detalhesUsuario = _usuarioConvert.ParaUsuarioLogadoDTO(usuario);
        HttpContext.Response.Headers["sessao"] = JsonConvert.SerializeObject(asEnviarPeloHeader);
        HttpContext.Response.Headers["usuario"] = JsonConvert.SerializeObject(detalhesUsuario);
        return StatusCode((int)HttpStatusCode.OK, new DevolvidoMensagemDTO { Mensagem = Mensagens.REQUISICAO_SUCESSO });
    }

    [BatchExclusaoSessoes]
    [HttpPost]
    [Route("excluirSessoesExpiradas")]
    public async Task<IActionResult> ExcluirSessoesExpiradas()
    {
        await _sessoesRepositorio.ExcluirSessoesExpiradas();
        return StatusCode((int)HttpStatusCode.OK, new DevolvidoMensagemDTO { Mensagem = Mensagens.REQUISICAO_SUCESSO });
    }
}
