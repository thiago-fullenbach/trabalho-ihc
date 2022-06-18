using DDD.Api.Domain.Interface.Business.Services;

namespace DDD.Api.BackEndApp.v1.Middleware;
public class CargaMinimaMiddleware
{
    public static async Task ProcessarAsync(HttpContext context, Func<Task> next)
    {
        var services = context.RequestServices;

        var usuarioService = services.GetRequiredService<IUsuarioService>();
        int idAdminRoot = await usuarioService.GetIdUsuarioAdminRootAsync();
        if (idAdminRoot == 0)
        {
            await usuarioService.CarregarUsuariosAdminRootEImportarExportarAsync();
        }
        await next();
    }
}