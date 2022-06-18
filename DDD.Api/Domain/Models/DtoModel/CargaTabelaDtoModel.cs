using System.Dynamic;

namespace DDD.Api.Domain.Models.DtoModel;
public class CargaTabelaDtoModel
{
    public CargaTabelaDtoModel() => Registros = new List<ExpandoObject>();
    public string Nome { get; set; } = string.Empty;
    public List<ExpandoObject> Registros { get; set; }
}