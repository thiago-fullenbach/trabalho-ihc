using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Repositorio.Convert;
using PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api;
public static class InjecaoDependencia
{
    public static IServiceCollection AdicionarContextoDeConexaoMySql(this IServiceCollection services, string conexao)
    {
        services.AddDbContext<PontoFacilContexto>(options => {
            options.UseMySql(conexao, ServerVersion.AutoDetect(conexao));
        });
        return services;
    }
    public static IServiceCollection AdicionarContextoDeConexaoInMemory(this IServiceCollection services)
    {
        services.AddDbContext<PontoFacilContexto>(options => {
            options.UseInMemoryDatabase("pontofacil_inmemorydb");
        });
        return services;
    }
    public static IServiceCollection AdicionarBibliotecaDeServicos(this IServiceCollection services)
    {
        services.AddScoped<ConfiguracoesServico>();
        services.AddScoped<CriptografiaServico>();
        return services;
    }
    public static IServiceCollection AdicionarBibliotecaDeConversoesUnicas(this IServiceCollection services)
    {
        services.AddScoped<UsuarioConvertUnique>();
        services.AddScoped<SessaoConvertUnique>();
        services.AddScoped<RecursoConvertUnique>();
        return services;
    }
    public static IServiceCollection AdicionarBibliotecaDeConversoes(this IServiceCollection services)
    {
        services.AddScoped<UsuarioConvert>();
        return services;
    }
    public static IServiceCollection AdicionarBibliotecaDeRepositorios(this IServiceCollection services)
    {
        services.AddScoped<UsuariosRepositorio>();
        services.AddScoped<SessoesRepositorio>();
        services.AddScoped<DatabaseRepositorio>();
        return services;
    }
}