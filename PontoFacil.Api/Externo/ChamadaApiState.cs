namespace PontoFacil.Api.Externo;
public class ChamadaApiState
{
    public string UrlHttpServidor { get; set; }
    public string PegaUrlHasheiaSenhaSemParametros()
    {
        return $"{UrlHttpServidor}/api/v1/Autorizacao/hasheiaSenha";
    }
    public string PegaUrlHasheiaSenha(string senha)
    {
        return $"{PegaUrlHasheiaSenhaSemParametros()}?senhaRaw={senha}";
    }
    public virtual async Task<string> HasheiaSenha(string senha)
    {
        throw new NotImplementedException();
    }
}