namespace PontoFacil.Api.Modelo;
public class Sessoes
{
    public int id { get; set; }
    public string hexVerificacao { get; set; }
    public int usuarios_id { get; set; }
    public DateTime datahora_ultima_autenticacao { get; set; }
    public virtual Usuarios NavegacaoUsuarios { get; set; }
}