namespace DDD.Api.Domain.Models.DtoModel;
public class CargaBancoDtoModel
{
    public CargaBancoDtoModel()
    {
        Tabelas = new List<CargaTabelaDtoModel>();
    }
    public List<CargaTabelaDtoModel> Tabelas { get; set; }
}