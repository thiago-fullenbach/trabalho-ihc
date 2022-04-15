namespace PontoFacil.Api.Modelo;
public class SessaoAberta
{
    public int Id { get; set; }
    public string? HexVerificacao { get; set; }
    public int IdUsuario { get; set; }
    public DateTime DataHoraUltimaAtualizacao { get; set; }
    public virtual Usuario? IdUsuarioNavegacao { get; set; }
}