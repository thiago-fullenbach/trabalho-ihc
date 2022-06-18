namespace DDD.Api.Domain.Models.DtoModel;
public class EditarUsuarioDtoModel
{   
    public int Id { get; set; }
    public string Nome { get; set; }
    public string CPF { get; set; }
    public DateTime Data_nascimento { get; set; }
    public int Horas_diarias { get; set; }
    public string Login { get; set; }
    public string? Nova_senha { get; set; }
    public List<AcessoUsuarioDtoModel> Acessos { get; set; }
}