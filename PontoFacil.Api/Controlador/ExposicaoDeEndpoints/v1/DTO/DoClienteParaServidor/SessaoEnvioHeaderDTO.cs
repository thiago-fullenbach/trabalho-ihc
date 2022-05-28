namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
public class SessaoEnvioHeaderDTO
{
    public int? Id { get; set; }
    public string? Hex_verificacao { get; set; }
    public DateTime? Datahora_ultima_autenticacao { get; set; }
}