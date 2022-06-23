namespace DDD.Api.Business;
public static class Utils
{
    public static int ParseZeroIfFails(this string? s)
    {
        int result = 0;
        bool success = int.TryParse(s, out result);
        if (!success)
        {
            return 0;
        }
        return result;
    }

    public static string RemoverAcentos(this string texto)
    {
        string comAcento = "áàãâäéèêëíìîïóòõôöúùûüñÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÔÖÚÙÛÜÑ";
        string semAcento = "aaaaaeeeeiiiiooooouuuunAAAAAEEEEIIIIOOOOOUUUUN";
        string textoSemAcentos = texto;
        for (int i = 0; i < comAcento.Length; i++)
        {
            textoSemAcentos = textoSemAcentos.Replace(comAcento[i], semAcento[i]);
        }
        return textoSemAcentos;
    }

    public static bool CpfTemDigitosIguais(this string? cpf)
    {
        if (cpf == null || cpf.Length <= 1)
        {
            return true;
        }
        char primeiroDigito = cpf[0];
        foreach (char digito in cpf)
        {
            if (digito != primeiroDigito)
            {
                return false;
            }
        }
        return true;
    }

    public static bool CpfObedeceRegraResto11(this string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf) || cpf.CpfTemDigitosIguais())
        {
            return false;
        }
        int totalMult10 = 0;
        int indexChar = 0;
        for (int mult = 10; mult >= 2; mult--)
        {
            totalMult10 += int.Parse(cpf[indexChar].ToString()) * mult;
            indexChar++;
        }
        int restoMult10 = totalMult10 % 11;
        int digito1 = restoMult10 <= 1 ? 0 : (11 - restoMult10);
        int totalMult11 = 0;
        indexChar = 0;
        for (int mult = 11; mult >= 2; mult--)
        {
            totalMult11 += int.Parse(cpf[indexChar].ToString()) * mult;
            indexChar++;
        }
        int restoMult11 = totalMult11 % 11;
        int digito2 = restoMult11 <= 1 ? 0 : (11 - restoMult11);
        int digitoMult10 = int.Parse(cpf[9].ToString());
        int digitoMult11 = int.Parse(cpf[10].ToString());
        return digitoMult10 == digito1 && digitoMult11 == digito2;
    }
}