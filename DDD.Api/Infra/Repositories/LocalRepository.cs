using System.Linq.Expressions;
using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Domain.Models.RepoModel;
using DDD.Api.Infra.Configuration.Database;

namespace DDD.Api.Infra.Repositories;
public class LocalRepository : RepositoryBase<Local>, ILocalRepository
{
    public LocalRepository(IUnitOfWork uow, MongoDbConnection connection, IDatabaseConfiguration databaseConfiguration)
        : base(uow, connection, databaseConfiguration)
    {
        NomeColecaoEntidade = databaseConfiguration.GetNomeColecaoLocais();
    }

    protected override Expression<Func<Local, bool>> MontarCallbackFindById(int id)
    {
        var idEm24Digit = FormatTo24DigitHex(id.ToString());
        return x => x.Id == idEm24Digit;
    }

    protected override string GetId(Local local)
    {
        return local.Id;
    }

    protected override void SetId(Local local, string id)
    {
        local.Id = id;
    }
    
}