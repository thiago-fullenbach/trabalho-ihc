using System.Linq.Expressions;
using System.Security.Cryptography;
using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Domain.Models.RepoModel;
using DDD.Api.Infra.Configuration.Database;

namespace DDD.Api.Infra.Repositories;
public class SessaoRepository : RepositoryBase<Sessao>, ISessaoRepository
{
    public SessaoRepository(IUnitOfWork uow, MongoDbConnection connection, IDatabaseConfiguration databaseConfiguration)
        : base(uow, connection, databaseConfiguration)
    {
        NomeColecaoEntidade = databaseConfiguration.GetNomeColecaoSessoes();
    }

    protected override Expression<Func<Sessao, bool>> MontarCallbackFindById(int id)
    {
        var idEm24Digit = FormatTo24DigitHex(id.ToString());
        return x => x.Id == idEm24Digit;
    }

    protected override string GetId(Sessao sessao)
    {
        return sessao.Id;
    }

    protected override void SetId(Sessao sessao, string id)
    {
        sessao.Id = id;
    }

    public string NovoCodigoVerificacao()
    {
        var r = new Random(Guid.NewGuid().GetHashCode());
        int DATA_SIZE = 128;
        byte[] data = new byte[DATA_SIZE];
        r.NextBytes(data);
        byte[] result;
        SHA512 shaM = SHA512.Create();
        result = shaM.ComputeHash(data);
        string bytesHexFormat = string.Empty;
        foreach (var x in result)
        { bytesHexFormat += x.ToString("X2"); } 
        return bytesHexFormat;
    }
}