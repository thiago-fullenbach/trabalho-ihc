using System.Dynamic;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Repositorio;
public class DatabaseRepositorio
{
    private PontoFacilContexto _contexto;
    public DatabaseRepositorio(PontoFacilContexto contexto)
    {
        _contexto = contexto;
    }

    public CargaBancoDTO ExportarBanco()
    {
        var properties = typeof(PontoFacilContexto).GetProperties();
        var dbSetProperties = properties.Where(x => x.PropertyType.FullName.Contains("DbSet"));
        var cargaBanco = new CargaBancoDTO();
        foreach (var iDbSetProperty in dbSetProperties)
            { cargaBanco.Tabelas.Add(ExportarTabela(iDbSetProperty)); }
        return cargaBanco;
    }
    public CargaTabelaDTO ExportarTabela(PropertyInfo? dbSetProperty)
    {
        var dbset = (IQueryable)(dbSetProperty.GetValue(_contexto));
        var cargaTabela = new CargaTabelaDTO();
        cargaTabela.Nome = dbSetProperty.Name;
        foreach (var registro in dbset)
            { cargaTabela.Registros.Add(ParaExpandoObject(dbset.ElementType, registro)); }
        return cargaTabela;
    }
    public ExpandoObject ParaExpandoObject(Type tipo, object registro)
    {
        var properties = tipo.GetProperties();
        var propertiesNaoVirtual = properties.Where(x => !(x.GetGetMethod().IsVirtual));
        var exo = new ExpandoObject();
        foreach (var iProperty in propertiesNaoVirtual)
        {
            var exoParChaveValor = (ICollection<KeyValuePair<string, object>>)exo;
            var addColuna = new KeyValuePair<string, object>(key: iProperty.Name, value: iProperty.GetValue(registro));
            exoParChaveValor.Add(addColuna);
        }
        return exo;
    }
    public async Task ImportarBanco(CargaBancoDTO cargaBanco)
    {
        var properties = typeof(PontoFacilContexto).GetProperties();
        var dbSetProperties = properties.Where(x => x.PropertyType.FullName.Contains("DbSet"));
        foreach (var iDbSetProperty in dbSetProperties)
            { TruncaTabela(iDbSetProperty); }
        await _contexto.SaveChangesAsync();
        foreach (var tabela in cargaBanco.Tabelas)
        {
            var dbsetProperty = typeof(PontoFacilContexto).GetProperty(tabela.Nome);
            await InsereRegistros(dbsetProperty, tabela.Registros);
        }
        await _contexto.SaveChangesAsync();
    }
    public void TruncaTabela(PropertyInfo? dbSetProperty)
    {
        var dbset = dbSetProperty.GetValue(_contexto);
        var arrayDbset = ParaArrayDinamico((IQueryable)dbset);
        var metodosDbset = dbset.GetType().GetMethods();
        var removeRange = metodosDbset.First(x => x.Name == "RemoveRange");
        removeRange.Invoke(dbset, new object?[] { arrayDbset });
    }
    public Array? ParaArrayDinamico(IQueryable itens)
    {
        return ParaArrayDinamicoComTipo(itens, itens.ElementType);
    }
    public Array? ParaArrayDinamicoComTipo(IQueryable itens, Type tipo)
    {
        var listaDbset = Utilitarios.ParaLista(itens);
        var arrayDbset = Array.CreateInstance(tipo, listaDbset.Count);
        int i = 0;
        foreach (var iRegistro in itens)
        {
            arrayDbset.SetValue(iRegistro, i);
            i++;
        }
        return arrayDbset;
    }
    public async Task InsereRegistros(PropertyInfo? dbsetProperty, IList<ExpandoObject> registros)
    {
        var dbset = dbsetProperty.GetValue(_contexto);
        var tipoElemento = ((IQueryable)dbset).ElementType;
        var listaRegistros = new List<object>();
        foreach (var iRegistro in registros)
            { listaRegistros.Add(ParaObject(tipoElemento, iRegistro)); }
        var query = (listaRegistros).AsQueryable();
        var arrayRegistros = ParaArrayDinamicoComTipo(query, tipoElemento);
        var metodosDbset = dbset.GetType().GetMethods();
        var addRangeAsync = metodosDbset.First(x => x.Name == "AddRangeAsync");
        var taskAddRangeAsync = (Task)(addRangeAsync.Invoke(dbset, new object?[] { arrayRegistros } ));
        await taskAddRangeAsync;
    }
    public object? ParaObject(Type tipo, ExpandoObject exo)
    {
        var objeto = Activator.CreateInstance(tipo);
        var exoParChaveValor = (ICollection<KeyValuePair<string, object>>)exo;
        foreach (var iParChaveValor in exoParChaveValor)
        {
            var propertyRegistro = tipo.GetProperty(iParChaveValor.Key);
            var tipoPropertyRegistro = propertyRegistro.PropertyType;
            if (iParChaveValor.Value == null) { continue; }
            var valorCasted = ((JsonElement)(iParChaveValor.Value)).Deserialize(tipoPropertyRegistro);
            if (valorCasted != null)
                { propertyRegistro.SetValue(objeto, valorCasted); }
        }
        return objeto;
    }
}