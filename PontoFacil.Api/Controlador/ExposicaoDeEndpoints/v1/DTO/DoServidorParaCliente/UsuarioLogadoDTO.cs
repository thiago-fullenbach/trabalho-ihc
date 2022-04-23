namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
public class UsuarioLogadoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string CPF { get; set; }
    public DateTime Data_nascimento { get; set; }
    public int Horas_diarias { get; set; }
    public string Login { get; set; }
    public UsuarioRecursoDTO NavegacaoRecurso { get; set; }
}