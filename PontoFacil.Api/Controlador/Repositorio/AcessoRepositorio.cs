using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Repositorio;
public class AcessoRepositorio
{
    private readonly PontoFacilContexto _contexto;
    private readonly ConfiguracoesServico _config;
    public AcessoRepositorio(PontoFacilContexto contexto, ConfiguracoesServico config)
    {
        _contexto = contexto;
        _config = config;
    }
    public IList<Acesso> TrazAcessosDoUsuario(int idUsuario)
    {
        var query = _contexto.Acessos.AsNoTracking().Where(x => x.usuario_id == idUsuario);
        var lista = Utilitarios.ParaLista(query);
        return lista;
    }
}