namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
public class ManterUsuarioComContaDTO
{
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public DateTime DataNascimento { get; set; }
    public string? Login { get; set; }
    public string? Senha { get; set; }
}