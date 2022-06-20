using System.Linq.Expressions;
using DDD.Api.Business;
using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Domain.Models.RepoModel;
using DDD.Api.Infra.Configuration.Database;
using MongoDB.Driver;

namespace DDD.Api.Infra.Repositories;
public class LocalRepository : RepositoryBase<Local>, ILocalRepository
{
    public LocalRepository(IUnitOfWork uow, MongoDbConnection connection, IDatabaseConfiguration databaseConfiguration)
        : base(uow, connection, databaseConfiguration)
    {
        NomeColecaoEntidade = databaseConfiguration.GetNomeColecaoLocais();
    }

    public async Task<List<Local>> SelectAllAsync()
    {
        var locais = await GetCollection().Find(_ => true).ToListAsync();
        foreach (var local in locais)
        {
            local.Id = FormatToStringifiedNumber(local.Id);
        }
        return locais;
    }

    public async Task<Local?> SelectByIdOrDefaultAsync(int id)
    {
        var id24Digit = FormatTo24DigitHex(id.ToString());
        var local = await GetCollection().Find(x => x.Id == id24Digit).FirstOrDefaultAsync();
        if (local == null)
        {
            return null;
        }
        local.Id = id.ToString();
        return local;
    }

    public async Task<int> SelectNextInsertIdAsync()
    {
        var locais = await SelectAllAsync();
        int maxId = 0;
        foreach (var local in locais)
        {
            int id = local.Id.ParseZeroIfFails();
            if (id > maxId)
            {
                maxId = id;
            }
        }
        return maxId + 1;
    }

    public async Task InsertAsync(int insertId, Local local)
    {
        if (await ExistsAsync(insertId))
        {
            throw new ArgumentException("insertId já existe no banco de dados.");
        }
        local.Id = FormatTo24DigitHex(insertId.ToString());
        await GetCollection().InsertOneAsync(local);
    }

    public async Task UpdateAsync(int id, Local local)
    {
        if (!(await ExistsAsync(id)))
        {
            throw new ArgumentException("local com esse id não encontrado.");
        }
        local.Id = FormatTo24DigitHex(id.ToString());
        await GetCollection().ReplaceOneAsync(x => x.Id == local.Id, local);
    }

    public async Task DeleteAsync(int id)
    {
        if (!(await ExistsAsync(id)))
        {
            throw new ArgumentException("local com esse id não encontrado.");
        }
        var id24Digit = FormatTo24DigitHex(id.ToString());
        await GetCollection().DeleteOneAsync(x => x.Id == id24Digit);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var id24Digit = FormatTo24DigitHex(id.ToString());
        var localExiste = await GetCollection().Find(x => x.Id == id24Digit).AnyAsync();
        return localExiste;
    }

    public async Task<Local?> SelectLocalSeJaCadastradoOrDefaultAsync(Local local)
    {
        var localCom24Digit = await GetCollection().Find(x => x.cep == local.cep
            && x.logradouro == local.logradouro
            && x.numero == local.numero
            && x.complemento == local.complemento
            && x.bairro == local.bairro
            && x.cidade == local.cidade
            && x.estado == local.estado).FirstOrDefaultAsync();
        if (localCom24Digit == null)
        {
            return null;
        }
        localCom24Digit.Id = FormatToStringifiedNumber(localCom24Digit.Id);
        return localCom24Digit;
    }
    
}