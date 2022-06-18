using DDD.Api.Business.Adapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.IntegratedAdapter.BusnModelAdapter;
public class SessaoBusnModelIntegratedAdapter : ISessaoBusnModelIntegratedAdapter
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAcessoRepository _acessoRepository;
    private readonly ISessaoRepository _sessaoRepository;
    public SessaoBusnModelIntegratedAdapter(IUsuarioRepository usuarioRepository, IAcessoRepository acessoRepository, ISessaoRepository sessaoRepository)
    {
        _usuarioRepository = usuarioRepository;
        _acessoRepository = acessoRepository;
        _sessaoRepository = sessaoRepository;
    }

    public async Task<SessaoBusnModel> ToNovaSessaoAutenticadaAsync(Sessao sessaoAdaptee)
    {
        var usuarioSessao = await _usuarioRepository.SelectByIdOrDefaultAsync(sessaoAdaptee.usuario_id.ParseZeroIfFails());
        var acessosUsuarioSessao = await _acessoRepository.SelectByUsuarioAsync(sessaoAdaptee.usuario_id.ParseZeroIfFails());
        SessaoBusnModel sessaoBusn = new SessaoBusnModelAdapter(sessaoAdaptee);
        sessaoBusn.UsuarioAutenticado = new UsuarioBusnModelAdapter(usuarioSessao) as UsuarioBusnModel;
        foreach (var acessoUsuarioSessao in acessosUsuarioSessao)
        {
            AcessoBusnModel acessoBusn = new AcessoBusnModelAdapter(acessoUsuarioSessao);
            sessaoBusn.UsuarioAutenticado.Acessos.Add(acessoBusn);
        }
        return sessaoBusn;
    }

    public async Task<SessaoBusnModel> ToSessaoAutenticadaJaExistenteAsync(SessaoEnvioHeaderDtoModel sessaoEnvioHeaderAdaptee)
    {
        var sessao = await _sessaoRepository.SelectByIdOrDefaultAsync(sessaoEnvioHeaderAdaptee.Id ?? 0);
        var usuarioSessao = await _usuarioRepository.SelectByIdOrDefaultAsync(sessao.usuario_id.ParseZeroIfFails());
        var acessosUsuarioSessao = await _acessoRepository.SelectByUsuarioAsync(sessao.usuario_id.ParseZeroIfFails());
        SessaoBusnModel sessaoBusn = new SessaoBusnModelAdapter(sessaoEnvioHeaderAdaptee);
        sessaoBusn.UsuarioAutenticado = new UsuarioBusnModelAdapter(usuarioSessao) as UsuarioBusnModel;
        foreach (var acessoUsuarioSessao in acessosUsuarioSessao)
        {
            AcessoBusnModel acessoBusn = new AcessoBusnModelAdapter(acessoUsuarioSessao);
            sessaoBusn.UsuarioAutenticado.Acessos.Add(acessoBusn);
        }
        return sessaoBusn;
    }
}