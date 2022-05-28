using System.Dynamic;

namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
public class CargaTabelaDTO
{
    public CargaTabelaDTO() => Registros = new List<ExpandoObject>();
    public string Nome { get; set; } = string.Empty;
    public IList<ExpandoObject> Registros { get; set; }
}