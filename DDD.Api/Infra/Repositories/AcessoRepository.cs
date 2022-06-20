using System.Linq.Expressions;
using DDD.Api.Business;
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
    public async Task<List<Acesso>> SelectAllAsync()
    {
        var acessos = await GetCollection().Find(_ => true).ToListAsync();
        foreach (var acesso in acessos)
        {
            acesso.Id = FormatToStringifiedNumber(acesso.Id);
            acesso.usuario_id = FormatToStringifiedNumber(acesso.usuario_id);
        }
        return acessos;
    }

    public async Task<Acesso?> SelectByIdOrDefaultAsync(int id)
    {
        var id24Digit = FormatTo24DigitHex(id.ToString());
        var acesso = await GetCollection().Find(x => x.Id == id24Digit).FirstOrDefaultAsync();
        if (acesso == null)
        {
            return null;
        }
        acesso.Id = FormatToStringifiedNumber(acesso.Id);
        acesso.usuario_id = FormatToStringifiedNumber(acesso.usuario_id);
        return acesso;
    }

    public async Task<int> SelectNextInsertIdAsync()
    {
        var acessos = await SelectAllAsync();
        int maxId = 0;
        foreach (var acesso in acessos)
        {
            int id = acesso.Id.ParseZeroIfFails();
            if (id > maxId)
            {
                maxId = id;
            }
        }
        return maxId + 1;
    }

    public async Task InsertAsync(int insertId, Acesso acesso)
    {
        if (await ExistsAsync(insertId))
        {
            throw new ArgumentException("insertId já existe no banco de dados.");
        }
        acesso.Id = FormatTo24DigitHex(insertId.ToString());
        acesso.usuario_id = FormatTo24DigitHex(acesso.usuario_id);
        await GetCollection().InsertOneAsync(acesso);
    }

    public async Task UpdateAsync(int id, Acesso acesso)
    {
        if (!(await ExistsAsync(id)))
        {
            throw new ArgumentException("acesso com esse id não encontrado.");
        }
        acesso.Id = FormatTo24DigitHex(id.ToString());
        acesso.usuario_id = FormatTo24DigitHex(acesso.usuario_id);
        await GetCollection().ReplaceOneAsync(x => x.Id == acesso.Id, acesso);
    }

    public async Task DeleteAsync(int id)
    {
        if (!(await ExistsAsync(id)))
        {
            throw new ArgumentException("acesso com esse id não encontrado.");
        }
        var id24Digit = FormatTo24DigitHex(id.ToString());
        await GetCollection().DeleteOneAsync(x => x.Id == id24Digit);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var id24Digit = FormatTo24DigitHex(id.ToString());
        var acessoExiste = await GetCollection().Find(x => x.Id == id24Digit).AnyAsync();
        return acessoExiste;
    }

    public async Task<List<Acesso>> SelectByUsuarioAsync(int idUsuario)
    {
        var idEm24Digit = FormatTo24DigitHex(idUsuario.ToString());
        var acessos = await GetCollection().Find(x => x.usuario_id == idEm24Digit).ToListAsync();
        return acessos;
    }
}