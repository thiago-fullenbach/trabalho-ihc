using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;
using System;

namespace PontoFacil.Api.Controlador.Repositorio;
public class SessaoAbertaRepositorio
{
    private readonly PontoFacilContext _c;
    private readonly IConfiguration _config;
    private readonly TimeSpan _tempoAteExpirar;
    public SessaoAbertaRepositorio(PontoFacilContext c, IConfiguration config)
    {
        _c = c;
        _config = config;
        int segundos = int.Parse(config["Autorizacao:Sessao:SegundosAteExpirar"].ToString());
        _tempoAteExpirar = TimeSpan.FromSeconds(segundos);
    }

    public static string GerarHexVerificacao(int tamanhoHexVerificacao)
    {
        var saida_HexVerificacao = string.Empty;
        var rand = new Random();
        foreach (var _ in new object[tamanhoHexVerificacao])
        {
            int indiceAleatorio = rand.Next(0, 16);
            var charHexVerificacao = "0123456789ABCDEF"[indiceAleatorio];
            saida_HexVerificacao += $"{charHexVerificacao}";
        }
        return saida_HexVerificacao;
    }

    public async Task<SessaoAberta> AbrirSessaoDoUsuario(int idUsuario)
    {
        var inSql_sessao = new SessaoAberta
        {
            HexVerificacao = SessaoAbertaRepositorio.GerarHexVerificacao(40),
            IdUsuario = idUsuario,
            DataHoraUltimaAtualizacao = DateTime.Now
        };

        await _c.SessaoAberta.AddAsync(inSql_sessao);
        await _c.SaveChangesAsync();

        return inSql_sessao;
    }

    public async Task EncerrarSessao(int idSessao, string hexVerificacao)
    {
        var dlSql_sessao = _c.SessaoAberta.FirstOrDefault(x => x.Id == idSessao && x.HexVerificacao == hexVerificacao);
        if (dlSql_sessao == null) return;

        _c.SessaoAberta.Remove(dlSql_sessao);
        await _c.SaveChangesAsync();
    }

    public async Task<SessaoAberta> AtualizarSessao(int idSessao, string hexVerificacao)
    {
        var upSql_sessao = _c.SessaoAberta.FirstOrDefault(x => x.Id == idSessao && x.HexVerificacao == hexVerificacao);
        if (upSql_sessao == null) throw new RepositorioException(Mensagens.ERRO_SESSAO_ENCERRADA);

        if (DateTime.Now - upSql_sessao.DataHoraUltimaAtualizacao > _tempoAteExpirar)
        {
            await EncerrarSessao(idSessao, hexVerificacao);
            throw new RepositorioException(Mensagens.ERRO_SESSAO_ENCERRADA);
        }

        upSql_sessao.DataHoraUltimaAtualizacao = DateTime.Now;

        _c.SessaoAberta.Update(upSql_sessao);
        await _c.SaveChangesAsync();

        return upSql_sessao;
    }
}
