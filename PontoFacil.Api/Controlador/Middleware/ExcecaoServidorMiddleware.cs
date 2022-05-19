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
            var corpo = new DevolvidoMensagensDTO { Mensagens = NegocioException.DesmontaMensagemErro(negocioEx.Message) };
            await EscreveRespostaJsonAsync(context, corpo);
        }
        catch (Exception ex)
        {
            if (!(ex is NegocioException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var corpo = new DevolvidoMensagensDTO();
                corpo.SetMensagemUnica(Mensagens.REQUISICAO_SUCESSO);
                await EscreveRespostaJsonAsync(context, corpo);
            }
        }
    }
    public static async Task EscreveRespostaJsonAsync(HttpContext context, DevolvidoMensagensDTO corpo)
    {
        var json = JsonConvert.SerializeObject(corpo);
        var bytes = Encoding.UTF8.GetBytes(json);
        await context.Response.Body.WriteAsync(bytes);
    }
}