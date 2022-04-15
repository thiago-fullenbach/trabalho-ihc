using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Modelo;

namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1;
[ApiController]
[Route("api/v1/[controller]")]
public class AutorizacaoController : ControllerBase
{
    UsuarioRepositorio _usuarioR;
    SessaoAbertaRepositorio _sessaoAbertaR;
    RecursoRepositorio _recursoR;
    ContaRepositorio _contaR;
    public AutorizacaoController(SessaoAbertaRepositorio sessaoAbertaR,
                                 UsuarioRepositorio usuarioR,
                                 RecursoRepositorio recursoR,
                                 ContaRepositorio contaR)
    {
        _sessaoAbertaR = sessaoAbertaR;
        _usuarioR = usuarioR;
        _recursoR = recursoR;
        _contaR = contaR;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody]LoginXSenhaDTO loginXSenha)
    {
        var saida_sessaoAtualizada = new SessaoAtualizadaDTO();
        if (loginXSenha.Login == null || loginXSenha.Senha == null)
            return StatusCode(((int)HttpStatusCode.BadRequest), new DevolvidoMensagemDTO { Mensagem = Mensagens.ERRO_PARAMETROS_INVALIDOS });
        
        var doSql_conta = _contaR.ObterContaPeloLoginXSenha(loginXSenha.Login, loginXSenha.Senha);
        if (doSql_conta == null)
            return StatusCode(((int)HttpStatusCode.NotFound), new DevolvidoMensagemDTO { Mensagem = Mensagens.LOGIN_SENHA_INVALIDOS });
        
        var doSql_novaSessao = await _sessaoAbertaR.AbrirSessaoDoUsuario(doSql_conta.IdUsuario);
        saida_sessaoAtualizada.Id = doSql_novaSessao.Id;
        saida_sessaoAtualizada.HexVerificacao = doSql_novaSessao.HexVerificacao;
        saida_sessaoAtualizada.DataHoraUltimaAtualizacao = doSql_novaSessao.DataHoraUltimaAtualizacao;
        var doSql_recursosPermitidos = _recursoR.ListarRecursosAcessadosPeloUsuario(doSql_conta.IdUsuario);
        saida_sessaoAtualizada.ListaRecursosPermitidos = doSql_recursosPermitidos.Select(x => new RecursoPermitidoDTO
        {
            Id = x.Id,
            Nome = x.Nome
        }).ToList();

        string saida_jsonDadosSessao = JsonConvert.SerializeObject(saida_sessaoAtualizada);
        this.HttpContext.Response.Headers["sessao"] = saida_jsonDadosSessao;

        return StatusCode((int)HttpStatusCode.OK, new DevolvidoMensagemDTO { Mensagem = Mensagens.SUCESSO });
    }

    [HttpPost]
    [Route("registrar")]
    public async Task<IActionResult> Registrar([FromBody]ManterUsuarioComContaDTO usuarioComConta)
    {
        var saida_sessaoAtualizada = new SessaoAtualizadaDTO();
        if (string.IsNullOrWhiteSpace(usuarioComConta.Nome)
            || string.IsNullOrWhiteSpace(usuarioComConta.CPF)
            || !usuarioComConta.DataNascimento.HasValue
            || string.IsNullOrWhiteSpace(usuarioComConta.Login)
            || string.IsNullOrWhiteSpace(usuarioComConta.Senha))
            return StatusCode(((int)HttpStatusCode.BadRequest), new DevolvidoMensagemDTO { Mensagem = Mensagens.ERRO_PARAMETROS_INVALIDOS });
        
        if (_contaR.LoginExiste(usuarioComConta.Login))
            return StatusCode(((int)HttpStatusCode.BadRequest), new DevolvidoMensagemDTO { Mensagem = Mensagens.LOGIN_EM_USO });

        var doSql_usuarios = _usuarioR.ObterUsuarioPeloFiltro(new FiltrarUsuarioDTO { CPF = usuarioComConta.CPF });
        var inSql_usuario = new ManterUsuarioComContaDTO
        {
            Nome = usuarioComConta.Nome,
            CPF = usuarioComConta.CPF,
            DataNascimento = usuarioComConta.DataNascimento,
            Login = usuarioComConta.Login,
            Senha = usuarioComConta.Senha
        };
        Usuario? doSql_novoUsuario = null;
        if (doSql_usuarios.Count == 0)
        {
            doSql_novoUsuario = await _usuarioR.RegistrarUsuarioEConta(inSql_usuario);
        }
        else
        {
            var doSql_usuario = doSql_usuarios[0];
            if (_contaR.UsuarioTemConta(doSql_usuario.Id))
                return StatusCode(((int)HttpStatusCode.BadRequest), new DevolvidoMensagemDTO { Mensagem = Mensagens.ERRO_PESSOA_POSSUI_CONTA });
            else
                doSql_novoUsuario = await _usuarioR.RegistrarContaDeUsuarioJaExistente(doSql_usuario.Id, inSql_usuario);
        }
        if (doSql_novoUsuario == null) return StatusCode((int)HttpStatusCode.InternalServerError);
        
        var doSql_novaSessao = await _sessaoAbertaR.AbrirSessaoDoUsuario(doSql_novoUsuario.Id);
        saida_sessaoAtualizada.Id = doSql_novaSessao.Id;
        saida_sessaoAtualizada.HexVerificacao = doSql_novaSessao.HexVerificacao;
        saida_sessaoAtualizada.DataHoraUltimaAtualizacao = doSql_novaSessao.DataHoraUltimaAtualizacao;
        var doSql_recursosPermitidos = _recursoR.ListarRecursosAcessadosPeloUsuario(doSql_novoUsuario.Id);
        saida_sessaoAtualizada.ListaRecursosPermitidos = doSql_recursosPermitidos.Select(x => new RecursoPermitidoDTO
        {
            Id = x.Id,
            Nome = x.Nome
        }).ToList();
        
        string saida_jsonDadosSessao = JsonConvert.SerializeObject(saida_sessaoAtualizada);
        this.HttpContext.Response.Headers["sessao"] = saida_jsonDadosSessao;

        return StatusCode((int)HttpStatusCode.OK, new DevolvidoMensagemDTO { Mensagem = Mensagens.SUCESSO });
    }
}
