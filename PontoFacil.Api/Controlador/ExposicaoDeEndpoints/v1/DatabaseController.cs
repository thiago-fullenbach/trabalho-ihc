using System.Net;
using Microsoft.AspNetCore.Mvc;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Middleware;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;

namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1;
[ApiController]
[Route("api/v1/[controller]")]
public class DatabaseController : ControllerBase
{
    public static bool IsDevelopment = false;
    private readonly DatabaseRepositorio _databaseRepositorio;
    private readonly UsuarioConvertUnique _usuarioConvertUnique;
    private readonly UsuariosRepositorio _usuariosRepositorio;
    public DatabaseController(DatabaseRepositorio databaseRepositorio,
                              UsuarioConvertUnique usuarioConvertUnique,
                              UsuariosRepositorio usuariosRepositorio)
    {
        _databaseRepositorio = databaseRepositorio;
        _usuarioConvertUnique = usuarioConvertUnique;
        _usuariosRepositorio = usuariosRepositorio;
    }

    [Autorizar]
    [HttpGet]
    [Route("exportar")]
    public IActionResult Exportar()
    {
        var usuarioLogado = _usuarioConvertUnique.ExtrairUsuarioLogado(HttpContext.Request.Headers);
        _usuariosRepositorio.AutorizaUsuarioImportarExportar(usuarioLogado);

        var cargaBanco = _databaseRepositorio.ExportarBanco();

        return StatusCode((int)HttpStatusCode.OK, new DevolvidoMensagemDTO { Devolvido = cargaBanco, Mensagem = Mensagens.REQUISICAO_SUCESSO });
    }
    
    [Autorizar]
    [HttpPost]
    [Route("importar")]
    public async Task<IActionResult> Importar([FromBody]CargaBancoDTO cargaBanco)
    {
        var usuarioLogado = _usuarioConvertUnique.ExtrairUsuarioLogado(HttpContext.Request.Headers);
        _usuariosRepositorio.AutorizaUsuarioImportarExportar(usuarioLogado);
        
        await _databaseRepositorio.ImportarBanco(cargaBanco);

        return StatusCode((int)HttpStatusCode.OK, new DevolvidoMensagemDTO { Mensagem = Mensagens.REQUISICAO_SUCESSO });
    }
}