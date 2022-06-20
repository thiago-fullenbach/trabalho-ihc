using System.Net;
using DDD.Api.BackEndApp.v1.ApiServices;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.DtoModelAdapter;
using DDD.Api.Domain.Interface.Business.Services;
using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using Microsoft.AspNetCore.Mvc;

namespace DDD.Api.BackEndApp.v1;
[ApiController]
[Route("api/v1/[controller]")]
public class PresencaController : ControllerBase
{
    private readonly HeaderHandler _headerHandler;
    private readonly IAutorizacaoService _autorizacaoService;
    private readonly IPresencaService _presencaService;
    private readonly IPresencaPesquisadaDtoModelIntegratedAdapter _presencaPesquisadaDtoModelIntegratedAdapter;
    private readonly IPresencaBusnModelIntegratedAdapter _presencaBusnModelIntegratedAdapter;
    public PresencaController(HeaderHandler headerHandler,
                              IAutorizacaoService autorizacaoService,
                              IPresencaService presencaService,
                              IPresencaPesquisadaDtoModelIntegratedAdapter presencaPesquisadaDtoModelIntegratedAdapter,
                              IPresencaBusnModelIntegratedAdapter presencaBusnModelIntegratedAdapter)
    {
        _headerHandler = headerHandler;
        _autorizacaoService = autorizacaoService;
        _presencaService = presencaService;
        _presencaPesquisadaDtoModelIntegratedAdapter = presencaPesquisadaDtoModelIntegratedAdapter;
        _presencaBusnModelIntegratedAdapter = presencaBusnModelIntegratedAdapter;
    }

    [HttpGet]
    [Route("listarTodas")]
    public async Task<IActionResult> ListarTodas()
    {
        await _headerHandler.EscreverNoDataServiceAsync(HttpContext.Request.Headers);
        await _autorizacaoService.ReautenticarSessaoAsync();
        var presencas = await _presencaService.ListarTodasAsync();
        var presencasDto = new List<PresencaPesquisadaDtoModel>();
        foreach (var presenca in presencas)
        {
            PresencaPesquisadaDtoModel presencaDto = _presencaPesquisadaDtoModelIntegratedAdapter.ToPresencaPesquisada(presenca);
            presencasDto.Add(presencaDto);
        }
        await _autorizacaoService.ReautenticarSessaoAsync();
        _headerHandler.EscreverNoHeader(HttpContext.Response.Headers);
        return ControllerUtils.StatusCodeDevolvidoMensagens(this, (int)HttpStatusCode.OK, presencasDto, $"Exibindo um total de presenças de {presencasDto.Count}.");
    }

    [HttpGet]
    [Route("completarNovaPresencaEhEntrada")]
    public async Task<IActionResult> CompletarNovaPresencaEhEntrada()
    {
        await _headerHandler.EscreverNoDataServiceAsync(HttpContext.Request.Headers);
        await _autorizacaoService.ReautenticarSessaoAsync();
        var novaPresencaEhEntrada = await _presencaService.NovaPresencaEhEntradaAsync();
        await _autorizacaoService.ReautenticarSessaoAsync();
        _headerHandler.EscreverNoHeader(HttpContext.Response.Headers);
        var tipoPresenca = novaPresencaEhEntrada ? "Início Trabalho" : "Fim Trabalho";
        return ControllerUtils.StatusCodeDevolvidoMensagens(this, (int)HttpStatusCode.OK, novaPresencaEhEntrada, $"A nova presença é {tipoPresenca}.");
    }

    [HttpPost]
    [Route("insere")]
    public async Task<IActionResult> Insere([FromBody]NovaPresencaDtoModel novaPresenca)
    {
        await _headerHandler.EscreverNoDataServiceAsync(HttpContext.Request.Headers);
        await _autorizacaoService.ReautenticarSessaoAsync();
        PresencaBusnModel novaPresencaBusn = _presencaBusnModelIntegratedAdapter.ToNovaPresenca(novaPresenca);
        await _presencaService.CreateAsync(novaPresencaBusn);
        await _autorizacaoService.ReautenticarSessaoAsync();
        _headerHandler.EscreverNoHeader(HttpContext.Response.Headers);
        return ControllerUtils.StatusCodeDevolvidoMensagens(this, (int)HttpStatusCode.OK, null, $"Presença incluída com sucesso.");
    }

    [HttpPost]
    [Route("darVisto")]
    public async Task<IActionResult> DarVisto(int id)
    {
        await _headerHandler.EscreverNoDataServiceAsync(HttpContext.Request.Headers);
        await _autorizacaoService.ReautenticarSessaoAsync();
        await _presencaService.DarVistoAsync(id);
        await _autorizacaoService.ReautenticarSessaoAsync();
        _headerHandler.EscreverNoHeader(HttpContext.Response.Headers);
        return ControllerUtils.StatusCodeDevolvidoMensagens(this, (int)HttpStatusCode.OK, null, $"Presença vistada com sucesso.");
    }
}