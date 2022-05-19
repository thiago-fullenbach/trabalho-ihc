namespace PontoFacil.Api.Modelo;
public class Local
{
    public Local()
    {
        NavegacaoMuitosApelidos = new List<Apelido>();
        NavegacaoMuitasPresencas = new List<Presenca>();
        NavegacaoMuitosAjustes = new List<Ajuste>();
    }
    public int id { get; set; }
    public string logradouro { get; set; }
    public string numero { get; set; }
    public string complemento { get; set; }
    public string bairro { get; set; }
    public string cidade { get; set; }
    public string uf { get; set; }
    public DateTime datahora_criacao { get; set; }
    public DateTime? datahora_modificacao { get; set; }
    public virtual IList<Apelido> NavegacaoMuitosApelidos { get; set; }
    public virtual IList<Presenca> NavegacaoMuitasPresencas { get; set; }
    public virtual IList<Ajuste> NavegacaoMuitosAjustes { get; set; }
}