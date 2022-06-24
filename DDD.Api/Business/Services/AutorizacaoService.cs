using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Business.Services;
using DDD.Api.Domain.Interface.Business.Services.DataServices;
using DDD.Api.Domain.Interface.Business.Services.MicroService;
using DDD.Api.Domain.Interface.Business.Services.SchedulerService;
using DDD.Api.Domain.Interface.Infra.Configuration.BackEndApp;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.BusnModel.Exception;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.Services;
public class AutorizacaoService : IAutorizacaoService
{
    private readonly ISessaoAutenticadaDataService _sessaoAutenticadaDataService;
    private readonly IMicroService _microService;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ISessaoRepository _sessaoRepository;
    private readonly IAcessoRepository _acessoRepository;
    private readonly IBackEndAppConfiguration _backEndAppConfiguration;
    private readonly IUnitOfWork _uow;
    private readonly ISessaoBusnModelIntegratedAdapter _sessaoBusnModelIntegratedAdapter;
    private readonly ISchedulerService _schedulerService;
    public AutorizacaoService(ISessaoAutenticadaDataService sessaoAutenticadaDataService,
                              IMicroService microService,
                              IUsuarioRepository usuarioRepository,
                              ISessaoRepository sessaoRepository,
                              IAcessoRepository acessoRepository,
                              IBackEndAppConfiguration backEndAppConfiguration,
                              IUnitOfWork uow,
                              ISessaoBusnModelIntegratedAdapter sessaoBusnModelIntegratedAdapter,
                              ISchedulerService schedulerService)
    {
        _sessaoAutenticadaDataService = sessaoAutenticadaDataService;
        _microService = microService;
        _usuarioRepository = usuarioRepository;
        _sessaoRepository = sessaoRepository;
        _acessoRepository = acessoRepository;
        _backEndAppConfiguration = backEndAppConfiguration;
        _uow = uow;
        _sessaoBusnModelIntegratedAdapter = sessaoBusnModelIntegratedAdapter;
        _schedulerService = schedulerService;
    }
    public async Task LogarAsync(UsuarioBusnModel usuario)
    {
        var accErros = new List<string>();
        if (string.IsNullOrWhiteSpace(usuario.Login))
        {
            accErros.Add("Login obrigatório.");
        }
        if (string.IsNullOrWhiteSpace(usuario.Senha))
        {
            accErros.Add("Senha obrigatória.");
        }
        if (accErros.Count > 0)
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                MensagensErro = accErros
            };
        }
        var usuarioEncontrado = await _usuarioRepository.SelectByLoginOrDefaultAsync(usuario.Login);
        if (usuarioEncontrado == null)
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                MensagensErro = new List<string> { "Login ou senha inválidos." }
            };
        }
        string senhaCriptografada = await CriptografarSenhaByUrlAsync(usuarioEncontrado.url_hasheia_senha_sem_parametros, usuario.Senha);
        if (senhaCriptografada != usuarioEncontrado.senha)
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                MensagensErro = new List<string> { "Login ou senha inválidos." }
            };
        }
        var transaction = await _uow.StartTransactionAsync();
        try
        {
            var idNovaSessao = await _sessaoRepository.SelectNextInsertIdAsync();
            var novaSessao = new Sessao
            {
                Id =  idNovaSessao.ToString(),
                hex_verificacao = _sessaoRepository.NovoCodigoVerificacao(),
                usuario_id = usuarioEncontrado.Id,
                datahora_ultima_autenticacao = DateTime.Now
            };
            await _sessaoRepository.InsertAsync(idNovaSessao, novaSessao);
            if (!_backEndAppConfiguration.EhAmbienteDesenv())
            {
                var urlCriptoSenhaAppInterno = _backEndAppConfiguration.GetUrlHashearSenhaSemParametrosExpostaExternamente();
                if (usuarioEncontrado.url_hasheia_senha_sem_parametros != urlCriptoSenhaAppInterno)
                {
                    var senhaCriptAppInterno = CriptografarSenhaAppInterno(usuario.Senha);
                    usuarioEncontrado.senha = senhaCriptAppInterno;
                    usuarioEncontrado.url_hasheia_senha_sem_parametros = urlCriptoSenhaAppInterno;
                    await _usuarioRepository.UpdateAsync(usuarioEncontrado.Id.ParseZeroIfFails(), usuarioEncontrado);
                }
            }
            var acessosUsuarioEncontrado = await _acessoRepository.SelectByUsuarioAsync(usuarioEncontrado.Id.ParseZeroIfFails());
            var sessaoBusn = await _sessaoBusnModelIntegratedAdapter.ToNovaSessaoAutenticadaAsync(novaSessao);
            _sessaoAutenticadaDataService.SetSessaoAutenticada(sessaoBusn);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw ex;
        }
        await _schedulerService.ScheduleExcluirSessoesAsync();
    }

    public async Task ReautenticarSessaoAsync()
    {
        if (!_sessaoAutenticadaDataService.TemSessaoAutenticada())
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                MensagensErro = new List<string> { "Usuário não logado ou autenticação expirada." }
            };
        }
        var sessaoAutenticada = _sessaoAutenticadaDataService.GetSessaoAutenticada();
        var transaction = await _uow.StartTransactionAsync();
        try
        {
            var sessaoRepositorio = await _sessaoRepository.SelectByIdOrDefaultAsync(sessaoAutenticada.Id);
            var dateTimeNow = DateTime.Now;
            if (sessaoRepositorio == null || sessaoRepositorio.hex_verificacao != sessaoAutenticada.CodigoVerificacao || dateTimeNow > sessaoRepositorio.datahora_ultima_autenticacao + _backEndAppConfiguration.GetTempoExpirarSessao())
            {
                throw new BusinessException
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    MensagensErro = new List<string> { "Usuário não logado ou autenticação expirada." }
                };
            }
            sessaoRepositorio.datahora_ultima_autenticacao = dateTimeNow;
            await _sessaoRepository.UpdateAsync(sessaoAutenticada.Id, sessaoRepositorio);
            var sessaoBusn = await _sessaoBusnModelIntegratedAdapter.ToNovaSessaoAutenticadaAsync(sessaoRepositorio);
            _sessaoAutenticadaDataService.SetSessaoAutenticada(sessaoBusn);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw ex;
        }
        await _schedulerService.ScheduleExcluirSessoesAsync();
    }

    public string CriptografarSenhaAppInterno(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                MensagensErro = new List<string> { "Senha obrigatória." }
            };
        }
        var hmacSha512 = new HMACSHA512(Encoding.UTF8.GetBytes(_backEndAppConfiguration.GetSegredo()));
        var bytesSenha = Encoding.UTF8.GetBytes(senha);
        var hash = hmacSha512.ComputeHash(bytesSenha);
        return BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
    }

    public async Task<string> CriptografarSenhaAppExternoAsync(string urlEndpointCriptografarSenha, string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                MensagensErro = new List<string> { "Senha obrigatória." }
            };
        }
        var urlCompleta = $"{urlEndpointCriptografarSenha}?senhaRaw={HttpUtility.UrlEncode(senha)}";
        var senhaCriptografada = await _microService.GetNaoAutenticadoAsync<string>(urlCompleta);
        return senhaCriptografada ?? string.Empty;
    }

    public async Task<string> CriptografarSenhaByUrlAsync(string urlEndpointCriptografarSenha, string senha)
    {
        string senhaCriptografada = string.Empty;
        if (urlEndpointCriptografarSenha == _backEndAppConfiguration.GetUrlHashearSenhaSemParametrosExpostaExternamente())
        {
            senhaCriptografada = CriptografarSenhaAppInterno(senha);
        }
        else
        {
            senhaCriptografada = await CriptografarSenhaAppExternoAsync(urlEndpointCriptografarSenha, senha);
        }
        return senhaCriptografada;
    }

    public async Task<DateTime?> ObterExpiracaoMaisRecenteSessaoAsync()
    {
        var todasSessoes = await _sessaoRepository.SelectAllAsync();
        if (todasSessoes.Count == 0)
        {
            return null;
        }
        var dataMaisRecente = todasSessoes[0].datahora_ultima_autenticacao + _backEndAppConfiguration.GetTempoExpirarSessao();
        foreach (var sessao in todasSessoes)
        {
            if (dataMaisRecente < sessao.datahora_ultima_autenticacao + _backEndAppConfiguration.GetTempoExpirarSessao())
            {
                dataMaisRecente = sessao.datahora_ultima_autenticacao + _backEndAppConfiguration.GetTempoExpirarSessao();
            }
        }
        return dataMaisRecente;
    }

    public async Task ExcluirSessoesExpiradasAsync()
    {
        var todasSessoes = await _sessaoRepository.SelectAllAsync();
        var transaction = await _uow.StartTransactionAsync();
        try
        {
            var dateTimeNow = DateTime.Now;
            foreach (var sessao in todasSessoes)
            {
                if (dateTimeNow > sessao.datahora_ultima_autenticacao + _backEndAppConfiguration.GetTempoExpirarSessao())
                {
                    await _sessaoRepository.DeleteAsync(int.Parse(sessao.Id));
                }
            }
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw ex;
        }
        await _schedulerService.ScheduleExcluirSessoesAsync();
    }
}