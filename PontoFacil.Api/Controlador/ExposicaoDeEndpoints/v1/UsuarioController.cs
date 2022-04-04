using System.Net;
using Microsoft.AspNetCore.Mvc;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.FilterComoMiddleware;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Modelo;

namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1;
[ApiController]
[Route("api/v1/[controller]")]
public class UsuarioController : ControllerBase
{
    UsuarioRepositorio _usuarioR;
    public UsuarioController(UsuarioRepositorio usuarioR)
    {
        _usuarioR = usuarioR;
    }

    [Autorizar]
    [HttpGet]
    [Route("listarTodos")]
    public IActionResult ListarTodos()
    {
        var saida_resultadosPesquisa = new List<UsuarioPesquisadoDTO>();
        var doSql_usuarios = _usuarioR.ObterUsuarioPeloFiltro(new FiltrarUsuarioDTO());
        saida_resultadosPesquisa.AddRange(doSql_usuarios.Select(x => new UsuarioPesquisadoDTO
        {
            Id = x.Id,
            Nome = x.Nome,
            CPF = x.CPF,
            DataNascimento = x.DataNascimento
        }));
        
        return StatusCode((int)HttpStatusCode.OK, new DevolvidoMensagemDTO { Devolvido = saida_resultadosPesquisa, Mensagem = Mensagens.SUCESSO });
    }
}
