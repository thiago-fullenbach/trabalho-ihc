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
}