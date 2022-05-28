using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Controlador.Middleware;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api;
public static class InjecoesCustomizadas
{
    public static WebApplication? UsarMiddlewaresCustomizados(this WebApplication? app)
    {
        app.Use(ExcecaoServidorMiddleware.Middleware);
        return app;
    }
    public static WebApplicationBuilder ExporUrlsForaContainer(this WebApplicationBuilder builder)
    {
        string aspnetcoreUrls = builder.Configuration["ASPNETCORE_URLS"];
        builder.WebHost.UseKestrel()
            .UseUrls(aspnetcoreUrls)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration();
        return builder;
    }
}