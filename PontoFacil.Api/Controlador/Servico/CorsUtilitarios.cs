namespace PontoFacil.Api.Controlador.Servico;
public class CorsUtilitarios
{
    public static bool PossuiDicionarioGuidToGuid(IHeaderDictionary headers, IDictionary<string, string> dicionarioGuidToGuid)
    {
        foreach (var chaveValor in dicionarioGuidToGuid)
        {
            if (!headers.ContainsKey(chaveValor.Key)) { return false; }
            if (headers[chaveValor.Key] != chaveValor.Value) { return false; }
        }
        return true;
    }
    public static IDictionary<string, string> GeraDicionarioGuidToGuid()
    {
        var randomServico = new Random(Guid.NewGuid().GetHashCode());
        int minimoChaves = 20; int maximoChaves = 30;
        int totalChaves = randomServico.Next(minimoChaves, maximoChaves + 1);
        var dicionario = new Dictionary<string, string>();
        foreach (var _ in new byte[totalChaves])
            { dicionario.TryAdd(CorsUtilitarios.GeraChaveHeaderAleatoria(), CriptografiaServico.HexAleatorioSeguro128Caracteres()); }
        return dicionario;
    }
    public static string GeraChaveHeaderAleatoria()
    {
        var hexBase = CriptografiaServico.HexAleatorioSeguro128Caracteres();
        string total = string.Empty;
        foreach (var caractere in hexBase)
        {
            var index = "0123456789ABCDEF".IndexOf(caractere);
            if (index == -1) { index = 0; }
            total += "abcdefghijklmnop"[index];
        }
        return total;
    }
}