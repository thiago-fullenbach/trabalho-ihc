using System.Linq.Expressions;
using DDD.Api.Business;
using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Domain.Models.RepoModel;
using DDD.Api.Infra.Configuration.Database;
using MongoDB.Driver;

namespace DDD.Api.Infra.Repositories;
public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
{
    private readonly IAcessoRepository _acessoRepository;
    public UsuarioRepository(IUnitOfWork uow,
                             MongoDbConnection connection,
                             IDatabaseConfiguration databaseConfiguration,
                             IAcessoRepository acessoRepository)
        : base(uow, connection, databaseConfiguration)
    {
        NomeColecaoEntidade = databaseConfiguration.GetNomeColecaoUsuarios();
        _acessoRepository = acessoRepository;
    }

    public async Task<List<Usuario>> SelectAllAsync()
    {
        var usuarios = await GetCollection().Find(_ => true).ToListAsync();
        foreach (var usuario in usuarios)
        {
            usuario.Id = FormatToStringifiedNumber(usuario.Id);
        }
        return usuarios;
    }

    public async Task<Usuario?> SelectByIdOrDefaultAsync(int id)
    {
        var id24Digit = FormatTo24DigitHex(id.ToString());
        var usuario = await GetCollection().Find(x => x.Id == id24Digit).FirstOrDefaultAsync();
        if (usuario == null)
        {
            return null;
        }
        usuario.Id = id.ToString();
        return usuario;
    }

    public async Task<int> SelectNextInsertIdAsync()
    {
        var usuarios = await SelectAllAsync();
        int maxId = 0;
        foreach (var usuario in usuarios)
        {
            int id = usuario.Id.ParseZeroIfFails();
            if (id > maxId)
            {
                maxId = id;
            }
        }
        return maxId + 1;
    }

    public async Task InsertAsync(int insertId, Usuario usuario)
    {
        if (await ExistsAsync(insertId))
        {
            throw new ArgumentException("insertId já existe no banco de dados.");
        }
        usuario.Id = FormatTo24DigitHex(insertId.ToString());
        await GetCollection().InsertOneAsync(usuario);
    }

    public async Task UpdateAsync(int id, Usuario usuario)
    {
        if (!(await ExistsAsync(id)))
        {
            throw new ArgumentException("usuario com esse id não encontrado.");
        }
        usuario.Id = FormatTo24DigitHex(id.ToString());
        await GetCollection().ReplaceOneAsync(x => x.Id == usuario.Id, usuario);
    }

    public async Task DeleteAsync(int id)
    {
        if (!(await ExistsAsync(id)))
        {
            throw new ArgumentException("usuario com esse id não encontrado.");
        }
        var id24Digit = FormatTo24DigitHex(id.ToString());
        await GetCollection().DeleteOneAsync(x => x.Id == id24Digit);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var id24Digit = FormatTo24DigitHex(id.ToString());
        var usuarioExiste = await GetCollection().Find(x => x.Id == id24Digit).AnyAsync();
        return usuarioExiste;
    }

    private IMongoCollection<Acesso> GetCollectionAcessos()
    {
        var collection = _connection.Client.GetDatabase(_databaseConfiguration.GetNomeBancoDados())
            .GetCollection<Acesso>(_databaseConfiguration.GetNomeColecaoAcessos());
        return collection;
    }

    private IMongoCollection<Sessao> GetCollectionSessoes()
    {
        var collection = _connection.Client.GetDatabase(_databaseConfiguration.GetNomeBancoDados())
            .GetCollection<Sessao>(_databaseConfiguration.GetNomeColecaoSessoes());
        return collection;
    }

    public async Task InsertAcessosAsync(int idUsuario, List<Acesso> acessos)
    {
        if (acessos.Count() == 0)
        {
            return;
        }
        if (!(await ExistsAsync(idUsuario)))
        {
            throw new ArgumentException("Usuario com esse id não encontrado.");
        }
        foreach (var acesso in acessos)
        {
            var nextIdAcesso = await _acessoRepository.SelectNextInsertIdAsync();
            acesso.Id = FormatTo24DigitHex(nextIdAcesso.ToString());
            acesso.usuario_id = FormatTo24DigitHex(idUsuario.ToString());
            await _acessoRepository.InsertAsync(nextIdAcesso, acesso);
        }
    }
    
    public async Task UpdateAcessosAsync(int idUsuario, List<Acesso> acessos)
    {
        var idEm24Digit = FormatTo24DigitHex(idUsuario.ToString());
        await GetCollectionAcessos().DeleteManyAsync(x => x.usuario_id == idEm24Digit);
        await InsertAcessosAsync(idUsuario, acessos);
    }

    public async Task<Usuario?> SelectByLoginOrDefaultAsync(string login)
    {
        var usuario = await GetCollection().Find(x => x.login == login).FirstOrDefaultAsync();
        if (usuario == null)
        {
            return null;
        }
        usuario.Id = FormatToStringifiedNumber(usuario.Id);
        return usuario;
    }

    public async Task<Usuario?> SelectByCPFOrDefaultAsync(string cpf)
    {
        var usuario = await GetCollection().Find(x => x.cpf == cpf).FirstOrDefaultAsync();
        if (usuario == null)
        {
            return null;
        }
        usuario.Id = FormatToStringifiedNumber(usuario.Id);
        return usuario;
    }

    public async Task ExcluirSessoesAsync(int idUsuario)
    {
        var idEm24Digit = FormatTo24DigitHex(idUsuario.ToString());
        await GetCollectionSessoes().DeleteManyAsync(x => x.usuario_id == idEm24Digit);
    }
}