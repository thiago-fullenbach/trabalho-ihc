using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Repositorio;
public class RecursoRepositorio
{
    private readonly PontoFacilContext _c;
    public RecursoRepositorio(PontoFacilContext c)
    {
        _c = c;
    }

    public List<Recurso> ListarRecursosAcessadosPeloUsuario(int idUsuario)
    {
        var saida_recursos = new List<Recurso>();
        var doSql_acessos = _c.Acesso.Include(x => x.IdRecursoNavegacao)
                                     .AsNoTracking()
                                     .Where(x => x.IdUsuario == idUsuario);
        if (doSql_acessos != null && doSql_acessos.Any())
        {
            foreach (var doSql_iAcesso in doSql_acessos)
            {
                if (doSql_iAcesso?.IdRecursoNavegacao != null && !saida_recursos.Any(x => x.Id == doSql_iAcesso.IdRecurso))
                    saida_recursos.Add(doSql_iAcesso.IdRecursoNavegacao);
            }
        }
        return saida_recursos;
    }
}