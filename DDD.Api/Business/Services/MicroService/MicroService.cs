using DDD.Api.Domain.Interface.Business.Services.MicroService;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.Business.Services.MicroService;
public class MicroService : IMicroService
{
    public async Task<T?> GetNaoAutenticadoAsync<T>(string url)
    {
        var uriBuilder = new UriBuilder(url);
        var clienteHttp = new HttpClient { BaseAddress = uriBuilder.Uri };
        var resposta = await clienteHttp.GetAsync(uriBuilder.Uri);
        resposta.EnsureSuccessStatusCode();
        var conteudo = await resposta.Content.ReadFromJsonAsync<DevolvidoMensagensDtoModel>();
        if (conteudo == null)
        {
            return default(T);
        }
        return (T?)(conteudo.Devolvido);
    }
}