using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo;

namespace PontoFacil.Api.Batch;
public class ProcessadorExclusaoSessoes
{
    public async Task Processar()
    {
        var ub = new UriBuilder(ParametrosBatchExclusaoSessoes.UrlBatchExclusaoSessoes);
        var client = new HttpClient();
        ParametrosBatchExclusaoSessoes.EndpointAberto = true;
        var dicionario = ParametrosBatchExclusaoSessoes.GuidToGuidCustomHeaders = CorsUtilitarios.GeraDicionarioGuidToGuid();
        foreach (var chaveValor in dicionario)
            { client.DefaultRequestHeaders.Add(chaveValor.Key, chaveValor.Value); }
        await client.PostAsync(ub.Uri, null);
    }
}