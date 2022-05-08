using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
public class RecursoConvertUnique
{
    private readonly PontoFacilContexto _contexto;
    private readonly ConfiguracoesServico _configuracoesServico;
    public RecursoConvertUnique(PontoFacilContexto contexto, ConfiguracoesServico configuracoesServico)
    {
        _contexto = contexto;
        _configuracoesServico = configuracoesServico;
    }
    public UsuarioRecursoDTO ParaUsuarioRecursoDTO(Recursos recurso)
    {
        var resultado = new UsuarioRecursoDTO
        {
            Pode_vis_usuario = recurso.pode_vis_usuario,
            Pode_cad_usuario = recurso.pode_cad_usuario,
            Pode_vis_demais_usuarios = recurso.pode_vis_demais_usuarios,
            Pode_cad_demais_usuarios = recurso.pode_cad_demais_usuarios,
            Pode_cad_administrador_acessos = recurso.pode_cad_administrador_acessos,
            Pode_vis_ponto = recurso.pode_vis_ponto,
            Pode_cad_ponto = recurso.pode_cad_ponto,
            Pode_vis_ponto_demais_usuarios = recurso.pode_vis_ponto_demais_usuarios,
            Pode_vis_ajuste = recurso.pode_vis_ajuste,
            Pode_vis_ajuste_demais_usuarios = recurso.pode_vis_ajuste_demais_usuarios,
            Pode_cad_ajuste_demais_usuarios = recurso.pode_cad_ajuste_demais_usuarios
        };
        return Utilitarios.DevolverComNovoEspacoNaMemoria(resultado);
    }
    public static UsuarioRecursoDTO RecursoPadrao
    {
        get
        {
            return new UsuarioRecursoDTO
            {
                Pode_vis_usuario = true,
                Pode_cad_usuario = true,
                Pode_vis_demais_usuarios = false,
                Pode_cad_demais_usuarios = false,
                Pode_cad_administrador_acessos = false,
                Pode_vis_ponto = true,
                Pode_cad_ponto = true,
                Pode_vis_ponto_demais_usuarios = false,
                Pode_vis_ajuste = true,
                Pode_vis_ajuste_demais_usuarios = false,
                Pode_cad_ajuste_demais_usuarios = false
            };
        }
    }
    public Recursos ParaRecursos(UsuarioRecursoDTO recurso)
    {
        var resultado = new Recursos
        {
            pode_vis_usuario = recurso.Pode_vis_usuario,
            pode_cad_usuario = recurso.Pode_cad_usuario,
            pode_vis_demais_usuarios = recurso.Pode_vis_demais_usuarios,
            pode_cad_demais_usuarios = recurso.Pode_cad_demais_usuarios,
            pode_cad_administrador_acessos = recurso.Pode_cad_administrador_acessos,
            pode_vis_ponto = recurso.Pode_vis_ponto,
            pode_cad_ponto = recurso.Pode_cad_ponto,
            pode_vis_ponto_demais_usuarios = recurso.Pode_vis_ponto_demais_usuarios,
            pode_vis_ajuste = recurso.Pode_vis_ajuste,
            pode_vis_ajuste_demais_usuarios = recurso.Pode_vis_ajuste_demais_usuarios,
            pode_cad_ajuste_demais_usuarios = recurso.Pode_cad_ajuste_demais_usuarios
        };
        return Utilitarios.DevolverComNovoEspacoNaMemoria(resultado);
    }
}