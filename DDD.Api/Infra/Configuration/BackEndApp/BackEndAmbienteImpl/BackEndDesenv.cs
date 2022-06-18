using DDD.Api.Domain.Interface.Infra.Configuration.BackEndApp;

namespace DDD.Api.Infra.Configuration.BackEndApp.BackEndAmbienteImpl;
public class BackEndDesenv : IBackEndAmbiente
{
    private readonly IConfiguration _configuration;
    public BackEndDesenv(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetUrlDominioExpostaExternamente()
    {
        var urls = _configuration["ASPNETCORE_URLS"].Split(';');
        var urlHttp = urls.First(x => x.Contains("http://"));
        return urlHttp;
    }
}