using DDD.Api.Domain.Interface.Infra.Configuration.Database;

namespace DDD.Api.Infra.Configuration.Database;
public class MongoDatabaseConfiguration : IDatabaseConfiguration
{
    private readonly IConfiguration _configuration;
    public MongoDatabaseConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetStringConexao()
    {
        return _configuration["CONEXAO_BANCO_DADOS_NOSQL"];
    }

    public string GetNomeBancoDados()
    {
        return _configuration["NOME_BANCO_DADOS_NOSQL"];
    }

    public string GetNomeColecaoUsuarios()
    {
        return _configuration["BancoDadosMongoDB:ColecaoUsuarios"];
    }

    public string GetNomeColecaoSessoes()
    {
        return _configuration["BancoDadosMongoDB:ColecaoSessoes"];
    }

    public string GetNomeColecaoAcessos()
    {
        return _configuration["BancoDadosMongoDB:ColecaoAcessos"];
    }

    public string GetNomeColecaoLocais()
    {
        return _configuration["BancoDadosMongoDB:ColecaoLocais"];
    }

    public string GetNomeColecaoPresencas()
    {
        return _configuration["BancoDadosMongoDB:ColecaoPresencas"];
    }

    public bool UsaBancoDadosRelacional()
    {
        return _configuration["BancoDadosRelacional"] == "S";
    }
}