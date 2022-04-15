namespace PontoFacil.Api.Modelo;
public class Recurso
{
    public Recurso()
    {
        AcessoMuitosNavegacao = new List<Acesso>();
    }
    public int Id { get; set; }
    public string? Nome { get; set; }
    public virtual List<Acesso> AcessoMuitosNavegacao { get; set; }
}