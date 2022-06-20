namespace DDD.Api.Domain.Models.BusnModel;
public class PresencaBusnModel
{
    public int Id { get; set; }
    public int IdUsuarioPresente { get; set; }
    public bool EhEntrada { get; set; }
    public DateTime Hora { get; set; }
    public int IdLocal { get; set; }
    public bool TemVisto { get; set; }
    public UsuarioBusnModel UsuarioPresente { get; set; }
    public LocalBusnModel Local { get; set; }
}