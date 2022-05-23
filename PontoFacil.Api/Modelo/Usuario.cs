namespace PontoFacil.Api.Modelo;
public class Usuario
{
    public Usuario()
    {
        NavegacaoMuitosAcessos = new List<Acesso>();
        NavegacaoMuitasSessoes = new List<Sessao>();
        NavegacaoMuitasPresencas = new List<Presenca>();
        NavegacaoMuitosAjustesPresentes = new List<Ajuste>();
        NavegacaoMuitosAjustesRegistrados = new List<Ajuste>();
    }
    public int id { get; set; }
    public string nome { get; set; }
    public string cpf { get; set; }
    public DateTime data_nascimento { get; set; }
    public int horas_diarias { get; set; }
    public string login { get; set; }
    public string senha { get; set; }
    public DateTime datahora_criacao { get; set; }
    public DateTime? datahora_modificacao { get; set; }
    public DateTime? datahora_modificacao_login { get; set; }
    public DateTime? datahora_modificacao_senha { get; set; }
    public bool? eh_senha_temporaria { get; set; }
    public virtual IList<Acesso> NavegacaoMuitosAcessos { get; set; }
    public virtual IList<Sessao> NavegacaoMuitasSessoes { get; set; }
    public virtual IList<Presenca> NavegacaoMuitasPresencas { get; set; }
    public virtual IList<Ajuste> NavegacaoMuitosAjustesPresentes { get; set; }
    public virtual IList<Ajuste> NavegacaoMuitosAjustesRegistrados { get; set; }
}