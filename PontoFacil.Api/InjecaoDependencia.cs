using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Repositorio.Convert;
using PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api;
public static class InjecaoDependencia
{
    public static WebApplicationBuilder AdicionarContextoDeConexao(this WebApplicationBuilder builder)
    {
        if (ConfiguracoesServico.EhBancoDadosRelacional(builder.Configuration))
        {
            string conexao_mariaDb = builder.Configuration[ConfiguracoesServico.ChaveConexaoBancoDadosRelacional];
            builder.Services.AdicionarContextoDeConexaoMySql(conexao_mariaDb);
        }
        else { builder.Services.AdicionarContextoDeConexaoInMemory(); }
        return builder;
    }
    public static IServiceCollection AdicionarContextoDeConexaoMySql(this IServiceCollection services, string conexao)
    {
        services.AddDbContext<PontoFacilContexto>(options => {
            options.UseMySql(conexao, ServerVersion.AutoDetect(conexao));
        }, ServiceLifetime.Singleton);
        return services;
    }
    public static IServiceCollection AdicionarContextoDeConexaoInMemory(this IServiceCollection services)
    {
        services.AddDbContext<PontoFacilContexto>(options => {
            options.UseInMemoryDatabase("pontofacil_inmemorydb");
        }, ServiceLifetime.Singleton);
        return services;
    }
    public static IServiceCollection AdicionarBibliotecaDeServicos(this IServiceCollection services)
    {
        services.AddSingleton<ConfiguracoesServico>();
        services.AddSingleton<CriptografiaServico>();
        return services;
    }
    public static IServiceCollection AdicionarBibliotecaDeConversoesUnicas(this IServiceCollection services)
    {
        services.AddSingleton<UsuarioConvertUnique>();
        services.AddSingleton<SessaoConvertUnique>();
        services.AddSingleton<AcessoConvertUnique>();
        return services;
    }
    public static IServiceCollection AdicionarBibliotecaDeConversoes(this IServiceCollection services)
    {
        services.AddSingleton<UsuarioConvert>();
        return services;
    }
    public static IServiceCollection AdicionarBibliotecaDeRepositorios(this IServiceCollection services)
    {
        services.AddSingleton<UsuarioRepositorio>();
        services.AddSingleton<SessaoRepositorio>();
        services.AddSingleton<DatabaseRepositorio>();
        return services;
    }
}