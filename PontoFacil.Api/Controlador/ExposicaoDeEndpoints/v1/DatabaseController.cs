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
    private readonly UsuarioRepositorio _usuarioRepositorio;
    private readonly SessaoRepositorio _sessaoRepositorio;
    public DatabaseController(DatabaseRepositorio databaseRepositorio,
                              UsuarioConvertUnique usuarioConvertUnique,
                              UsuarioRepositorio usuarioRepositorio,
                              SessaoRepositorio sessaoRepositorio)
    {
        _databaseRepositorio = databaseRepositorio;
        _usuarioConvertUnique = usuarioConvertUnique;
        _usuarioRepositorio = usuarioRepositorio;
        _sessaoRepositorio = sessaoRepositorio;
    }

    [Autorizar]
    [HttpGet]
    [Route("exportar")]
    public async Task<IActionResult> Exportar()
    {
        var usuarioLogado = _usuarioConvertUnique.ExtrairUsuarioLogado(HttpContext.Request.Headers);
        _usuarioRepositorio.AutorizaUsuarioImportarExportar(usuarioLogado);

        var cargaBanco = _databaseRepositorio.ExportarBanco();
        await _sessaoRepositorio.AgendarBatchExclusaoSessoes();

        var json = new DevolvidoMensagensDTO();
        json.Devolvido = cargaBanco;
        json.SetMensagemUnica(Mensagens.REQUISICAO_SUCESSO);
        return StatusCode((int)HttpStatusCode.OK, json);
    }
    
    [Autorizar]
    [HttpPost]
    [Route("importar")]
    public async Task<IActionResult> Importar([FromBody]CargaBancoDTO cargaBanco)
    {
        var usuarioLogado = _usuarioConvertUnique.ExtrairUsuarioLogado(HttpContext.Request.Headers);
        _usuarioRepositorio.AutorizaUsuarioImportarExportar(usuarioLogado);
        
        await _databaseRepositorio.ImportarBanco(cargaBanco);

        var json = new DevolvidoMensagensDTO();
        json.SetMensagemUnica(Mensagens.REQUISICAO_SUCESSO);
        return StatusCode((int)HttpStatusCode.OK, json);
    }
}