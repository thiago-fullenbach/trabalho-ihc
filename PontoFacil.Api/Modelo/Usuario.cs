namespace PontoFacil.Api.Modelo;
public class Usuario
{
    public Usuario()
    {
        ContaMuitosNavegacao = new List<Conta>();
        AcessoMuitosNavegacao = new List<Acesso>();
        SessaoAbertaMuitosNavegacao = new List<SessaoAberta>();
    }
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public DateTime DataNascimento { get; set; }
    public virtual List<Conta> ContaMuitosNavegacao { get; set; }
    public virtual List<Acesso> AcessoMuitosNavegacao { get; set; }
    public virtual List<SessaoAberta> SessaoAbertaMuitosNavegacao { get; set; }
}