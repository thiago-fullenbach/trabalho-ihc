using System.Net;
using DDD.Api.BackEndApp.v1.ApiServices;
using DDD.Api.Business.Adapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Business.Services;
using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using Microsoft.AspNetCore.Mvc;

namespace DDD.Api.BackEndApp.v1;
[ApiController]
[Route("api/v1/[controller]")]
public class AutorizacaoController : ControllerBase
{
    private readonly IAutorizacaoService _autorizacaoService;
    private readonly HeaderHandler _headerHandler;
    public AutorizacaoController(IAutorizacaoService autorizacaoService, HeaderHandler headerHandler)
    {
        _autorizacaoService = autorizacaoService;
        _headerHandler = headerHandler;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody]LoginXSenhaDtoModel loginXSenha)
    {
        UsuarioBusnModel usuarioBusnModel = new UsuarioBusnModelAdapter(loginXSenha);
        await _autorizacaoService.LogarAsync(usuarioBusnModel);
        _headerHandler.EscreverNoHeader(HttpContext.Response.Headers);
        return ControllerUtils.StatusCodeDevolvidoMensagens(this, (int)HttpStatusCode.OK, null, "Usuário logado com sucesso.");
    }

    [HttpGet]
    [Route("hasheiaSenha")]
    public IActionResult HasheiaSenha(string senhaRaw)
    {
        string senhaCriptografada = _autorizacaoService.CriptografarSenhaAppInterno(senhaRaw);
        return ControllerUtils.StatusCodeDevolvidoMensagens(this, (int)HttpStatusCode.OK, senhaCriptografada, "Senha criptografada com sucesso.");
    }
}
