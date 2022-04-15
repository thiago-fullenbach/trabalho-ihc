using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Modelo;

namespace PontoFacil.Api.Controlador.FilterComoMiddleware;
public class AutorizarAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var sessaoAbertaR = context.HttpContext.RequestServices.GetService<SessaoAbertaRepositorio>();
        var recursoR = context.HttpContext.RequestServices.GetService<RecursoRepositorio>();
        if (sessaoAbertaR == null || recursoR == null)
        {
            context.Result = new ObjectResult((int)HttpStatusCode.InternalServerError);
            return;
        }

        // antes de ser consumido pelo endpoint
        string jsonDadosSessao = context.HttpContext.Request.Headers["identidadeSessao"];
        IdentidadeSessaoDTO dadosSessao = JsonConvert.DeserializeObject<IdentidadeSessaoDTO>(jsonDadosSessao) ?? new IdentidadeSessaoDTO();

        SessaoAberta? doSql_sessao = null;
        bool retornar = false;
        try
        {
            doSql_sessao = await sessaoAbertaR.AtualizarSessao(dadosSessao.Id, dadosSessao.HexVerificacao ?? string.Empty);
        }
        catch (RepositorioException ex)
        {
            context.Result = new ObjectResult((int)HttpStatusCode.Unauthorized) { Value = new DevolvidoMensagemDTO { Mensagem = ex.Message } };
            retornar = true;
        }
        if (retornar) return;

        await next();

        // depois de ser consumido pelo endpoint
        var doSql_sessaoAposRequisicao = await sessaoAbertaR.AtualizarSessao(doSql_sessao?.Id ?? 0, doSql_sessao?.HexVerificacao ?? string.Empty);
        
        var saida_sessaoAtualizada = new SessaoAtualizadaDTO();
        saida_sessaoAtualizada.Id = doSql_sessaoAposRequisicao.Id;
        saida_sessaoAtualizada.HexVerificacao = doSql_sessaoAposRequisicao.HexVerificacao;
        saida_sessaoAtualizada.DataHoraUltimaAtualizacao = doSql_sessaoAposRequisicao.DataHoraUltimaAtualizacao;
        var doSql_recursosPermitidos = recursoR.ListarRecursosAcessadosPeloUsuario(doSql_sessaoAposRequisicao.IdUsuario);
        saida_sessaoAtualizada.ListaRecursosPermitidos = doSql_recursosPermitidos.Select(x => new RecursoPermitidoDTO
        {
            Id = x.Id,
            Nome = x.Nome
        }).ToList();
        string saida_jsonDadosSessao = JsonConvert.SerializeObject(saida_sessaoAtualizada);
        context.HttpContext.Response.Headers["sessao"] = saida_jsonDadosSessao;
    }
}
