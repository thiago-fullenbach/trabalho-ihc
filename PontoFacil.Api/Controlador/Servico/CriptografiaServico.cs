using System.Security.Cryptography;
using System.Text;

namespace PontoFacil.Api.Controlador.Servico;
public class CriptografiaServico
{
    private readonly ConfiguracoesServico _configuracoesServico;
    private readonly HMACSHA512 _hmacSha512;
    public CriptografiaServico(ConfiguracoesServico configuracoesServico)
    {
        _configuracoesServico = configuracoesServico;
        _hmacSha512 = new HMACSHA512(Encoding.UTF8.GetBytes(_configuracoesServico.Segredo));
    }
    public string HashearSenha(string senha)
    {
        var bytesSenha = Encoding.UTF8.GetBytes(senha);
        var hash = _hmacSha512.ComputeHash(bytesSenha);
        return BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
    }
    public static string HexAleatorioSeguro128Caracteres()
    {
        var r = new Random(Guid.NewGuid().GetHashCode());
        int DATA_SIZE = 128;
        byte[] data = new byte[DATA_SIZE];
        r.NextBytes(data);
        byte[] result;
        SHA512 shaM = SHA512.Create();
        result = shaM.ComputeHash(data);
        string bytesHexFormat = string.Empty;
        foreach (var x in result)
        { bytesHexFormat += x.ToString("X2"); } 
        return bytesHexFormat;
    }
}