namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
public class SessaoEnviarPeloHeaderDTO
{
    public int? Id { get; set; }
    public string? HexVerificacao { get; set; }
    public DateTime? Datahora_ultima_autenticacao { get; set; }
}