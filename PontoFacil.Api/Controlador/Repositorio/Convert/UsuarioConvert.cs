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
    private readonly AcessoConvertUnique _acessoConvertUnique;
    public UsuarioConvert(PontoFacilContexto contexto,
                          ConfiguracoesServico configuracoesServico,
                          UsuarioConvertUnique usuarioConvertUnique,
                          AcessoConvertUnique acessoConvertUnique)
    {
        _contexto = contexto;
        _configuracoesServico = configuracoesServico;
        _usuarioConvertUnique = usuarioConvertUnique;
        _acessoConvertUnique = acessoConvertUnique;
    }
    public UsuarioLogadoDTO ParaUsuarioLogadoDTO(Usuario usuario)
    {
        var usuarioLogado = _usuarioConvertUnique.ParaUsuarioLogadoDTO(usuario);
        var acessosUsuario = _contexto.Acessos
            .AsNoTracking()
            .Where(x => x.usuario_id == usuario.id);
        usuarioLogado.Acessos = new List<AcessoUsuarioLogadoDTO>();
        foreach (var iAcesso in acessosUsuario)
            { usuarioLogado.Acessos.Add(_acessoConvertUnique.ParaAcessoUsuarioLogadoDTO(iAcesso)); }
        return usuarioLogado;
    }
}