using DDD.Api.Business.Adapter.DtoModelAdapter;
using DDD.Api.Domain.Interface.Infra.Configuration.BackEndApp;
using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;
using DDD.Api.Infra.Configuration.BackEndApp.BackEndAmbienteImpl;

namespace DDD.Api.Infra.Configuration.BackEndApp;
public class BackEndAppConfiguration : IBackEndAppConfiguration
{
    private readonly IConfiguration _configuration;
    public BackEndAppConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetSegredo()
    {
        return _configuration["SEGREDO_HASH_HMACSHA512"];
    }

    public IBackEndAmbiente GetBackEndAmbiente()
    {
        if (EhAmbienteDesenv())
        {
            return new BackEndDesenv(_configuration);
        }
        else
        {
            return new BackEndProd();
        }
    }

    public string GetUrlDominioExpostaExternamente()
    {
        var ambiente = GetBackEndAmbiente();
        return ambiente.GetUrlDominioExpostaExternamente();
    }

    public string GetUrlHashearSenhaSemParametrosExpostaExternamente()
    {
        return $"{GetUrlDominioExpostaExternamente()}/api/v1/Autorizacao/hasheiaSenha";
    }

    public NovoUsuarioDtoModel GetNovoUsuarioDtoModelAdminRoot()
    {
        var adminRoot = new NovoUsuarioDtoModel
        {
            Nome = _configuration["UsuarioAdminRaiz:Nome"],
            CPF = _configuration["UsuarioAdminRaiz:CPF"],
            Data_nascimento = DateTime.ParseExact(_configuration["UsuarioAdminRaiz:DataNascimento"], format: "yyyy-MM-dd", null),
            Horas_diarias = _configuration.GetValue<int>("UsuarioAdminRaiz:HorasDiarias"),
            Login = _configuration["UsuarioAdminRaiz:Login"],
            Nova_senha = _configuration["SENHA_ADMINISTRADOR_RAIZ"],
            Acessos = new List<AcessoUsuarioDtoModel>()
        };
        var acessosBusnModel = AcessoBusnModel.GetAcessosAdmin();
        foreach (var acessoBusn in acessosBusnModel)
        {
            AcessoUsuarioDtoModel acessoUsuarioDto = new AcessoUsuarioDtoModelAdapter(acessoBusn);
            adminRoot.Acessos.Add(acessoUsuarioDto);
        }
        return adminRoot;
    }

    public NovoUsuarioDtoModel GetNovoUsuarioDtoModelImportarExportar()
    {
        var usuarioImportarExportar = new NovoUsuarioDtoModel
        {
            Nome = _configuration["UsuarioImportarExportar:Nome"],
            CPF = _configuration["UsuarioImportarExportar:CPF"],
            Data_nascimento = DateTime.ParseExact(_configuration["UsuarioImportarExportar:DataNascimento"], format: "yyyy-MM-dd", null),
            Horas_diarias = _configuration.GetValue<int>("UsuarioImportarExportar:HorasDiarias"),
            Login = _configuration["UsuarioImportarExportar:Login"],
            Nova_senha = _configuration["SENHA_USUARIO_IMPORTAR_EXPORTAR"],
            Acessos = new List<AcessoUsuarioDtoModel>()
        };
        var acessosBusnModel = AcessoBusnModel.GetAcessosPadrao();
        foreach (var acessoBusn in acessosBusnModel)
        {
            AcessoUsuarioDtoModel acessoUsuarioDto = new AcessoUsuarioDtoModelAdapter(acessoBusn);
            usuarioImportarExportar.Acessos.Add(acessoUsuarioDto);
        }
        return usuarioImportarExportar;
    }

    public TimeSpan GetTempoExpirarSessao()
    {
        var segs = _configuration.GetValue<int>("Autorizacao:Sessao:SegundosAteExpirar");
        var tempoExpiracao = TimeSpan.FromSeconds(segs);
        return tempoExpiracao;
    }

    public bool EhAmbienteDesenv()
    {
        return GetSegredo().StartsWith("Sim");
    }
}