namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
public class SessaoAtualizadaDTO
{
    public int Id { get; set; }
    public string? HexVerificacao { get; set; }
    public DateTime DataHoraUltimaAtualizacao { get; set; }
    public List<RecursoPermitidoDTO> ListaRecursosPermitidos { get; set; }
}