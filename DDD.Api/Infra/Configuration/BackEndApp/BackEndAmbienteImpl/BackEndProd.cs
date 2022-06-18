using DDD.Api.Domain.Interface.Infra.Configuration.BackEndApp;

namespace DDD.Api.Infra.Configuration.BackEndApp.BackEndAmbienteImpl;
public class BackEndProd : IBackEndAmbiente
{
    public string GetUrlDominioExpostaExternamente()
    {
        return "https://ihc-n-ponto-facil-api.herokuapp.com";
    }
}