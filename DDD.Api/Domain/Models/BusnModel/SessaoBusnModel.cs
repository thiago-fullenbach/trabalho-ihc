namespace DDD.Api.Domain.Models.BusnModel;
public class SessaoBusnModel
{
    public int Id { get; set; }
    public string CodigoVerificacao { get; set; } = string.Empty;
    public int IdUsuario { get; set; }
    public DateTime UltimaAutenticacao { get; set; }
    public UsuarioBusnModel UsuarioAutenticado { get; set; }
}