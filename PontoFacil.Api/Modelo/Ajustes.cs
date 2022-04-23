namespace PontoFacil.Api.Modelo;
public class Ajustes
{
    public int id { get; set; }
    public int usuarios_id_modificador { get; set; }
    public int? presencas_id { get; set; }
    public int? locais_id_ajuste { get; set; }
    public bool? eh_entrada_ajuste { get; set; }
    public DateTime? datahora_presenca_ajuste { get; set; }
    public int tipo_ajuste { get; set; }
    public string observacao { get; set; }
    public DateTime datahora_criacao { get; set; }
    public DateTime? datahora_modificacao { get; set; }
    public virtual Presencas? NavegacaoPresencas { get; set; }
    public virtual Usuarios NavegacaoUsuariosModificador { get; set; }
    public virtual Locais NavegacaoLocaisAjuste { get; set; }
    public virtual TiposAjuste NavegacaoTiposAjuste { get; set; }
}