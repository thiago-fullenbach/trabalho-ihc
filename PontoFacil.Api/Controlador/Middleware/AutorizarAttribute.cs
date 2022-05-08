using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Controlador.Repositorio.Convert;
using PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Middleware;
public class AutorizarAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var erroInterno = new StringBuilder();
        var sessoesRepositorio = context.HttpContext.RequestServices.GetService<SessoesRepositorio>();
        var sessaoConvertUnique = context.HttpContext.RequestServices.GetService<SessaoConvertUnique>();
        var contexto = context.HttpContext.RequestServices.GetService<PontoFacilContexto>();
        var usuarioConvert = context.HttpContext.RequestServices.GetService<UsuarioConvert>();
        if (sessoesRepositorio == null)
            { erroInterno.AppendLine(string.Format(Mensagens.FALHA_RECUPERAR_XXXX_AO_CHAMAR_GETSERVICE_XXXX, "SessoesRepositorio")); }
        if (sessaoConvertUnique == null)
            { erroInterno.AppendLine(string.Format(Mensagens.FALHA_RECUPERAR_XXXX_AO_CHAMAR_GETSERVICE_XXXX, "SessaoConvertUnique")); }
        if (contexto == null)
            { erroInterno.AppendLine(string.Format(Mensagens.FALHA_RECUPERAR_XXXX_AO_CHAMAR_GETSERVICE_XXXX, "PontoFacilContexto")); }
        if (usuarioConvert == null)
            { erroInterno.AppendLine(string.Format(Mensagens.FALHA_RECUPERAR_XXXX_AO_CHAMAR_GETSERVICE_XXXX, "UsuarioConvert")); }
        if (erroInterno.ToString() != string.Empty)
            { throw new Exception(erroInterno.ToString()); }

        // antes de ser consumido pelo endpoint
        var sessaoEnviada = sessaoConvertUnique.ExtrairSessaoEnviarPeloHeader(context.HttpContext.Request.Headers);
        var sessaoUpd = await sessoesRepositorio.AtualizarSessao(sessaoEnviada);
        var sessaoEnviarNovamente = sessaoConvertUnique.ParaSessaoEnviarPeloHeaderDTO(sessaoUpd);
        var usuario = contexto.Usuarios
            .AsNoTracking()
            .First(x => x.id == sessaoUpd.usuarios_id);
        var usuarioLogado = usuarioConvert.ParaUsuarioLogadoDTO(usuario);
        context.HttpContext.Request.Headers["sessao"] = JsonConvert.SerializeObject(sessaoEnviarNovamente);
        context.HttpContext.Request.Headers["usuario"] = JsonConvert.SerializeObject(usuarioLogado);

        await next();

        // depois de ser consumido pelo endpoint
        sessaoUpd = await sessoesRepositorio.AtualizarSessao(sessaoEnviarNovamente);
        sessaoEnviarNovamente = sessaoConvertUnique.ParaSessaoEnviarPeloHeaderDTO(sessaoUpd);
        usuario = contexto.Usuarios
            .AsNoTracking()
            .First(x => x.id == sessaoUpd.usuarios_id);
        usuarioLogado = usuarioConvert.ParaUsuarioLogadoDTO(usuario);
        context.HttpContext.Response.Headers["sessao"] = JsonConvert.SerializeObject(sessaoEnviarNovamente);
        context.HttpContext.Response.Headers["usuario"] = JsonConvert.SerializeObject(usuarioLogado);
    }
}
