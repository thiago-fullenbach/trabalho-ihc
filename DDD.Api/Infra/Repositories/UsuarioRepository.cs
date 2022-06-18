using System.Linq.Expressions;
using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Domain.Models.RepoModel;
using DDD.Api.Infra.Configuration.Database;
using MongoDB.Driver;

namespace DDD.Api.Infra.Repositories;
public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(IUnitOfWork uow, MongoDbConnection connection, IDatabaseConfiguration databaseConfiguration)
        : base(uow, connection, databaseConfiguration)
    {
        NomeColecaoEntidade = databaseConfiguration.GetNomeColecaoUsuarios();
    }

    protected override Expression<Func<Usuario, bool>> MontarCallbackFindById(int id)
    {
        var idEm24Digit = FormatTo24DigitHex(id.ToString());
        return x => x.Id == idEm24Digit;
    }

    protected override string GetId(Usuario usuario)
    {
        return usuario.Id;
    }

    protected override void SetId(Usuario usuario, string id)
    {
        usuario.Id = id;
    }

    private IMongoCollection<Acesso> GetCollectionAcessos()
    {
        var collection = _connection.Client.GetDatabase(_databaseConfiguration.GetNomeBancoDados())
            .GetCollection<Acesso>(_databaseConfiguration.GetNomeColecaoAcessos());
        return collection;
    }

    public async Task InsertAcessosAsync(int idUsuario, List<Acesso> acessos)
    {
        if (!(await ExistsAsync(idUsuario)))
        {
            throw new ArgumentException("Usuario com esse id não encontrado.");
        }
        foreach (var acesso in acessos)
        {
            acesso.usuario_id = FormatTo24DigitHex(idUsuario.ToString());
        }
        await GetCollectionAcessos().InsertManyAsync(acessos);

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

}