namespace PontoFacil.Api.Modelo;
public class UFs
{
    public UFs()
    {
        NavegacaoMuitosCidades = new List<Cidades>();
    }
    public int id { get; set; }
    public string nome { get; set; }
    public virtual IList<Cidades> NavegacaoMuitosCidades { get; set; }
}