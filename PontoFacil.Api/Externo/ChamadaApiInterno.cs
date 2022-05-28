using PontoFacil.Api.Controlador.Servico;

namespace PontoFacil.Api.Externo;
public class ChamadaApiInterno : ChamadaApiState
{
    public ChamadaApiInterno()
    {
        UrlHttpServidor = AplicacaoMementoSingleton.PegaInstancia().UrlHttpServidor;
    }
    public override async Task<string> HasheiaSenha(string senha)
    {
        var serviceProvider = AplicacaoMementoSingleton.PegaInstancia().ServiceProvider;
        var configServico = serviceProvider.GetService<ConfiguracoesServico>();
        var criptoServico = serviceProvider.GetService<CriptografiaServico>();
        return criptoServico.HashearSenha(senha);
    }
}