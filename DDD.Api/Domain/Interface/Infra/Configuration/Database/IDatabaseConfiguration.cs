namespace DDD.Api.Domain.Interface.Infra.Configuration.Database;
public interface IDatabaseConfiguration
{
    string GetStringConexao();
    string GetNomeBancoDados();
    string GetNomeColecaoUsuarios();
    string GetNomeColecaoSessoes();
    string GetNomeColecaoAcessos();
    string GetNomeColecaoLocais();
    string GetNomeColecaoPresencas();
    bool UsaBancoDadosRelacional();
}