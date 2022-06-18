namespace DDD.Api.Domain.Models.DtoModel;
public class DevolvidoMensagensDtoModel
{
    public DevolvidoMensagensDtoModel()
    {
        Mensagens = new List<string>();
    }
    public object? Devolvido { get; set; }
    public List<string> Mensagens { get; set; }
}