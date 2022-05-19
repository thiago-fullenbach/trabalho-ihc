namespace PontoFacil.Api.Modelo;
public class Sessao
{
    public int id { get; set; }
    public string hex_verificacao { get; set; }
    public int usuario_id { get; set; }
    public DateTime datahora_ultima_autenticacao { get; set; }
    public virtual Usuario NavegacaoUsuario { get; set; }
}