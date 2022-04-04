namespace PontoFacil.Api.Modelo;
public class Acesso
{
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public int IdRecurso { get; set; }
    public virtual Usuario? IdUsuarioNavegacao { get; set; }
    public virtual Recurso? IdRecursoNavegacao { get; set; }
}
