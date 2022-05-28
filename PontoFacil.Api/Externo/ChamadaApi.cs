namespace PontoFacil.Api.Externo;
public class ChamadaApi
{
    public bool EstouraExcecaoQuandoStatusCodeDiferente200 = true;
    public ChamadaApiState? ChamadaApiStateCriado { get; set; }
    public async Task<string> HasheiaSenha(string senha)
    {
        return await ChamadaApiStateCriado.HasheiaSenha(senha);
    }
    public string PegaUrlHttpServidorPelaUrlHasheiaSenhaSemParametros(string urlHasheiaSenhaSemParametros)
    {
        var dividido = urlHasheiaSenhaSemParametros.Split('/');
        return $"{dividido[0]}//{dividido[2]}";
    }
    public ChamadaApiState CriaStatePelaUrlHasheiaSenhaSemParametros(string urlHasheiaSenhaSemParametros)
    {
        string urlHttpServidorUsuarioBanco = PegaUrlHttpServidorPelaUrlHasheiaSenhaSemParametros(urlHasheiaSenhaSemParametros);
        var intscAplicacao = AplicacaoMementoSingleton.PegaInstancia();
        string urlHasheiaSenhaAplicacao = intscAplicacao.PegaUrlHasheiaSenhaSemParametros();
        string urlHttpServidorAplicacao = PegaUrlHttpServidorPelaUrlHasheiaSenhaSemParametros(urlHasheiaSenhaAplicacao);
        if (urlHttpServidorUsuarioBanco == urlHttpServidorAplicacao)
            { return new ChamadaApiInterno(); }
        else { return new ChamadaApiExterno(urlHttpServidorUsuarioBanco); }
    }
}