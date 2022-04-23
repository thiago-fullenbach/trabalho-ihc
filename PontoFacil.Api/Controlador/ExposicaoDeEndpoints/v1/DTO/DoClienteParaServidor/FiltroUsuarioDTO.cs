namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
public class FiltroUsuarioDTO
{
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public DateTime? Data_nascimento { get; set; }
    public int? Horas_diarias { get; set; }
    public string? Login { get; set; }
}