using System.Net;
using System.Text;
using System.Text.Json;
using DDD.Api.Domain.Models.BusnModel.Exception;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.BackEndApp.v1.Middleware;
public class ExceptionMiddleware
{
    public static async Task ProcessarAsync(HttpContext context, Func<Task> next)
    {
        try
        {
            await next();
        }
        catch (BusinessException bEx)
        {
            context.Response.StatusCode = bEx.StatusCode;
            var corpo = new DevolvidoMensagensDtoModel { Mensagens = bEx.MensagensErro };
            await EscreveRespostaJsonAsync(context, corpo);
        }
        catch (Exception ex)
        {
            if (!(ex is BusinessException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var corpo = new DevolvidoMensagensDtoModel { Mensagens = new List<string> { "Ocorreu um erro interno no servidor..." } };
                await EscreveRespostaJsonAsync(context, corpo);
            }
        }

    }
    private static async Task EscreveRespostaJsonAsync(HttpContext context, DevolvidoMensagensDtoModel corpo)
    {
        var json = JsonSerializer.Serialize(corpo);
        var bytes = Encoding.UTF8.GetBytes(json);
        await context.Response.Body.WriteAsync(bytes);
    }
}