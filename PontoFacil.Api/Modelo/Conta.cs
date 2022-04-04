namespace PontoFacil.Api.Modelo;
public class Conta
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Senha { get; set; }
    public int IdUsuario { get; set; }
    public DateTime DataHoraUltimaAlteracaoLogin { get; set; }
    public virtual Usuario? IdUsuarioNavegacao { get; set; }
}
