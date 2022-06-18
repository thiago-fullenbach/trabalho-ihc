namespace DDD.Api.Domain.Interface.Infra.UnitOfWork;
public interface ITransaction
{
    Task CommitAsync();
    Task RollbackAsync();
}