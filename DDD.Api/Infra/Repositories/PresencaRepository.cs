using System.Linq.Expressions;
using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Domain.Models.RepoModel;
using DDD.Api.Infra.Configuration.Database;

namespace DDD.Api.Infra.Repositories;
public class PresencaRepository : RepositoryBase<Presenca>, IPresencaRepository
{
    public PresencaRepository(IUnitOfWork uow, MongoDbConnection connection, IDatabaseConfiguration databaseConfiguration)
        : base(uow, connection, databaseConfiguration)
    {
        NomeColecaoEntidade = databaseConfiguration.GetNomeColecaoPresencas();
    }

    protected override Expression<Func<Presenca, bool>> MontarCallbackFindById(int id)
    {
        var idEm24Digit = FormatTo24DigitHex(id.ToString());
        return x => x.Id == idEm24Digit;
    }

    protected override string GetId(Presenca presenca)
    {
        return presenca.Id;
    }

    protected override void SetId(Presenca presenca, string id)
    {
        presenca.Id = id;
    }
}