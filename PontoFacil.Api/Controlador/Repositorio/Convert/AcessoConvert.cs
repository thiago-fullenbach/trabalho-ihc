using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;
using PontoFacil.Api.Modelo.Dominios;

namespace PontoFacil.Api.Controlador.Repositorio.Convert;
public class AcessoConvert
{
    public bool UsuarioTemRecursoHabilitado(UsuarioLogadoDTO usuario, EnRecurso recurso)
    {
        var acessoAoRecurso = usuario.Acessos.First(x => x.Recurso_cod_en == (int)recurso);
        return (acessoAoRecurso.Eh_habilitado ?? false);
    }
}