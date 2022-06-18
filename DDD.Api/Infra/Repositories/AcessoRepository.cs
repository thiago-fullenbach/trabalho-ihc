using System.Linq.Expressions;
using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Domain.Models.RepoModel;
using DDD.Api.Infra.Configuration.Database;
using MongoDB.Driver;

namespace DDD.Api.Infra.Repositories;
public class AcessoRepository : RepositoryBase<Acesso>, IAcessoRepository
{
    public AcessoRepository(IUnitOfWork uow, MongoDbConnection connection, IDatabaseConfiguration databaseConfiguration)
        : base(uow, connection, databaseConfiguration)
    {
        NomeColecaoEntidade = databaseConfiguration.GetNomeColecaoAcessos();
    }

    protected override Expression<Func<Acesso, bool>> MontarCallbackFindById(int id)
    {
        var idEm24Digit = FormatTo24DigitHex(id.ToString());
        return x => x.Id == idEm24Digit;
    }

    protected override string GetId(Acesso acesso)
    {
        return acesso.Id;
    }

    protected override void SetId(Acesso acesso, string id)
    {
        acesso.Id = id;
    }
    public async Task<List<Acesso>> SelectByUsuarioAsync(int idUsuario)
    {
        var idEm24Digit = FormatTo24DigitHex(idUsuario.ToString());
        var acessos = await GetCollection().Find(x => x.usuario_id == idEm24Digit).ToListAsync();
        return acessos;
    }
}