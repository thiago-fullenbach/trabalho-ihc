using DDD.Api.Domain.Models.Enumerations;

namespace DDD.Api.Domain.Models.BusnModel;
public class UsuarioBusnModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public int HorasDiarias { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string UrlEndpointHashearSenha { get; set; } = string.Empty;
    public List<AcessoBusnModel> Acessos { get; set; } = new List<AcessoBusnModel>();
    public bool TemRecursoPermitido(EnRecurso recurso)
    {
        var acessoRecurso = Acessos.FirstOrDefault(x => x.CodigoRecurso == (int)recurso);
        return acessoRecurso?.EhHabilitado ?? false;
    }
    public bool EhAlteracaoSenha()
    {
        return !string.IsNullOrWhiteSpace(Senha);
    }
}