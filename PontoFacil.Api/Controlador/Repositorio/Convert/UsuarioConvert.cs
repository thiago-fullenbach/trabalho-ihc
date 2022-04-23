using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Repositorio.Convert;
public class UsuarioConvert
{
    private readonly PontoFacilContexto _contexto;
    private readonly ConfiguracoesServico _configuracoesServico;
    private readonly UsuarioConvertUnique _usuarioConvertUnique;
    private readonly RecursoConvertUnique _recursoConvertUnique;
    public UsuarioConvert(PontoFacilContexto contexto,
                          ConfiguracoesServico configuracoesServico,
                          UsuarioConvertUnique usuarioConvertUnique,
                          RecursoConvertUnique recursoConvertUnique)
    {
        _contexto = contexto;
        _configuracoesServico = configuracoesServico;
        _usuarioConvertUnique = usuarioConvertUnique;
        _recursoConvertUnique = recursoConvertUnique;
    }
    public UsuarioLogadoDTO ParaUsuarioLogadoDTO(Usuarios usuario)
    {
        var usuarioLogado = _usuarioConvertUnique.ParaUsuarioLogadoDTO(usuario);
        var recurso = _contexto.Recursos
            .AsNoTracking()
            .First(x => x.usuarios_id == usuario.id);
        usuarioLogado.NavegacaoRecurso = _recursoConvertUnique.ParaUsuarioRecursoDTO(recurso);
        return usuarioLogado;
    }
}