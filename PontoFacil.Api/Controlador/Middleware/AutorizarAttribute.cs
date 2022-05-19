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
        var sessaoRepositorio = context.HttpContext.RequestServices.GetService<SessaoRepositorio>();
        var sessaoConvertUnique = context.HttpContext.RequestServices.GetService<SessaoConvertUnique>();
        var contexto = context.HttpContext.RequestServices.GetService<PontoFacilContexto>();
        var usuarioConvert = context.HttpContext.RequestServices.GetService<UsuarioConvert>();
        if (sessaoRepositorio == null)
            { erroInterno.AppendLine(string.Format(Mensagens.FALHA_RECUPERAR_XXXX_AO_CHAMAR_GETSERVICE_XXXX, "SessaoRepositorio")); }
        if (sessaoConvertUnique == null)
            { erroInterno.AppendLine(string.Format(Mensagens.FALHA_RECUPERAR_XXXX_AO_CHAMAR_GETSERVICE_XXXX, "SessaoConvertUnique")); }
        if (contexto == null)
            { erroInterno.AppendLine(string.Format(Mensagens.FALHA_RECUPERAR_XXXX_AO_CHAMAR_GETSERVICE_XXXX, "PontoFacilContexto")); }
        if (usuarioConvert == null)
            { erroInterno.AppendLine(string.Format(Mensagens.FALHA_RECUPERAR_XXXX_AO_CHAMAR_GETSERVICE_XXXX, "UsuarioConvert")); }
        if (erroInterno.ToString() != string.Empty)
            { throw new Exception(erroInterno.ToString()); }

        // antes de ser consumido pelo endpoint
        var sessaoEnviada = sessaoConvertUnique.ExtrairSessaoEnvioHeader(context.HttpContext.Request.Headers);
        var sessaoUpd = await sessaoRepositorio.AtualizarSessao(sessaoEnviada);
        var sessaoEnviarNovamente = sessaoConvertUnique.ParaSessaoEnvioHeaderDTO(sessaoUpd);
        var usuario = contexto.Usuarios
            .AsNoTracking()
            .First(x => x.id == sessaoUpd.usuario_id);
        var usuarioLogado = usuarioConvert.ParaUsuarioLogadoDTO(usuario);
        context.HttpContext.Request.Headers["sessao"] = JsonConvert.SerializeObject(sessaoEnviarNovamente);
        context.HttpContext.Request.Headers["usuario"] = JsonConvert.SerializeObject(usuarioLogado);

        await next();

        // depois de ser consumido pelo endpoint
        sessaoUpd = await sessaoRepositorio.AtualizarSessao(sessaoEnviarNovamente);
        sessaoEnviarNovamente = sessaoConvertUnique.ParaSessaoEnvioHeaderDTO(sessaoUpd);
        usuario = contexto.Usuarios
            .AsNoTracking()
            .First(x => x.id == sessaoUpd.usuario_id);
        usuarioLogado = usuarioConvert.ParaUsuarioLogadoDTO(usuario);
        context.HttpContext.Response.Headers["sessao"] = JsonConvert.SerializeObject(sessaoEnviarNovamente);
        context.HttpContext.Response.Headers["usuario"] = JsonConvert.SerializeObject(usuarioLogado);
    }
}
