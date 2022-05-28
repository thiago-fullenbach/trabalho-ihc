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
        string texto = string.Empty;
        bool primeiraLinha = true;
        foreach (var iMensagem in listaMensagens)
        {
            if (primeiraLinha)
            {
                primeiraLinha = false;
                texto = iMensagem;
            }
            else { texto += Mensagens.PROXIMA_LINHA + iMensagem; }
        }
        return texto;
    }
    public static List<string> DesmontaMensagemErro(string mensagemErro)
    {
        var mensagensArray = mensagemErro.Split(Mensagens.PROXIMA_LINHA);
        var mensagensLista = new List<string>(mensagensArray);
        return mensagensLista;
    }
}