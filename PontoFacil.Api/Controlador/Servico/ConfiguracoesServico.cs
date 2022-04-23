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
    public ConfiguracoesServico(IConfiguration configuration)
    {
        _configuration = configuration;
        Segredo = _configuration["Autorizacao:Segredo"];
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
    }
}