using System.Globalization;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Modelo;

namespace PontoFacil.Api.Controlador.Servico;
public class ConfiguracoesServico
{
    private readonly IConfiguration _configuration;
    public string Segredo { get; set; } 
    public TimeSpan TempoExpirarSessao { get; set; }
    public TimeSpan TempoTrocarLoginNovamente { get; set; }
    public TimeSpan TempoTrocarSenhaNovamente { get; set; }
    public TimeSpan TempoBatchExclusaoSessoesDescanso { get; set; }
    public TimeSpan TempoBatchExclusaoSessoesInatividadeMaxima { get; set; }
    public CultureInfo Cultura { get; set; }
    public CadUsuarioCadastreSeDTO UsuarioImportarExportar { get; set; }
    public CadUsuarioCadastreSeDTO UsuarioAdminRaiz { get; set; }
    public ConfiguracoesServico(IConfiguration configuration)
    {
        _configuration = configuration;
        Segredo = _configuration[ConfiguracoesServico.ChaveSegredoHashHmacsha512];
        int segs;
        segs = int.Parse(_configuration["Autorizacao:Sessao:SegundosAteExpirar"]);
        TempoExpirarSessao = TimeSpan.FromSeconds(segs);
        segs = int.Parse(_configuration["CadUsuario:SegundosParaTrocarLoginNovamente"]);
        TempoTrocarLoginNovamente = TimeSpan.FromSeconds(segs);
        segs = int.Parse(_configuration["CadUsuario:SegundosParaTrocarSenhaNovamente"]);
        TempoTrocarSenhaNovamente = TimeSpan.FromSeconds(segs);
        segs = int.Parse(_configuration["BatchExclusaoSessoes:SegundosDescanso"]);
        TempoBatchExclusaoSessoesDescanso = TimeSpan.FromSeconds(segs);
        segs = int.Parse(_configuration["BatchExclusaoSessoes:SegundosInatividadeMaxima"]);
        TempoBatchExclusaoSessoesInatividadeMaxima = TimeSpan.FromSeconds(segs);
        Cultura = new CultureInfo("pt-BR");
        UsuarioImportarExportar = new CadUsuarioCadastreSeDTO
        {
            Nome = _configuration["UsuarioImportarExportar:Nome"],
            CPF = _configuration["UsuarioImportarExportar:CPF"],
            Data_nascimento = DateTime.ParseExact(_configuration["UsuarioImportarExportar:DataNascimento"], format: "yyyy-MM-dd", null),
            Horas_diarias = int.Parse(_configuration["UsuarioImportarExportar:HorasDiarias"]),
            Login = _configuration["UsuarioImportarExportar:Login"],
            Senha = _configuration[ConfiguracoesServico.ChaveSenhaUsuarioImportarExportar]
        };
        UsuarioAdminRaiz = new CadUsuarioCadastreSeDTO
        {
            Nome = _configuration["UsuarioAdminRaiz:Nome"],
            CPF = _configuration["UsuarioAdminRaiz:CPF"],
            Data_nascimento = DateTime.ParseExact(_configuration["UsuarioAdminRaiz:DataNascimento"], format: "yyyy-MM-dd", null),
            Horas_diarias = int.Parse(_configuration["UsuarioAdminRaiz:HorasDiarias"]),
            Login = _configuration["UsuarioAdminRaiz:Login"],
            Senha = _configuration[ConfiguracoesServico.ChaveSenhaAdministradorRaiz]
        };
    }
    public static readonly string ChaveSegredoHashHmacsha512 = "SEGREDO_HASH_HMACSHA512";
    public static readonly string ChaveConexaoBancoDadosRelacional = "CONEXAO_BANCO_DADOS_RELACIONAL";
    public static readonly string ChaveSenhaUsuarioImportarExportar = "SENHA_USUARIO_IMPORTAR_EXPORTAR";
    public static readonly string ChaveSenhaAdministradorRaiz = "SENHA_ADMINISTRADOR_RAIZ";
    public static bool EhBancoDadosRelacional(ConfigurationManager configuration) => configuration["BancoDadosRelacional"] == "S";
}