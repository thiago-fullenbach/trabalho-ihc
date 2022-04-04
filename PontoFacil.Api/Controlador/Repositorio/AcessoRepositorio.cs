using System;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Repositorio;
public class AcessoRepositorio
{
    private readonly PontoFacilContext _c;
    public AcessoRepositorio(PontoFacilContext c)
    {
        _c = c;
    }


}
