namespace PontoFacil.Api.Modelo;
public class Acesso
{
    public int usuario_id { get; set; }
    public int recurso_cod_en { get; set; }
    public bool? eh_habilitado { get; set; }
    public DateTime datahora_criacao { get; set; }
    public DateTime? datahora_modificacao { get; set; }
    public virtual Usuario NavegacaoUsuario { get; set; }
}