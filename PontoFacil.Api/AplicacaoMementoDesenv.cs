namespace PontoFacil.Api;
public class AplicacaoMementoDesenv : AplicacaoMementoState
{
    public AplicacaoMementoDesenv(string urlListeningPort)
        => UrlDominio = urlListeningPort;
}