using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Batch;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Repositorio;
public class SessaoRepositorio
{
    private readonly PontoFacilContexto _contexto;
    private readonly ConfiguracoesServico _configuracoesServico;
    private readonly SessaoConvertUnique _sessaoConvertUnique;
    public SessaoRepositorio(PontoFacilContexto contexto,
                              ConfiguracoesServico configuracoesServico,
                              SessaoConvertUnique sessaoConvertUnique)
    {
        _contexto = contexto;
        _configuracoesServico = configuracoesServico;
        _sessaoConvertUnique = sessaoConvertUnique;
    }
    public async Task<Sessao> AbrirSessao(int usuarioId)
    {
        var inclSessao = new Sessao
        {
            hex_verificacao = GeraHex_verificacao(),
            usuario_id = usuarioId,
            datahora_ultima_autenticacao = DateTime.Now
        };
        await _contexto.Sessoes.AddAsync(inclSessao);
        await _contexto.SaveChangesAsync();
        await AgendarBatchExclusaoSessoes();
        return inclSessao;
    }
    public string GeraHex_verificacao()
    {
        return CriptografiaServico.HexAleatorioSeguro128Caracteres();
    }
    public async Task AgendarBatchExclusaoSessoes()
    {
        var sessoesExclusao = _contexto.Sessoes.AsNoTracking();
        var gerenciadorAgendamento = await GerenciadorAgendamento.Instancia(_configuracoesServico);
        await gerenciadorAgendamento.DefineProximoBatchExclusaoSessoes(sessoesExclusao, _configuracoesServico);
    }
    public async Task<Sessao> AtualizarSessao(SessaoEnvioHeaderDTO sessao)
    {
        var updtSessao = _contexto.Sessoes
            .FirstOrDefault(x => x.id == sessao.Id
            && x.hex_verificacao == sessao.Hex_verificacao);
        var erros = new List<string>();
        if (updtSessao == null || Expirou(updtSessao))
            { erros.Add(string.Format(Mensagens.XXXX_INVALIDY, "Sessão", "a")); }
        NegocioException.ThrowErroSeHouver(erros, (int)HttpStatusCode.Unauthorized);
        
        updtSessao.datahora_ultima_autenticacao = DateTime.Now;
        _contexto.Sessoes.Update(updtSessao);
        await _contexto.SaveChangesAsync();
        await AgendarBatchExclusaoSessoes();
        return updtSessao;
    }
    public bool Expirou(Sessao sessao)
    {
        return (DateTime.Now > sessao.datahora_ultima_autenticacao + _configuracoesServico.TempoExpirarSessao);
    }
    public async Task ExcluirSessoesExpiradas()
    {
        var sessoes = _contexto.Sessoes.AsNoTracking();
        var sessoesExpiradas = new List<Sessao>();
        foreach (var x in sessoes)
            { if (Expirou(x)) { sessoesExpiradas.Add(x); } }
        _contexto.Sessoes.RemoveRange(Utilitarios.ParaLista(sessoesExpiradas));
        await _contexto.SaveChangesAsync();
        await AgendarBatchExclusaoSessoes();
    }
}
