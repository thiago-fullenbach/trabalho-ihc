namespace PontoFacil.Api.Modelo;
public class Apelidos
{
    public int usuarios_id { get; set; }
    public int locais_id { get; set; }
    public string apelido { get; set; }
    public DateTime datahora_criacao { get; set; }
    public DateTime? datahora_modificacao { get; set; }
    public virtual Usuarios NavegacaoUsuarios { get; set; }
    public virtual Locais NavegacaoLocais { get; set; }
}