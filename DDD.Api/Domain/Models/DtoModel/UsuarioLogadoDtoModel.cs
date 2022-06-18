namespace DDD.Api.Domain.Models.DtoModel;
public class UsuarioLogadoDtoModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string CPF { get; set; }
    public DateTime Data_nascimento { get; set; }
    public int Horas_diarias { get; set; }
    public string Login { get; set; }
    public List<AcessoUsuarioLogadoDtoModel> Acessos { get; set; }
}