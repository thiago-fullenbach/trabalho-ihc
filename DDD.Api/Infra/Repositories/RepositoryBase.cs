using System.Linq.Expressions;
using System.Reflection;
using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Infra.Configuration.Database;
using MongoDB.Driver;

namespace DDD.Api.Infra.Repositories;
public class RepositoryBase<T> : IRepositoryBase<T> where T : new()
{
    protected readonly IUnitOfWork _uow;
    protected readonly MongoDbConnection _connection;
    protected readonly IDatabaseConfiguration _databaseConfiguration;
    public RepositoryBase(IUnitOfWork uow, MongoDbConnection connection, IDatabaseConfiguration databaseConfiguration)
    {
        _uow = uow;
        _connection = connection;
        _databaseConfiguration = databaseConfiguration;
    }
    protected string? NomeColecaoEntidade { get; set; }

    protected IMongoCollection<T> GetCollection()
    {
        var collection = _connection.Client.GetDatabase(_databaseConfiguration.GetNomeBancoDados())
            .GetCollection<T>(NomeColecaoEntidade);
        return collection;
    }

    protected string FormatTo24DigitHex(string id)
    {
        return id.PadLeft(24, '0');
    }

    protected string FormatToStringifiedNumber(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return string.Empty;
        }
        int contagemZeros = 0;
        for (; id[contagemZeros] != '0'; contagemZeros++)
        {
        }
        if (contagemZeros == 0)
        {
            return id;
        }
        else
        {
            return id.Substring(contagemZeros);
        }
    }

    protected virtual Expression<Func<T, bool>> MontarCallbackFindById(int id)
    {
        throw new NotImplementedException();
    }

    protected virtual string GetId(T entidade)
    {
        throw new NotImplementedException();
    }

    protected virtual void SetId(T entidade, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<T>> SelectAllAsync()
    {
        var entidades = await GetCollection().Find(_ => true).ToListAsync();
        foreach (var entidade in entidades)
        {
            SetId(entidade, FormatToStringifiedNumber(GetId(entidade)));
        }
        return entidades;
    }

    public async Task<T?> SelectByIdOrDefaultAsync(int id)
    {
        var entidade = await GetCollection().Find(MontarCallbackFindById(id)).FirstOrDefaultAsync();
        if (entidade == null)
        {
            return default(T);
        }
        SetId(entidade, FormatToStringifiedNumber(GetId(entidade)));
        return entidade;
    }

    public async Task<int> SelectNextInsertIdAsync()
    {
        var entidades = await SelectAllAsync();
        int maxId = 0;
        foreach (var entidade in entidades)
        {
            int id = int.Parse(GetId(entidade));
            if (id > maxId)
            {
                maxId = id;
            }
        }
        return maxId + 1;
    }

    public async Task InsertAsync(int insertId, T entidade)
    {
        if (await ExistsAsync(insertId))
        {
            throw new ArgumentException("insertId já existe no banco de dados.");
        }
        SetId(entidade, FormatTo24DigitHex(insertId.ToString()));
        await GetCollection().InsertOneAsync(entidade);
    }

    public async Task UpdateAsync(int id, T entidade)
    {
        if (!(await ExistsAsync(id)))
        {
            throw new ArgumentException("sessao com esse id não encontrado.");
        }
        SetId(entidade, FormatTo24DigitHex(id.ToString()));
        await GetCollection().ReplaceOneAsync(MontarCallbackFindById(id), entidade);
    }

    public async Task DeleteAsync(int id)
    {
        if (!(await ExistsAsync(id)))
        {
            throw new ArgumentException("sessao com esse id não encontrado.");
        }
        await GetCollection().DeleteOneAsync(MontarCallbackFindById(id));
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var entidadeExiste = await GetCollection().Find(MontarCallbackFindById(id)).AnyAsync();
        return entidadeExiste;
    }
}