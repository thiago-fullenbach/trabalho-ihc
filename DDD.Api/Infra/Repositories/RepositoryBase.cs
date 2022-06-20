using System.Linq.Expressions;
using System.Reflection;
using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Infra.Configuration.Database;
using MongoDB.Driver;

namespace DDD.Api.Infra.Repositories;
public class RepositoryBase<T> where T : new()
{
    protected readonly IUnitOfWork _uow;
    protected readonly MongoDbConnection _connection;
    protected readonly IDatabaseConfiguration _databaseConfiguration;
    public RepositoryBase(IUnitOfWork uow, MongoDbConnection connection, IDatabaseConfiguration databaseConfiguration)
    {
        _uow = uow;
        _connection = connection;
        _databaseConfiguration = databaseConfiguration;
    }
    protected string? NomeColecaoEntidade { get; set; }

    protected IMongoCollection<T> GetCollection()
    {
        var collection = _connection.Client.GetDatabase(_databaseConfiguration.GetNomeBancoDados())
            .GetCollection<T>(NomeColecaoEntidade);
        return collection;
    }

    protected string FormatTo24DigitHex(string id)
    {
        return id.PadLeft(24, '0');
    }

    protected string FormatToStringifiedNumber(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return string.Empty;
        }
        int contagemZeros = 0;
        for (; id[contagemZeros] != '0'; contagemZeros++)
        {
        }
        if (contagemZeros == 0)
        {
            return id;
        }
        else
        {
            return id.Substring(contagemZeros);
        }
    }
}