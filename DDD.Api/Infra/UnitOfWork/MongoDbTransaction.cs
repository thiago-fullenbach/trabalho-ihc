using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using MongoDB.Driver;

namespace DDD.Api.Infra.UnitOfWork;
public class MongoDbTransaction : ITransaction
{
    private readonly IClientSessionHandle _sessionWrappee;
    public MongoDbTransaction(IClientSessionHandle session)
    {
        _sessionWrappee = session;
    }

    public async Task CommitAsync()
    {
        await _sessionWrappee.CommitTransactionAsync();
    }

    public async Task RollbackAsync()
    {
        await _sessionWrappee.AbortTransactionAsync();
    }

    public IClientSessionHandle GetSessionWrappee()
    {
        return _sessionWrappee;
    }
}