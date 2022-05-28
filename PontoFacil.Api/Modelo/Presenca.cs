namespace PontoFacil.Api.Modelo;
public class Presenca
{
    public int id { get; set; }
    public int usuario_id { get; set; }
    public DateTime datahora_presenca { get; set; }
    public int local_id { get; set; }
    public int tipo_presenca_cod_en { get; set; }
    public virtual Usuario NavegacaoUsuario { get; set; }
    public virtual Local NavegacaoLocal { get; set; }
    public virtual Ajuste? NavegacaoAjuste { get; set; }
}