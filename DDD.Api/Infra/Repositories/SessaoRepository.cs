using System.Linq.Expressions;
using System.Security.Cryptography;
using DDD.Api.Business;
using DDD.Api.Business.Services.DataServices;
using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Domain.Models.RepoModel;
using DDD.Api.Infra.Configuration.Database;
using MongoDB.Driver;

namespace DDD.Api.Infra.Repositories;
public class SessaoRepository : RepositoryBase<Sessao>, ISessaoRepository
{
    public SessaoRepository(IUnitOfWork uow,
                            MongoDbConnection connection,
                            IDatabaseConfiguration databaseConfiguration,
                            MongoDbTransactionDataService mongoDbTransactionDataService)
        : base(uow, connection, databaseConfiguration, mongoDbTransactionDataService)
    {
        NomeColecaoEntidade = databaseConfiguration.GetNomeColecaoSessoes();
    }
    public async Task<List<Sessao>> SelectAllAsync()
    {
        var transaction = _mongoDbTransactionDataService.GetMongoDbTransaction();
        var sessoes = transaction == null
            ? await GetCollection().Find(_ => true).ToListAsync()
            : await GetCollection().Find(transaction.GetSessionWrappee(), _ => true).ToListAsync();
        foreach (var sessao in sessoes)
        {
            sessao.Id = FormatToStringifiedNumber(sessao.Id);
            sessao.usuario_id = FormatToStringifiedNumber(sessao.usuario_id);
        }
        return sessoes;
    }

    public async Task<Sessao?> SelectByIdOrDefaultAsync(int id)
    {
        var transaction = _mongoDbTransactionDataService.GetMongoDbTransaction();
        var id24Digit = FormatTo24DigitHex(id.ToString());
        var sessao = transaction == null
            ? await GetCollection().Find(x => x.Id == id24Digit).FirstOrDefaultAsync()
            : await GetCollection().Find(transaction.GetSessionWrappee(), x => x.Id == id24Digit).FirstOrDefaultAsync();
        if (sessao == null)
        {
            return null;
        }
        sessao.Id = FormatToStringifiedNumber(sessao.Id);
        sessao.usuario_id = FormatToStringifiedNumber(sessao.usuario_id);
        return sessao;
    }

    public async Task<int> SelectNextInsertIdAsync()
    {
        var sessoes = await SelectAllAsync();
        int maxId = 0;
        foreach (var sessao in sessoes)
        {
            int id = sessao.Id.ParseZeroIfFails();
            if (id > maxId)
            {
                maxId = id;
            }
        }
        return maxId + 1;
    }

    public async Task InsertAsync(int insertId, Sessao sessao)
    {
        if (await ExistsAsync(insertId))
        {
            throw new ArgumentException("insertId já existe no banco de dados.");
        }
        sessao.Id = FormatTo24DigitHex(insertId.ToString());
        sessao.usuario_id = FormatTo24DigitHex(sessao.usuario_id);
        var transaction = _mongoDbTransactionDataService.GetMongoDbTransaction();
        if (transaction == null)
        {
            await GetCollection().InsertOneAsync(sessao);
        }
        else
        {
            await GetCollection().InsertOneAsync(transaction.GetSessionWrappee(), sessao);
        }
    }

    public async Task UpdateAsync(int id, Sessao sessao)
    {
        if (!(await ExistsAsync(id)))
        {
            throw new ArgumentException("sessao com esse id não encontrado.");
        }
        sessao.Id = FormatTo24DigitHex(id.ToString());
        sessao.usuario_id = FormatTo24DigitHex(sessao.usuario_id);
        var transaction = _mongoDbTransactionDataService.GetMongoDbTransaction();
        if (transaction == null)
        {
            await GetCollection().ReplaceOneAsync(x => x.Id == sessao.Id, sessao);
        }
        else
        {
            await GetCollection().ReplaceOneAsync(transaction.GetSessionWrappee(), x => x.Id == sessao.Id, sessao);
        }
    }

    public async Task DeleteAsync(int id)
    {
        if (!(await ExistsAsync(id)))
        {
            throw new ArgumentException("sessao com esse id não encontrado.");
        }
        var id24Digit = FormatTo24DigitHex(id.ToString());
        var transaction = _mongoDbTransactionDataService.GetMongoDbTransaction();
        if (transaction == null)
        {
            await GetCollection().DeleteOneAsync(x => x.Id == id24Digit);
        }
        else
        {
            await GetCollection().DeleteOneAsync(transaction.GetSessionWrappee(), x => x.Id == id24Digit);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var id24Digit = FormatTo24DigitHex(id.ToString());
        var transaction = _mongoDbTransactionDataService.GetMongoDbTransaction();
        var sessaoExiste = transaction == null
            ? await GetCollection().Find(x => x.Id == id24Digit).AnyAsync()
            : await GetCollection().Find(transaction.GetSessionWrappee(), x => x.Id == id24Digit).AnyAsync();
        return sessaoExiste;
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