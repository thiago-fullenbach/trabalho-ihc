using System.Net;
using Microsoft.AspNetCore.Mvc;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Middleware;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
using PontoFacil.Api.Modelo;

namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1;
[ApiController]
[Route("api/v1/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuariosRepositorio _usuariosRepositorio;
    private readonly UsuarioConvertUnique _usuarioConvertUnique;
    public UsuarioController(UsuariosRepositorio usuariosRepositorio, UsuarioConvertUnique usuarioConvertUnique)
    {
        _usuariosRepositorio = usuariosRepositorio;
        _usuarioConvertUnique = usuarioConvertUnique;
    }

    [Autorizar]
    [HttpGet]
    [Route("listarTodos")]
    public IActionResult ListarTodos()
    {
        // var usuarioLogado = _usuarioConvertUnique.ExtrairUsuarioLogado(HttpContext.Request.Headers);
        // _usuariosRepositorio.AutorizaUsuario(usuarioLogado, x => x.Pode_vis_demais_usuarios);

        var listaUsuarios = _usuariosRepositorio.RecuperarUsuariosPeloFiltro(new FiltroUsuarioDTO());
        var listaUsuariosEmDTO = new List<UsuarioPesquisadoDTO>();
        foreach (var iUsuario in listaUsuarios)
            { listaUsuariosEmDTO.Add(_usuarioConvertUnique.ParaUsuarioPesquisadoDTO(iUsuario)); }
        
        return StatusCode((int)HttpStatusCode.OK, new DevolvidoMensagemDTO { Devolvido = listaUsuariosEmDTO, Mensagem = Mensagens.REQUISICAO_SUCESSO });
    }
}
