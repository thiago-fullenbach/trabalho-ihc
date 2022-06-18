namespace DDD.Api.Domain.Models.BusnModel;
public class PresencaBusnModel
{
    public int Id { get; set; }
    public int IdUsuarioPresente { get; set; }
    public DateTime Hora { get; set; }
    public int IdLocal { get; set; }
    public bool EhEntrada { get; set; }
    public bool FoiAprovada { get; set; }
    public UsuarioBusnModel UsuarioPresente { get; set; }
    public LocalBusnModel Local { get; set; }
}