using DDD.Api.Infra.UnitOfWork;

namespace DDD.Api.Business.Services.DataServices;
public class MongoDbTransactionDataService
{
    private MongoDbTransaction? _mongoDbTransaction;
    public MongoDbTransaction? GetMongoDbTransaction()
    {
        return _mongoDbTransaction;
    }
    public void SetMongoDbTransaction(MongoDbTransaction mongoDbTransaction)
    {
        _mongoDbTransaction = mongoDbTransaction;
    }
}