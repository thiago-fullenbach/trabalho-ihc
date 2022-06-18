using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using MongoDB.Driver;

namespace DDD.Api.Infra.Configuration.Database;
public class MongoDbConnection
{
    private readonly IDatabaseConfiguration _databaseConfiguration;
    public MongoDbConnection(IDatabaseConfiguration databaseConfiguration)
    {
        _databaseConfiguration = databaseConfiguration;
        Client = new MongoClient(databaseConfiguration.GetStringConexao());
    }
    public MongoClient Client { get; private set; }
}