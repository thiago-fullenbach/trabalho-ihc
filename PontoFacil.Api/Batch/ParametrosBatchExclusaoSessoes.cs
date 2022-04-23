namespace PontoFacil.Api.Batch;
public class ParametrosBatchExclusaoSessoes
{
    public static IList<string> UrlsServidor { get; set; }
    public static string? UrlHttpsServidor
    {
        get
        {
            foreach (var x in UrlsServidor)
                { if (x.Contains("https:")) { return x; } }
            return string.Empty;
        }
    }
    public static string? UrlBatchExclusaoSessoes
    {
        get
        {
            return $"{UrlHttpsServidor}/api/v1/Autorizacao/excluirSessoesExpiradas";
        }
    }
    public static IDictionary<string, string>? GuidToGuidCustomHeaders { get; set; }
    public static bool? EndpointAberto { get; set; }
}