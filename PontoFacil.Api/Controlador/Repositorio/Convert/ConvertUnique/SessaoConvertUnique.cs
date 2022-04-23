using System.Net;
using Newtonsoft.Json;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
public class SessaoConvertUnique
{
    private readonly PontoFacilContexto _contexto;
    private readonly ConfiguracoesServico _configuracoesServico;
    public SessaoConvertUnique(PontoFacilContexto contexto, ConfiguracoesServico configuracoesServico)
    {
        _contexto = contexto;
        _configuracoesServico = configuracoesServico;
    }
    public SessaoEnviarPeloHeaderDTO ParaSessaoEnviarPeloHeaderDTO(Sessoes sessao)
    {
        var resultado = new SessaoEnviarPeloHeaderDTO
        {
            Id = sessao.id,
            HexVerificacao = sessao.hexVerificacao,
            Datahora_ultima_autenticacao = sessao.datahora_ultima_autenticacao
        };
        return Utilitarios.DevolverComNovoEspacoNaMemoria(resultado);
    }
    public SessaoEnviarPeloHeaderDTO ExtrairSessaoEnviarPeloHeader(IHeaderDictionary headers)
    {
        var mensagens = new List<string>();
        if (!headers.ContainsKey("sessao"))
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY, "Sessão", "a")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);

        var sessaoJson = headers["sessao"];
        if (string.IsNullOrWhiteSpace(sessaoJson))
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY, "Sessão", "a")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);

        var sessao = JsonConvert.DeserializeObject<SessaoEnviarPeloHeaderDTO>(sessaoJson);
        if (sessao == null || sessao.Id == null || sessao.HexVerificacao == null)
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY, "Sessão", "a")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);

        return Utilitarios.DevolverComNovoEspacoNaMemoria(sessao);
    }
}