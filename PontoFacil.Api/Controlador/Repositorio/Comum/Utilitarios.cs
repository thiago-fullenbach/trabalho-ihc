using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace PontoFacil.Api.Controlador.Repositorio.Comum;
public class Utilitarios
{
    public static T DevolverComNovoEspacoNaMemoria<T>(T objeto)
    {
        var objetoNaString = JsonConvert.SerializeObject(objeto);
        var objetoAlocado = JsonConvert.DeserializeObject<T>(objetoNaString);
        return objetoAlocado;
    }
    public static IList<T> ParaLista<T>(IEnumerable<T>? enumerable)
    {
        var listaTs = new List<T>();
        if (enumerable == null) { return listaTs; }
        foreach (var x in enumerable) { listaTs.Add(x); }
        return listaTs;
    }
    public static void SetFieldPorReflection<TObjeto, TField>(TObjeto objeto, string nomeField, TField valorField)
    {
        var field = (typeof(TObjeto)).GetField(nomeField, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(objeto, valorField);
    }
    public static string ParaExpressaoCron(DateTime dataHora)
    {
        var culturaEstadosUnidos = CultureInfo.GetCultureInfo("en-US");
        return dataHora.ToString("s m H d MMM ? yyyy/1", culturaEstadosUnidos).ToUpper();
    }
}