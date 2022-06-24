using DDD.Api.Business.Services.DataServices;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Infra.Configuration.Database;

namespace DDD.Api.Infra.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly MongoDbConnection _connection;
    private readonly MongoDbTransactionDataService _mongoDbTransactionDataService;
    public UnitOfWork(MongoDbConnection connection, MongoDbTransactionDataService mongoDbTransactionDataService)
    {
        _connection = connection;
        _mongoDbTransactionDataService = mongoDbTransactionDataService;
    }

    public async Task<ITransaction> StartTransactionAsync()
    {
        var session = await _connection.Client.StartSessionAsync();
        session.StartTransaction();
        var transaction = new MongoDbTransaction(session);
        _mongoDbTransactionDataService.SetMongoDbTransaction(transaction);
        return transaction;
    }
}