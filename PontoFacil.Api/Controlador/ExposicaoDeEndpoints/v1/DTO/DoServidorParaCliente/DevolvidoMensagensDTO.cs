using PontoFacil.Api.Controlador.Repositorio.Comum;

namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
public class DevolvidoMensagensDTO
{
    public DevolvidoMensagensDTO()
    {
        Mensagens = new List<string>();
    }
    public object? Devolvido { get; set; }
    public IList<string> Mensagens { get; set; }
    public string GetMensagemUnica()
    {
        return NegocioException.MontaMensagemErro(new List<string>(Mensagens));
    }
    public void SetMensagemUnica(string value)
    {
        Mensagens = new List<string> { value };
    }
}