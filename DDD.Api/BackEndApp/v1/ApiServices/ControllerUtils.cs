using DDD.Api.Domain.Models.DtoModel;
using Microsoft.AspNetCore.Mvc;

namespace DDD.Api.BackEndApp.v1.ApiServices;
public class ControllerUtils
{
    public static ObjectResult StatusCodeDevolvidoMensagens(ControllerBase controller, int statusCode, object? devolvido, string mensagemUnica)
    {
        var corpo = new DevolvidoMensagensDtoModel
        {
            Devolvido = devolvido,
            Mensagens = new List<string> { mensagemUnica }
        };
        return controller.StatusCode(statusCode, corpo);
    }

    public static ObjectResult StatusCodeDevolvidoMensagens(ControllerBase controller, int statusCode, object? devolvido, IEnumerable<string> mensagens)
    {
        var corpo = new DevolvidoMensagensDtoModel
        {
            Devolvido = devolvido,
            Mensagens = new List<string>(mensagens)
        };
        return controller.StatusCode(statusCode, corpo);
    }
}