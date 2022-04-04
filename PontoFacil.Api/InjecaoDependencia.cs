using PontoFacil.Api.Controlador.Repositorio;

namespace PontoFacil.Api;
public static class InjecaoDependencia
{
    public static IServiceCollection AdicionarBibliotecaDeRepositorios(this IServiceCollection svcs)
    {
        svcs.AddScoped<AcessoRepositorio>();
        svcs.AddScoped<ContaRepositorio>();
        svcs.AddScoped<RecursoRepositorio>();
        svcs.AddScoped<SessaoAbertaRepositorio>();
        svcs.AddScoped<UsuarioRepositorio>();
        return svcs;
    }
}