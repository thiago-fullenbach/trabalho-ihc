using PontoFacil.Api.Modelo;

namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
public class CargaBancoDTO
{
    public CargaBancoDTO()
    {
        Tabelas = new List<CargaTabelaDTO>();
    }
    public IList<CargaTabelaDTO> Tabelas { get; set; }
}