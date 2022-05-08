using System.Text;

namespace PontoFacil.Api.Controlador.Repositorio.Comum;
public class NegocioException : Exception
{
    public int StatusCodeErro { get; set; }
    public NegocioException(string mensagem) : base(mensagem) { }
    public static void ThrowErroSeHouver(List<string> listaMensagens, int statusCode)
    {
        if (listaMensagens.Count > 0)
            { throw new NegocioException(MontaMensagemErro(listaMensagens)) { StatusCodeErro = statusCode }; }
    }
    public static string MontaMensagemErro(List<string> listaMensagens)
    {
        var saida_texto = new StringBuilder();
        saida_texto.AppendLine(Mensagens.ERRO_NEGOCIO_CABECALHO);
        foreach (var iMensagem in listaMensagens)
        {
            saida_texto.AppendLine(string.Format(Mensagens.ERRO_NEGOCIO_ITEM_XXXX, iMensagem));
        }
        return saida_texto.ToString();
    }
}