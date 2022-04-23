using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PontoFacil.Api.Batch;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Controlador.Repositorio.Convert;
using PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Middleware;
public class BatchExclusaoSessoesAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var headers = context.HttpContext.Request.Headers;
        var mensagens = new List<string>();
        if (!(ParametrosBatchExclusaoSessoes.EndpointAberto ?? false)
            || (ParametrosBatchExclusaoSessoes.GuidToGuidCustomHeaders == null)
            || !CorsUtilitarios.PossuiDicionarioGuidToGuid(headers, ParametrosBatchExclusaoSessoes.GuidToGuidCustomHeaders))
            { mensagens.Add(Mensagens.ACESSO_NEGADO); }
        ParametrosBatchExclusaoSessoes.EndpointAberto = false;
        ParametrosBatchExclusaoSessoes.GuidToGuidCustomHeaders = null;
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.Unauthorized);

        await next();
    }
}