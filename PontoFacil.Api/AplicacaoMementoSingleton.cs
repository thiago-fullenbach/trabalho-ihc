using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using PontoFacil.Api.Controlador.Servico;

namespace PontoFacil.Api;
public class AplicacaoMementoSingleton
{
    private static AplicacaoMementoSingleton _instancia;
    private AplicacaoMementoSingleton()
    {
    }
    public static AplicacaoMementoSingleton PegaInstancia()
    {
        if (_instancia != null)
            { return _instancia; }
        _instancia = new AplicacaoMementoSingleton();
        return _instancia;
    }
    public WebApplicationBuilder Builder { get; set; }
    public WebApplication App { get; set; }
    public IServiceProvider ServiceProvider { get; set; }
    public string UrlHasheiaSenhaSemParametros { get; set; }
    public IList<string> UrlsServidor
    {
        get
        {
            return new List<string>(Builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey).Replace("*", "localhost").Split(';'));
        }
    }
    public string UrlHttpsServidor
    {
        get
        {
            foreach (var x in UrlsServidor)
                { if (x.Contains("https:")) { return x; } }
            return string.Empty;
        }
    }
    public string UrlHttpServidor
    {
        get
        {
            foreach (var x in UrlsServidor)
                { if (x.Contains("http:")) { return x; } }
            return string.Empty;
        }
    }
    public AplicacaoMementoState CriaEstadoAplicacao()
    {
        var config = ServiceProvider.GetService<ConfiguracoesServico>();
        if (config.Segredo.StartsWith("Sim"))
            { return new AplicacaoMementoDesenv(UrlHttpServidor); }
        else { return new AplicacaoMementoProd(); }
    }
    public string UrlHttpExposta
    {
        get
        {
            var estadoAplicacao = CriaEstadoAplicacao();
            return estadoAplicacao.UrlDominio;
        }
    }
    public string PegaUrlHasheiaSenhaSemParametros()
    {
        return $"{UrlHttpExposta}/api/v1/Autorizacao/hasheiaSenha";
    }
}