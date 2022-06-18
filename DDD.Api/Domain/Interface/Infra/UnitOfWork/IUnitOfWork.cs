using MongoDB.Driver;

namespace DDD.Api.Domain.Interface.Infra.UnitOfWork;
public interface IUnitOfWork
{
    Task<ITransaction> StartTransactionAsync();
}