using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;
using PontoFacil.Api.Modelo.Dominios;

namespace PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
public class AcessoConvertUnique
{
    private readonly PontoFacilContexto _contexto;
    private readonly ConfiguracoesServico _configuracoesServico;
    public AcessoConvertUnique(PontoFacilContexto contexto, ConfiguracoesServico configuracoesServico)
    {
        _contexto = contexto;
        _configuracoesServico = configuracoesServico;
    }
    public AcessoUsuarioLogadoDTO ParaAcessoUsuarioLogadoDTO(Acesso acesso)
    {
        var resultado = new AcessoUsuarioLogadoDTO
        {
            Recurso_cod_en = acesso.recurso_cod_en,
            Eh_habilitado = acesso.eh_habilitado
        };
        return Utilitarios.DevolverComNovoEspacoNaMemoria(resultado);
    }
    public static IList<AcessoUsuarioLogadoDTO> AcessosPadrao
    {
        get
        {
            return new List<AcessoUsuarioLogadoDTO>
            {
                new AcessoUsuarioLogadoDTO
                {
                    Recurso_cod_en = (int)EnRecurso.VisualizarUsuario,
                    Eh_habilitado = true
                },
                new AcessoUsuarioLogadoDTO
                {
                    Recurso_cod_en = (int)EnRecurso.CadastrarUsuario,
                    Eh_habilitado = true
                },
                new AcessoUsuarioLogadoDTO
                {
                    Recurso_cod_en = (int)EnRecurso.VisualizarDemaisUsuarios,
                    Eh_habilitado = false
                },
                new AcessoUsuarioLogadoDTO
                {
                    Recurso_cod_en = (int)EnRecurso.CadastrarDemaisUsuarios,
                    Eh_habilitado = false
                },
                new AcessoUsuarioLogadoDTO
                {
                    Recurso_cod_en = (int)EnRecurso.CadastrarAcessoTodosUsuarios,
                    Eh_habilitado = false
                },
                new AcessoUsuarioLogadoDTO
                {
                    Recurso_cod_en = (int)EnRecurso.VisualizarPonto,
                    Eh_habilitado = true
                },
                new AcessoUsuarioLogadoDTO
                {
                    Recurso_cod_en = (int)EnRecurso.RegistrarPonto,
                    Eh_habilitado = true
                },
                new AcessoUsuarioLogadoDTO
                {
                    Recurso_cod_en = (int)EnRecurso.VisualizarPontoDemaisUsuarios,
                    Eh_habilitado = false
                },
                new AcessoUsuarioLogadoDTO
                {
                    Recurso_cod_en = (int)EnRecurso.VisualizarAjuste,
                    Eh_habilitado = true
                },
                new AcessoUsuarioLogadoDTO
                {
                    Recurso_cod_en = (int)EnRecurso.VisualizarAjusteDemaisUsuarios,
                    Eh_habilitado = false
                },
                new AcessoUsuarioLogadoDTO
                {
                    Recurso_cod_en = (int)EnRecurso.RegistrarAjusteDemaisUsuarios,
                    Eh_habilitado = false
                }
            };
        }
    }
    public Acesso ParaAcesso(AcessoUsuarioLogadoDTO acesso)
    {
        var resultado = new Acesso
        {
            recurso_cod_en = acesso.Recurso_cod_en,
            eh_habilitado = acesso.Eh_habilitado
        };
        return Utilitarios.DevolverComNovoEspacoNaMemoria(resultado);
    }
}