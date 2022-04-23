using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio.Comum;

namespace PontoFacil.Api.Controlador.Middleware;
public class ExcecaoServidorMiddleware
{
    public static async Task Middleware(HttpContext context, Func<Task> next)
    {
        try { await next(); }
        catch (NegocioException negocioEx)
        {
            context.Response.StatusCode = negocioEx.StatusCodeErro;
            var corpo = new DevolvidoMensagemDTO { Mensagem = negocioEx.Message };
            var json = JsonConvert.SerializeObject(corpo);
            var bytes = Encoding.UTF8.GetBytes(json);
            await context.Response.Body.WriteAsync(bytes);
        }
        catch (Exception ex)
        {
            if (!(ex is NegocioException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var corpo = new DevolvidoMensagemDTO { Mensagem = Mensagens.FALHA_REQUISICAO };
                var json = JsonConvert.SerializeObject(corpo);
                var bytes = Encoding.UTF8.GetBytes(json);
                await context.Response.Body.WriteAsync(bytes);
            }
        }
    }
}