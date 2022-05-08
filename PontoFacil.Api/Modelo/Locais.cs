namespace PontoFacil.Api.Modelo;
public class Locais
{
    public Locais()
    {
        NavegacaoMuitosApelidos = new List<Apelidos>();
        NavegacaoMuitosPresencas = new List<Presencas>();
        NavegacaoMuitosAjustes = new List<Ajustes>();
    }
    public int id { get; set; }
    public int tiposLogradouro_id { get; set; }
    public string logradouro { get; set; }
    public string numero { get; set; }
    public string complemento { get; set; }
    public string bairro { get; set; }
    public int cidades_id { get; set; }
    public DateTime datahora_criacao { get; set; }
    public DateTime? datahora_modificacao { get; set; }
    public virtual TiposLogradouro NavegacaoTiposLogradouro { get; set; }
    public virtual Cidades NavegacaoCidades { get; set; }
    public virtual IList<Apelidos> NavegacaoMuitosApelidos { get; set; }
    public virtual IList<Presencas> NavegacaoMuitosPresencas { get; set; }
    public virtual IList<Ajustes> NavegacaoMuitosAjustes { get; set; }
}