namespace PontoFacil.Api.Modelo;
public class Cidades
{
    public Cidades()
    {
        NavegacaoMuitosLocais = new List<Locais>();
    }
    public int id { get; set; }
    public string nome { get; set; }
    public int ufs_id { get; set; }
    public virtual UFs NavegacaoUFs { get; set; }
    public virtual IList<Locais> NavegacaoMuitosLocais { get; set; }
}