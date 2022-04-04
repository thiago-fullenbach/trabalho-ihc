using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Modelo;

namespace PontoFacil.Api.Controlador.FilterComoMiddleware;
public class AutorizarAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var sessaoAberta = context.HttpContext.RequestServices.GetService<SessaoAbertaRepositorio>();
        if (sessaoAberta == null)
        {
            context.Result = new ObjectResult((int)HttpStatusCode.InternalServerError);
            return;
        }

        // antes de ser consumido pelo endpoint
        string headerSessaoIdGet = context.HttpContext.Request.Headers["identidadeSessao_Id"];
        int headerSessaoId = int.Parse(headerSessaoIdGet);
        string headerSessaoHexVerificacao = context.HttpContext.Request.Headers["identidadeSessao_HexVerificacao"];
        
        SessaoAberta? doSql_sessao = null;
        bool retornar = false;
        try
        {
            doSql_sessao = await sessaoAberta.AtualizarSessao(headerSessaoId, headerSessaoHexVerificacao);
        }
        catch (RepositorioException ex)
        {
            context.Result = new ObjectResult((int)HttpStatusCode.Unauthorized) { Value = new DevolvidoMensagemDTO { Mensagem = ex.Message } };
            retornar = true;
        }
        if (retornar) return;

        await next();

        // depois de ser consumido pelo endpoint
        var doSql_sessaoAposRequisicao = await sessaoAberta.AtualizarSessao(doSql_sessao?.Id ?? 0, doSql_sessao?.HexVerificacao ?? string.Empty);
        int doSql_sessaoId = doSql_sessaoAposRequisicao.Id;
        string doSql_sessaoHexVerificacao = doSql_sessaoAposRequisicao?.HexVerificacao ?? string.Empty;
        context.HttpContext.Response.Headers["identidadeSessao_Id"] = doSql_sessaoId.ToString();
        context.HttpContext.Response.Headers["identidadeSessao_HexVerificacao"] = doSql_sessaoHexVerificacao;
    }
}
