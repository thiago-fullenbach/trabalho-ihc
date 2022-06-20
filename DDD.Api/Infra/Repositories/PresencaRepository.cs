using System.Linq.Expressions;
using DDD.Api.Business;
using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Domain.Models.RepoModel;
using DDD.Api.Infra.Configuration.Database;
using MongoDB.Driver;

namespace DDD.Api.Infra.Repositories;
public class PresencaRepository : RepositoryBase<Presenca>, IPresencaRepository
{
    public PresencaRepository(IUnitOfWork uow, MongoDbConnection connection, IDatabaseConfiguration databaseConfiguration)
        : base(uow, connection, databaseConfiguration)
    {
        NomeColecaoEntidade = databaseConfiguration.GetNomeColecaoPresencas();
    }

    public async Task<List<Presenca>> SelectAllAsync()
    {
        var presencas = await GetCollection().Find(_ => true).ToListAsync();
        foreach (var presenca in presencas)
        {
            presenca.Id = FormatToStringifiedNumber(presenca.Id);
            presenca.usuario_id = FormatToStringifiedNumber(presenca.usuario_id);
            presenca.local_id = FormatToStringifiedNumber(presenca.local_id);
        }
        return presencas;
    }

    public async Task<Presenca?> SelectByIdOrDefaultAsync(int id)
    {
        var id24Digit = FormatTo24DigitHex(id.ToString());
        var presenca = await GetCollection().Find(x => x.Id == id24Digit).FirstOrDefaultAsync();
        if (presenca == null)
        {
            return null;
        }
        presenca.Id = FormatToStringifiedNumber(presenca.Id);
        presenca.usuario_id = FormatToStringifiedNumber(presenca.usuario_id);
        presenca.local_id = FormatToStringifiedNumber(presenca.local_id);
        return presenca;
    }

    public async Task<int> SelectNextInsertIdAsync()
    {
        var presencas = await SelectAllAsync();
        int maxId = 0;
        foreach (var presenca in presencas)
        {
            int id = presenca.Id.ParseZeroIfFails();
            if (id > maxId)
            {
                maxId = id;
            }
        }
        return maxId + 1;
    }

    public async Task InsertAsync(int insertId, Presenca presenca)
    {
        if (await ExistsAsync(insertId))
        {
            throw new ArgumentException("insertId já existe no banco de dados.");
        }
        presenca.Id = FormatTo24DigitHex(insertId.ToString());
        presenca.usuario_id = FormatTo24DigitHex(presenca.usuario_id);
        presenca.local_id = FormatTo24DigitHex(presenca.local_id);
        await GetCollection().InsertOneAsync(presenca);
    }

    public async Task UpdateAsync(int id, Presenca presenca)
    {
        if (!(await ExistsAsync(id)))
        {
            throw new ArgumentException("presenca com esse id não encontrado.");
        }
        presenca.Id = FormatTo24DigitHex(id.ToString());
        presenca.usuario_id = FormatTo24DigitHex(presenca.usuario_id);
        presenca.local_id = FormatTo24DigitHex(presenca.local_id);
        await GetCollection().ReplaceOneAsync(x => x.Id == presenca.Id, presenca);
    }

    public async Task DeleteAsync(int id)
    {
        if (!(await ExistsAsync(id)))
        {
            throw new ArgumentException("presenca com esse id não encontrado.");
        }
        var id24Digit = FormatTo24DigitHex(id.ToString());
        await GetCollection().DeleteOneAsync(x => x.Id == id24Digit);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var id24Digit = FormatTo24DigitHex(id.ToString());
        var presencaExiste = await GetCollection().Find(x => x.Id == id24Digit).AnyAsync();
        return presencaExiste;
    }

    public async Task<bool> NovaPresencaUsuarioEhEntradaAsync(int idUsuario)
    {
        var idEm24Digit = FormatTo24DigitHex(idUsuario.ToString());
        var presencasUsuario = await GetCollection().Find(x => x.usuario_id == idEm24Digit).ToListAsync();
        if (presencasUsuario.Count == 0)
        {
            return true;
        }
        var horaUltimaPresenca = presencasUsuario[0].datahora_presenca;
        var ultimaPresencaEhEntrada = (presencasUsuario[0].eh_entrada ?? false);
        foreach (var presenca in presencasUsuario)
        {
            if (presenca.datahora_presenca > horaUltimaPresenca)
            {
                horaUltimaPresenca = presenca.datahora_presenca;
            }
            ultimaPresencaEhEntrada = (presenca.eh_entrada ?? false);
        }
        return !ultimaPresencaEhEntrada;
    }
}