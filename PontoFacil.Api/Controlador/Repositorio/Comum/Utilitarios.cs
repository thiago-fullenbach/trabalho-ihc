using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
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
    public static IList<object> ParaLista(IQueryable queryable)
    {
        var listaTs = new List<object>();
        if (queryable == null) { return listaTs; }
        foreach (var x in queryable) { listaTs.Add(x); }
        return listaTs;
    }
    public static IList<T> ParaLista<T>(IQueryable<T>? queryable)
    {
        var listaTs = new List<T>();
        if (queryable == null) { return listaTs; }
        foreach (var x in queryable) { listaTs.Add(x); }
        return listaTs;
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
        field.SetValue(objeto, (TField)valorField);
    }
    public static void SetFieldPorReflection(Type tObjeto, object objeto, string nomeField, object valorField)
    {
        var field = tObjeto.GetField(nomeField, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(objeto, valorField);
    }
    public static string ParaExpressaoCron(DateTime dataHora)
    {
        var culturaEstadosUnidos = CultureInfo.GetCultureInfo("en-US");
        return dataHora.ToString("s m H d MMM ? yyyy/1", culturaEstadosUnidos).ToUpper();
    }
    public static ExpandoObject ParaExpandoObject<T>(T entidade)
    {
        var properties = typeof(T).GetProperties();
        var exo = new ExpandoObject();
        foreach (var iProperty in properties)
        {
            var exoParChaveValor = (ICollection<KeyValuePair<string, object>>)exo;
            var addColuna = new KeyValuePair<string, object>(key: iProperty.Name, value: iProperty.GetValue(entidade));
            exoParChaveValor.Add(addColuna);
        }
        return exo;
    }
    public static T ParaEntidade<T>(ExpandoObject exo) where T : new()
    {
        var entidade = new T();
        var exoParChaveValor = (ICollection<KeyValuePair<string, object>>)exo;
        foreach (var iParChaveValor in exoParChaveValor)
            { Utilitarios.SetFieldPorReflection(entidade, iParChaveValor.Key, iParChaveValor.Value); }
        return entidade;
    }
}