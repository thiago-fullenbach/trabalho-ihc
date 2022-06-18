using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Infra.Configuration.Database;

namespace DDD.Api.Infra.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly MongoDbConnection _connection;
    public UnitOfWork(MongoDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<ITransaction> StartTransactionAsync()
    {
        var session = await _connection.Client.StartSessionAsync();
        session.StartTransaction();
        var transaction = new MongoDbTransaction(session);
        return transaction;
    }
}