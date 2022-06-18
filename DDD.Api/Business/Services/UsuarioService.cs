using System.Net;
using System.Text.RegularExpressions;
using DDD.Api.Business.Adapter.BusnModelAdapter;
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
public class UsuarioService : IUsuarioService
{
    private readonly ISessaoAutenticadaDataService _sessaoAutenticadaDataService;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IBackEndAppConfiguration _backEndAppConfiguration;
    private readonly IUnitOfWork _uow;
    private readonly IUsuarioBusnModelIntegratedAdapter _usuarioBusnModelIntegratedAdapter;
    private readonly IAutorizacaoService _autorizacaoService;
    public UsuarioService(ISessaoAutenticadaDataService sessaoAutenticadaDataService,
                          IUsuarioRepository usuarioRepository,
                          IBackEndAppConfiguration backEndAppConfiguration,
                          IUnitOfWork uow,
                          IUsuarioBusnModelIntegratedAdapter usuarioBusnModelIntegratedAdapter,
                          IAutorizacaoService autorizacaoService)
    {
        _sessaoAutenticadaDataService = sessaoAutenticadaDataService;
        _usuarioRepository = usuarioRepository;
        _backEndAppConfiguration = backEndAppConfiguration;
        _uow = uow;
        _usuarioBusnModelIntegratedAdapter = usuarioBusnModelIntegratedAdapter;
        _autorizacaoService = autorizacaoService;
    }

    public async Task<int> GetIdUsuarioImportarExportarAsync()
    {
        var loginUsuarioImportarExportar = _backEndAppConfiguration.GetNovoUsuarioDtoModelImportarExportar().Login;
        var usuarioImportarExportar = await _usuarioRepository.SelectByLoginOrDefaultAsync(loginUsuarioImportarExportar);
        if (usuarioImportarExportar == null)
        {
            return 0;
        }
        return usuarioImportarExportar.Id.ParseZeroIfFails();
    }

    public async Task<int> GetIdUsuarioAdminRootAsync()
    {
        var loginUsuarioAdminRoot = _backEndAppConfiguration.GetNovoUsuarioDtoModelAdminRoot().Login;
        var usuarioAdminRoot = await _usuarioRepository.SelectByLoginOrDefaultAsync(loginUsuarioAdminRoot);
        if (usuarioAdminRoot == null)
        {
            return 0;
        }
        return usuarioAdminRoot.Id.ParseZeroIfFails();
    }

    public bool PodeVisualizarUsuario(UsuarioBusnModel usuario, int idUsuarioImportarExportar)
    {
        if (usuario.Id == idUsuarioImportarExportar)
        {
            return false;
        }
        var usuarioLogado = _sessaoAutenticadaDataService.GetSessaoAutenticada().UsuarioAutenticado;
        if (usuario.Id == usuarioLogado.Id)
        {
            return usuarioLogado.TemRecursoPermitido(EnRecurso.VisualizarUsuario);
        }
        return usuarioLogado.TemRecursoPermitido(EnRecurso.VisualizarDemaisUsuarios);
    }

    public bool PodeCadastrarUsuario(UsuarioBusnModel usuario, int idUsuarioImportarExportar, int idUsuarioAdminRoot)
    {
        if (usuario.Id == idUsuarioImportarExportar)
        {
            return false;
        }
        var usuarioLogado = _sessaoAutenticadaDataService.GetSessaoAutenticada().UsuarioAutenticado;
        if (usuario.Id == usuarioLogado.Id)
        {
            return usuarioLogado.TemRecursoPermitido(EnRecurso.CadastrarUsuario);
        }
        return usuarioLogado.TemRecursoPermitido(EnRecurso.CadastrarDemaisUsuarios) && usuario.Id != idUsuarioAdminRoot;
    }

    public bool PodeExcluirUsuario(UsuarioBusnModel usuario, int idUsuarioImportarExportar, int idUsuarioAdminRoot)
    {
        var usuarioLogado = _sessaoAutenticadaDataService.GetSessaoAutenticada().UsuarioAutenticado;
        return PodeCadastrarUsuario(usuario, idUsuarioImportarExportar, idUsuarioAdminRoot) && usuario.Id != usuarioLogado.Id;
    }

    public async Task<List<UsuarioBusnModel>> ListarTodosAsync()
    {
        var todosUsuarios = await _usuarioRepository.SelectAllAsync();
        var todosUsuariosBusnModel = new List<UsuarioBusnModel>();
        foreach (var usuario in todosUsuarios)
        {
            UsuarioBusnModel usuarioBusnModel = new UsuarioBusnModelAdapter(usuario);
            todosUsuariosBusnModel.Add(usuarioBusnModel);
        }
        int idUsuarioImportarExportar = await GetIdUsuarioImportarExportarAsync();
        var usuariosPodeVer = new List<UsuarioBusnModel>();
        foreach (var usuario in todosUsuariosBusnModel)
        {
            if (PodeVisualizarUsuario(usuario, idUsuarioImportarExportar))
            {
                usuariosPodeVer.Add(usuario);
            }
        }
        return usuariosPodeVer;
    }

    public async Task<UsuarioBusnModel> GetByIdAsync(int id)
    {
        var usuarioEncontrado = await _usuarioRepository.SelectByIdOrDefaultAsync(id);
        if (usuarioEncontrado == null)
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                MensagensErro = new List<string> { "Usuário não encontrado." }
            };
        }
        var usuarioBusnModel = await _usuarioBusnModelIntegratedAdapter.ToUsuarioDetalhadoAsync(usuarioEncontrado);
        if (!PodeVisualizarUsuario(usuarioBusnModel, await GetIdUsuarioImportarExportarAsync()))
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                MensagensErro = new List<string> { "Usuário não encontrado." }
            };
        }
        return usuarioBusnModel;
    }

    private async Task ValidarNovoUsuarioAsync(UsuarioBusnModel usuario)
    {
        var accErros = new List<string>();
        usuario.Nome = usuario.Nome.ToUpper().Trim().RemoverAcentos();
        if (string.IsNullOrWhiteSpace(usuario.Nome))
        {
            accErros.Add("Nome obrigatório.");
        }
        else if (usuario.Nome.Length < 2)
        {
            accErros.Add("Nome deve conter pelo menos 2 caracteres.");
        }
        usuario.CPF = usuario.CPF.Replace(".", string.Empty).Replace("-", string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(usuario.CPF))
        {
            accErros.Add("CPF obrigatório.");
        }
        else if (Regex.IsMatch(usuario.CPF, "[^0-9]+") || usuario.CPF.Length != 11 || !usuario.CPF.CpfObedeceRegraResto11())
        {
            accErros.Add("CPF inválido.");
        }
        else
        {
            var usuarioMesmoCpf = await _usuarioRepository.SelectByCPFOrDefaultAsync(usuario.CPF);
            if (usuarioMesmoCpf != null)
            {
                accErros.Add("CPF não pode ser o mesmo de outro cadastro.");
            }
        }
        var dataHj = DateTime.Today;
        var data160AnosAtras = new DateTime(dataHj.Year - 160, dataHj.Month, dataHj.Day);
        if (usuario.DataNascimento == default(DateTime))
        {
            accErros.Add("Data de nascimento obrigatória.");
        }
        else if (usuario.DataNascimento > dataHj || usuario.DataNascimento < data160AnosAtras)
        {
            accErros.Add("Data de nascimento inválida");
        }
        if (usuario.HorasDiarias == default(int))
        {
            accErros.Add("Horas diárias obrigatória.");
        }
        else if (usuario.HorasDiarias < 2 || usuario.HorasDiarias > 12)
        {
            accErros.Add("Horas diárias inválida.");
        }
        usuario.Login = usuario.Login.Trim();
        if (string.IsNullOrWhiteSpace(usuario.Login))
        {
            accErros.Add("Login obrigatório.");
        }
        else if (Regex.IsMatch(usuario.Login, "[^a-zA-Z0-9._]+") || usuario.Login.Length < 6 || usuario.Login.Length > 12)
        {
            accErros.Add("Login deve ter entre 6 e 12 caracteres, e só aceita letras sem acento, dígitos, ponto (.) e underline (_).");
        }
        else
        {
            var usuarioMesmoLogin = await _usuarioRepository.SelectByLoginOrDefaultAsync(usuario.Login);
            if (usuarioMesmoLogin != null)
            {
                accErros.Add("Login não pode ser o mesmo de outro cadastro.");
            }
        }
        if (string.IsNullOrWhiteSpace(usuario.Senha))
        {
            accErros.Add("Senha obrigatória.");
        }
        else if (Regex.IsMatch(usuario.Senha, "[^a-zA-Z0-9]+") || usuario.Senha.Length < 6 || usuario.Login.Length > 12)
        {
            accErros.Add("Senha deve ter entre 6 e 12 caracteres, e só aceita letras sem acento e dígitos.");
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

    public async Task<UsuarioBusnModel> CreateAsync(UsuarioBusnModel usuario)
    {
        var usuarioLogado = _sessaoAutenticadaDataService.GetSessaoAutenticada().UsuarioAutenticado;
        if (!usuarioLogado.TemRecursoPermitido(EnRecurso.CadastrarDemaisUsuarios))
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
            await ValidarNovoUsuarioAsync(usuario);
            usuario.Id = await _usuarioRepository.SelectNextInsertIdAsync();
            var senhaCriptografada = _autorizacaoService.CriptografarSenhaAppInterno(usuario.Senha);
            usuario.Senha = senhaCriptografada;
            usuario.UrlEndpointHashearSenha = _backEndAppConfiguration.GetUrlHashearSenhaSemParametrosExpostaExternamente();
            Usuario usuarioRepositorio = new UsuarioAdapter(usuario);
            await _usuarioRepository.InsertAsync(usuario.Id, usuarioRepositorio);
            if (!usuarioLogado.TemRecursoPermitido(EnRecurso.CadastrarAcessoTodosUsuarios))
            {
                usuario.Acessos = AcessoBusnModel.GetAcessosPadrao();
            }
            var acessosRepositorio = new List<Acesso>();
            foreach (var acesso in usuario.Acessos)
            {
                acesso.IdUsuario = usuario.Id;
                Acesso acessoRepositorio = new AcessoAdapter(acesso);
                acessosRepositorio.Add(acessoRepositorio);
            }
            await _usuarioRepository.InsertAcessosAsync(usuario.Id, acessosRepositorio);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw ex;
        }
        return usuario;
    }

    private async Task ValidarAlterarUsuarioAsync(UsuarioBusnModel usuario)
    {
        var usuarioRepositorio = await _usuarioRepository.SelectByIdOrDefaultAsync(usuario.Id);
        if (usuarioRepositorio == null)
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                MensagensErro = new List<string> { "Usuário não encontrado." }
            };
        }
        var accErros = new List<string>();
        usuario.Nome = usuario.Nome.ToUpper().Trim().RemoverAcentos();
        if (string.IsNullOrWhiteSpace(usuario.Nome))
        {
            accErros.Add("Nome obrigatório.");
        }
        else if (usuario.Nome.Length < 2)
        {
            accErros.Add("Nome deve conter pelo menos 2 caracteres.");
        }
        usuario.CPF = usuario.CPF.Replace(".", string.Empty).Replace("-", string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(usuario.CPF))
        {
            accErros.Add("CPF obrigatório.");
        }
        else if (Regex.IsMatch(usuario.CPF, "[^0-9]+") || usuario.CPF.Length != 11 || !usuario.CPF.CpfObedeceRegraResto11())
        {
            accErros.Add("CPF inválido.");
        }
        else
        {
            var usuarioMesmoCpf = await _usuarioRepository.SelectByCPFOrDefaultAsync(usuario.CPF);
            if (usuarioMesmoCpf != null && usuarioRepositorio.cpf != usuario.CPF)
            {
                accErros.Add("CPF não pode ser o mesmo de outro cadastro.");
            }
        }
        var dataHj = DateTime.Today;
        var data160AnosAtras = new DateTime(dataHj.Year - 160, dataHj.Month, dataHj.Day);
        if (usuario.DataNascimento == default(DateTime))
        {
            accErros.Add("Data de nascimento obrigatória.");
        }
        else if ((usuario.DataNascimento > dataHj && usuarioRepositorio.data_nascimento != usuario.DataNascimento) || usuario.DataNascimento < data160AnosAtras)
        {
            accErros.Add("Data de nascimento inválida");
        }
        if (usuario.HorasDiarias == default(int))
        {
            accErros.Add("Horas diárias obrigatória.");
        }
        else if (usuario.HorasDiarias < 2 || usuario.HorasDiarias > 12)
        {
            accErros.Add("Horas diárias inválida.");
        }
        var usuarioLogado = _sessaoAutenticadaDataService.GetSessaoAutenticada().UsuarioAutenticado;
        if (usuarioLogado.Id != usuario.Id && !usuarioLogado.TemRecursoPermitido(EnRecurso.CadastrarAcessoTodosUsuarios))
        {
            usuario.Login = usuarioRepositorio.login;
            usuario.Senha = string.Empty;
        }
        usuario.Login = usuario.Login.Trim();
        if (string.IsNullOrWhiteSpace(usuario.Login))
        {
            accErros.Add("Login obrigatório.");
        }
        else if (Regex.IsMatch(usuario.Login, "[^a-zA-Z0-9._]+") || usuario.Login.Length < 6 || usuario.Login.Length > 12)
        {
            accErros.Add("Login deve ter entre 6 e 12 caracteres, e só aceita letras sem acento, dígitos, ponto (.) e underline (_).");
        }
        else
        {
            var usuarioMesmoLogin = await _usuarioRepository.SelectByLoginOrDefaultAsync(usuario.Login);
            if (usuarioMesmoLogin != null && usuarioRepositorio.login != usuario.Login)
            {
                accErros.Add("Login não pode ser o mesmo de outro cadastro.");
            }
        }
        if (usuario.EhAlteracaoSenha() && (Regex.IsMatch(usuario.Login, "[^a-zA-Z0-9._]+") || usuario.Login.Length < 6 || usuario.Login.Length > 12))
        {
            accErros.Add("Senha deve ter entre 6 e 12 caracteres, e só aceita letras sem acento e dígitos.");
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

    public async Task<UsuarioBusnModel> UpdateAsync(UsuarioBusnModel usuario)
    {
        var usuarioLogado = _sessaoAutenticadaDataService.GetSessaoAutenticada().UsuarioAutenticado;
        if (!PodeCadastrarUsuario(usuario, await GetIdUsuarioImportarExportarAsync(), await GetIdUsuarioAdminRootAsync()))
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
            await ValidarAlterarUsuarioAsync(usuario);
            var usuarioRepositorio = await _usuarioRepository.SelectByIdOrDefaultAsync(usuario.Id);
            if (usuario.EhAlteracaoSenha())
            {
                var senhaCriptografada = _autorizacaoService.CriptografarSenhaAppInterno(usuario.Senha);
                usuario.Senha = senhaCriptografada;
                usuario.UrlEndpointHashearSenha = _backEndAppConfiguration.GetUrlHashearSenhaSemParametrosExpostaExternamente();
            }
            else
            {
                usuario.Senha = usuarioRepositorio.senha;
                usuario.UrlEndpointHashearSenha = usuarioRepositorio.url_hasheia_senha_sem_parametros;
            }
            Usuario usuarioSalvar = new UsuarioAdapter(usuario);
            await _usuarioRepository.UpdateAsync(usuario.Id, usuarioSalvar);
            if (usuarioLogado.TemRecursoPermitido(EnRecurso.CadastrarAcessoTodosUsuarios))
            {
                var acessosRepositorio = new List<Acesso>();
                foreach (var acesso in usuario.Acessos)
                {
                    acesso.IdUsuario = usuario.Id;
                    Acesso acessoRepositorio = new AcessoAdapter(acesso);
                    acessosRepositorio.Add(acessoRepositorio);
                }
                await _usuarioRepository.UpdateAcessosAsync(usuario.Id, acessosRepositorio);
            }
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw ex;
        }
        return usuario;
    }

    public async Task RemoveAsync(int id)
    {
        var transaction = await _uow.StartTransactionAsync();
        try
        {
            var usuario = await _usuarioRepository.SelectByIdOrDefaultAsync(id);
            if (usuario == null)
            {
                throw new BusinessException
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    MensagensErro = new List<string> { "Usuário não encontrado." }
                };
            }
            UsuarioBusnModel usuarioBusnModel = new UsuarioBusnModelAdapter(usuario);
            if (!PodeExcluirUsuario(usuarioBusnModel, await GetIdUsuarioImportarExportarAsync(), await GetIdUsuarioAdminRootAsync()))
            {
                throw new BusinessException
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    MensagensErro = new List<string> { "Acesso negado." }
                };
            }
            await _usuarioRepository.ExcluirSessoesAsync(id);
            await _usuarioRepository.UpdateAcessosAsync(id, new List<Acesso>());
            await _usuarioRepository.DeleteAsync(id);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw ex;
        }
    }

    public async Task CarregarUsuariosAdminRootEImportarExportarAsync()
    {
        var novoAdminRootDto = _backEndAppConfiguration.GetNovoUsuarioDtoModelAdminRoot();
        var adminRootBusn = _usuarioBusnModelIntegratedAdapter.ToNovoUsuario(novoAdminRootDto);
        var novoImportarExportarDto = _backEndAppConfiguration.GetNovoUsuarioDtoModelImportarExportar();
        var importarExportarBusn = _usuarioBusnModelIntegratedAdapter.ToNovoUsuario(novoImportarExportarDto);
        var transaction = await _uow.StartTransactionAsync();
        try
        {
            await CarregaUsuarioAsync(adminRootBusn);
            await CarregaUsuarioAsync(importarExportarBusn);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw ex;
        }
    }

    private async Task CarregaUsuarioAsync(UsuarioBusnModel usuario)
    {
        usuario.Id = await _usuarioRepository.SelectNextInsertIdAsync();
        var senhaCriptografada = _autorizacaoService.CriptografarSenhaAppInterno(usuario.Senha);
        usuario.Senha = senhaCriptografada;
        usuario.UrlEndpointHashearSenha = _backEndAppConfiguration.GetUrlHashearSenhaSemParametrosExpostaExternamente();
        Usuario usuarioRepositorio = new UsuarioAdapter(usuario);
        await _usuarioRepository.InsertAsync(usuario.Id, usuarioRepositorio);
        var acessosRepositorio = new List<Acesso>();
        foreach (var acesso in usuario.Acessos)
        {
            acesso.IdUsuario = usuario.Id;
            Acesso acessoRepositorio = new AcessoAdapter(acesso);
            acessosRepositorio.Add(acessoRepositorio);
        }
        await _usuarioRepository.UpdateAcessosAsync(usuario.Id, acessosRepositorio);
    }
}