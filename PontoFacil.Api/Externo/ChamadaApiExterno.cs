using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;

namespace PontoFacil.Api.Externo;
public class ChamadaApiExterno : ChamadaApiState
{
    public ChamadaApiExterno(string urlHttpServidor)
    {
        UrlHttpServidor = urlHttpServidor;
    }
    public override async Task<string> HasheiaSenha(string senha)
    {
        var uriBuilder = new UriBuilder(PegaUrlHasheiaSenha(senha));
        var clienteHttp = new HttpClient { BaseAddress = uriBuilder.Uri };
        var resposta = await clienteHttp.GetAsync(uriBuilder.Uri);
        resposta.EnsureSuccessStatusCode();
        var conteudo = await resposta.Content.ReadFromJsonAsync<DevolvidoMensagensDTO>();
        return conteudo.Devolvido.ToString();
    }
}