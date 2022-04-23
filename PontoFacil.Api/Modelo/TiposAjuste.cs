namespace PontoFacil.Api.Modelo;
public class TiposAjuste
{
    public TiposAjuste()
    {
        NavegacaoMuitosAjustes = new List<Ajustes>();
    }
    public int id { get; set; }
    public string descricao { get; set; }
    public bool? eh_inclusao { get; set; }
    public bool? eh_alteracao { get; set; }
    public bool? eh_exclusao { get; set; }
    public virtual IList<Ajustes> NavegacaoMuitosAjustes { get; set; }
}