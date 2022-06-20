namespace DDD.Api.Domain.Models.BusnModel;
public class LocalBusnModel
{
    public int Id { get; set; }
    public string CodigoCep { get; set; } = string.Empty;
    public string NomeLogradouro { get; set; } = string.Empty;
    public string NumeroLogradouro { get; set; } = string.Empty;
    public string ComplementoLogradouro { get; set; } = string.Empty;
    public string NomeBairro { get; set; } = string.Empty;
    public string NomeCidade { get; set; } = string.Empty;
    public string NomeEstado { get; set; } = string.Empty;
}