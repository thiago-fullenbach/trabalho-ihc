namespace DDD.Api.Domain.Models.DtoModel;
public class SessaoEnvioHeaderDtoModel
{
    public int? Id { get; set; }
    public string? Hex_verificacao { get; set; }
    public DateTime? Datahora_ultima_autenticacao { get; set; }
}