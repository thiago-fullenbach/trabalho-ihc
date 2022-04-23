namespace PontoFacil.Api.Modelo;
public class Usuarios
{
    public Usuarios()
    {
        NavegacaoMuitosSessoes = new List<Sessoes>();
        NavegacaoMuitosApelidos = new List<Apelidos>();
        NavegacaoMuitosPresencas = new List<Presencas>();
        NavegacaoMuitosAjustes = new List<Ajustes>();
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
    public virtual Recursos? NavegacaoRecursos { get; set; }
    public virtual IList<Sessoes> NavegacaoMuitosSessoes { get; set; }
    public virtual IList<Apelidos> NavegacaoMuitosApelidos { get; set; }
    public virtual IList<Presencas> NavegacaoMuitosPresencas { get; set; }
    public virtual IList<Ajustes> NavegacaoMuitosAjustes { get; set; }
}