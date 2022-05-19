namespace PontoFacil.Api.Modelo;
public class Apelido
{
    public int usuario_id { get; set; }
    public int local_id { get; set; }
    public string apelido { get; set; }
    public DateTime datahora_criacao { get; set; }
    public DateTime? datahora_modificacao { get; set; }
    public virtual Usuario NavegacaoUsuario { get; set; }
    public virtual Local NavegacaoLocal { get; set; }
}