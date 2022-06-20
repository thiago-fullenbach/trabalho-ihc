using System.Net;
using System.Text.RegularExpressions;
using DDD.Api.Business.Adapter.RepoModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Business.Services;
using DDD.Api.Domain.Interface.Business.Services.DataServices;
using DDD.Api.Domain.Interface.Infra.Configuration.BackEndApp;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.BusnModel.Exception;
using DDD.Api.Domain.Models.Enumerations;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.Services;
public class PresencaService : IPresencaService
{
    private readonly ISessaoAutenticadaDataService _sessaoAutenticadaDataService;
    private readonly IPresencaRepository _presencaRepository;
    private readonly ILocalRepository _localRepository;
    private readonly IUnitOfWork _uow;
    private readonly IPresencaBusnModelIntegratedAdapter _presencaBusnModelIntegratedAdapter;
    public PresencaService(ISessaoAutenticadaDataService sessaoAutenticadaDataService,
                           IPresencaRepository presencaRepository,
                           ILocalRepository localRepository,
                           IUnitOfWork uow,
                           IPresencaBusnModelIntegratedAdapter presencaBusnModelIntegratedAdapter)
    {
        _sessaoAutenticadaDataService = sessaoAutenticadaDataService;
        _presencaRepository = presencaRepository;
        _localRepository = localRepository;
        _uow = uow;
        _presencaBusnModelIntegratedAdapter = presencaBusnModelIntegratedAdapter;
    }
    
    public async Task<bool> NovaPresencaEhEntradaAsync()
    {
        var usuarioLogado = _sessaoAutenticadaDataService.GetSessaoAutenticada().UsuarioAutenticado;
        return await _presencaRepository.NovaPresencaUsuarioEhEntradaAsync(usuarioLogado.Id);
    }

    public bool PodeVisualizarPresenca(PresencaBusnModel presenca)
    {
        var usuarioLogado = _sessaoAutenticadaDataService.GetSessaoAutenticada().UsuarioAutenticado;
        if (presenca.IdUsuarioPresente == usuarioLogado.Id)
        {
            return usuarioLogado.TemRecursoPermitido(EnRecurso.VisualizarPonto);
        }
        return usuarioLogado.TemRecursoPermitido(EnRecurso.VisualizarPontoDemaisUsuarios);
    }

    public async Task<List<PresencaBusnModel>> ListarTodasAsync()
    {
        var presencas = await _presencaRepository.SelectAllAsync();
        var presencasBusn = new List<PresencaBusnModel>();
        foreach (var presenca in presencas)
        {
            PresencaBusnModel presencaBusn = await _presencaBusnModelIntegratedAdapter.ToPresencaPesquisadaAsync(presenca);
            if (PodeVisualizarPresenca(presencaBusn))
            {
                presencasBusn.Add(presencaBusn);
            }
        }
        return presencasBusn;
    }

    private void ValidarNovaPresenca(PresencaBusnModel presenca)
    {
        var accErros = new List<string>();
        presenca.Local.CodigoCep = presenca.Local.CodigoCep.Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        if (string.IsNullOrWhiteSpace(presenca.Local.CodigoCep))
        {
            accErros.Add("Não foi informado CEP.");
        }
        else if (Regex.IsMatch(presenca.Local.CodigoCep, "[^0-9]+"))
        {
            accErros.Add("O CEP deve conter apenas dígitos.");
        }
        presenca.Local.NomeLogradouro = presenca.Local.NomeLogradouro.Trim();
        if (string.IsNullOrWhiteSpace(presenca.Local.NomeLogradouro))
        {
            accErros.Add("Não foi informada a Rua.");
        }
        presenca.Local.NumeroLogradouro = presenca.Local.NumeroLogradouro.Trim();
        if (!string.IsNullOrWhiteSpace(presenca.Local.NumeroLogradouro) && Regex.IsMatch(presenca.Local.NumeroLogradouro, "[^0-9]+"))
        {
            accErros.Add("O Número da rua aceita apenas dígitos.");
        }
        presenca.Local.ComplementoLogradouro = presenca.Local.ComplementoLogradouro.Trim();
        presenca.Local.NomeBairro = presenca.Local.NomeBairro.Trim();
        if (string.IsNullOrWhiteSpace(presenca.Local.NomeBairro))
        {
            accErros.Add("Não foi informado o Bairro.");
        }
        presenca.Local.NomeCidade = presenca.Local.NomeCidade.Trim();
        if (string.IsNullOrWhiteSpace(presenca.Local.NomeCidade))
        {
            accErros.Add("Não foi informada a Cidade.");
        }
        presenca.Local.NomeEstado = presenca.Local.NomeEstado.Trim();
        if (string.IsNullOrWhiteSpace(presenca.Local.NomeEstado))
        {
            accErros.Add("Não foi informado o Estado.");
        }
        if (accErros.Count > 0)
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                MensagensErro = accErros
            };
        }
    }

    public async Task CreateAsync(PresencaBusnModel presenca)
    {
        var usuarioLogado = _sessaoAutenticadaDataService.GetSessaoAutenticada().UsuarioAutenticado;
        if (!usuarioLogado.TemRecursoPermitido(EnRecurso.RegistrarPonto))
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                MensagensErro = new List<string> { "Acesso negado." }
            };
        }
        ValidarNovaPresenca(presenca);
        Local localRepositorio = new LocalAdapter(presenca.Local);
        var horaAtual = DateTime.Now;
        var transaction = await _uow.StartTransactionAsync();
        try
        {
            var idNovaPresenca = await _presencaRepository.SelectNextInsertIdAsync();
            var salvarPresenca = new Presenca
            {
                Id = idNovaPresenca.ToString(),
                usuario_id = usuarioLogado.Id.ToString(),
                eh_entrada = await NovaPresencaEhEntradaAsync(),
                datahora_presenca = horaAtual,
                tem_visto = false
            };
            var localCadastrado = await _localRepository.SelectLocalSeJaCadastradoOrDefaultAsync(localRepositorio);
            if (localCadastrado == null)
            {
                Local salvarLocal = new LocalAdapter(presenca.Local);
                var idNovoLocal = await _localRepository.SelectNextInsertIdAsync();
                salvarLocal.Id = idNovoLocal.ToString();
                await _localRepository.InsertAsync(idNovoLocal, salvarLocal);
                salvarPresenca.local_id = idNovoLocal.ToString();
            }
            else
            {
                salvarPresenca.local_id = localCadastrado.Id;
            }
            await _presencaRepository.InsertAsync(idNovaPresenca, salvarPresenca);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw ex;
        }
    }

    public async Task DarVistoAsync(int id)
    {
        var usuarioLogado = _sessaoAutenticadaDataService.GetSessaoAutenticada().UsuarioAutenticado;
        if (!usuarioLogado.TemRecursoPermitido(EnRecurso.VisualizarPontoDemaisUsuarios))
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                MensagensErro = new List<string> { "Acesso negado." }
            };
        }
        var transaction = await _uow.StartTransactionAsync();
        try
        {
            var presenca = await _presencaRepository.SelectByIdOrDefaultAsync(id);
            if (presenca == null)
            {
                throw new BusinessException
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    MensagensErro = new List<string> { "Presença não encontrada." }
                };
            }
            if (presenca.tem_visto ?? false)
            {
                await transaction.RollbackAsync();
                return;
            }
            presenca.tem_visto = true;
            await _presencaRepository.UpdateAsync(id, presenca);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw ex;
        }
    }
}