namespace PontoFacil.Api.Modelo;
public class Ajuste
{
    public int id { get; set; }
    public int usuario_ajustador_id { get; set; }
    public int? presenca_id { get; set; }
    public int usuario_presente_id { get; set; }
    public DateTime? datahora_presenca { get; set; }
    public int local_id { get; set; }
    public int tipo_presenca_cod_en { get; set; }
    public int primitiva_ajuste_cod_en { get; set; }
    public string observacao { get; set; }
    public DateTime datahora_criacao { get; set; }
    public DateTime? datahora_modificacao { get; set; }
    public virtual Usuario NavegacaoUsuarioAjustador { get; set; }
    public virtual Presenca? NavegacaoPresenca { get; set; }
    public virtual Usuario NavegacaoUsuarioPresente { get; set; }
    public virtual Local NavegacaoLocal { get; set; }
}