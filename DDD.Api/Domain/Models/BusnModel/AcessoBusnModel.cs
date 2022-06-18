using DDD.Api.Domain.Models.Enumerations;

namespace DDD.Api.Domain.Models.BusnModel;
public class AcessoBusnModel
{
    public int IdUsuario { get; set; }
    public int CodigoRecurso { get; set; }
    public bool EhHabilitado { get; set; }
    public static List<AcessoBusnModel> GetAcessosPadrao()
    {
        return new List<AcessoBusnModel>
        {
            new AcessoBusnModel
            {
                CodigoRecurso = (int)EnRecurso.VisualizarUsuario,
                EhHabilitado = true
            },
            new AcessoBusnModel
            {
                CodigoRecurso = (int)EnRecurso.CadastrarUsuario,
                EhHabilitado = true
            },
            new AcessoBusnModel
            {
                CodigoRecurso = (int)EnRecurso.VisualizarDemaisUsuarios,
                EhHabilitado = false
            },
            new AcessoBusnModel
            {
                CodigoRecurso = (int)EnRecurso.CadastrarDemaisUsuarios,
                EhHabilitado = false
            },
            new AcessoBusnModel
            {
                CodigoRecurso = (int)EnRecurso.CadastrarAcessoTodosUsuarios,
                EhHabilitado = false
            },
            new AcessoBusnModel
            {
                CodigoRecurso = (int)EnRecurso.VisualizarPonto,
                EhHabilitado = true
            },
            new AcessoBusnModel
            {
                CodigoRecurso = (int)EnRecurso.RegistrarPonto,
                EhHabilitado = true
            },
            new AcessoBusnModel
            {
                CodigoRecurso = (int)EnRecurso.VisualizarPontoDemaisUsuarios,
                EhHabilitado = false
            },
            new AcessoBusnModel
            {
                CodigoRecurso = (int)EnRecurso.VisualizarAjuste,
                EhHabilitado = true
            },
            new AcessoBusnModel
            {
                CodigoRecurso = (int)EnRecurso.VisualizarAjusteDemaisUsuarios,
                EhHabilitado = false
            },
            new AcessoBusnModel
            {
                CodigoRecurso = (int)EnRecurso.RegistrarAjusteDemaisUsuarios,
                EhHabilitado = false
            }
        };
    }
    public static List<AcessoBusnModel> GetAcessosAdmin()
    {
        var acessos = new List<AcessoBusnModel>();
        for (int i = 1; i <= 11; i++)
        {
            acessos.Add(new AcessoBusnModel
            {
                CodigoRecurso = i,
                EhHabilitado = true
            });
        }
        return acessos;
    }
}