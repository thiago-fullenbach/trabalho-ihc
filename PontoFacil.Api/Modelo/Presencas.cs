namespace PontoFacil.Api.Modelo;
public class Presencas
{
    public int id { get; set; }
    public int usuarios_id { get; set; }
    public int locais_id { get; set; }
    public bool? eh_entrada { get; set; }
    public DateTime datahora_presenca { get; set; }
    public virtual Usuarios NavegacaoUsuarios { get; set; }
    public virtual Locais NavegacaoLocais { get; set; }
    public virtual Ajustes? NavegacaoAjustes { get; set; }
}