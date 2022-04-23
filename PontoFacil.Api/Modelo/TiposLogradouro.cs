namespace PontoFacil.Api.Modelo;
public class TiposLogradouro
{
    public TiposLogradouro()
    {
        NavegacaoMuitosLocais = new List<Locais>();
    }
    public int id { get; set; }
    public string descricao { get; set; }
    public virtual IList<Locais> NavegacaoMuitosLocais { get; set; }
}