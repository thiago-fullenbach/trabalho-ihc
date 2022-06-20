using DDD.Api.Business.Adapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.IntegratedAdapter.BusnModelAdapter;
public class PresencaBusnModelIntegratedAdapter : IPresencaBusnModelIntegratedAdapter
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILocalRepository _localRepository;
    public PresencaBusnModelIntegratedAdapter(IUsuarioRepository usuarioRepository, ILocalRepository localRepository)
    {
        _usuarioRepository = usuarioRepository;
        _localRepository = localRepository;
    }

    public async Task<PresencaBusnModel> ToPresencaPesquisadaAsync(Presenca presencaAdaptee)
    {
        PresencaBusnModel presencaBusn = new PresencaBusnModelAdapter(presencaAdaptee);
        var usuarioPresenca = await _usuarioRepository.SelectByIdOrDefaultAsync(presencaBusn.IdUsuarioPresente);
        presencaBusn.UsuarioPresente = new UsuarioBusnModelAdapter(usuarioPresenca);
        var localPresenca = await _localRepository.SelectByIdOrDefaultAsync(presencaBusn.IdLocal);
        presencaBusn.Local = new LocalBusnModelAdapter(localPresenca);
        return presencaBusn;
    }

    public PresencaBusnModel ToNovaPresenca(NovaPresencaDtoModel novaPresencaDtoAdaptee)
    {
        PresencaBusnModel presencaBusn = new PresencaBusnModelAdapter(novaPresencaDtoAdaptee);
        presencaBusn.Local = new LocalBusnModelAdapter(novaPresencaDtoAdaptee.Local);
        return presencaBusn;
    }
}