using DDD.Api.Domain.Interface.Infra.Configuration.BatchApp;

namespace DDD.Api.Infra.Configuration.BatchApp;
public class BatchAppConfiguration : IBatchAppConfiguration
{
    private readonly IConfiguration _configuration;
    public BatchAppConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TimeSpan GetTempoMinimoDescanso()
    {
        var segs = _configuration.GetValue<int>("BatchExclusaoSessoes:SegundosDescanso");
        var tempoMinDescanso = TimeSpan.FromSeconds(segs);
        return tempoMinDescanso;
    }

    public TimeSpan GetTempoMaixmoDescanso()
    {
        var segs = _configuration.GetValue<int>("BatchExclusaoSessoes:SegundosInatividadeMaxima");
        var tempoMaxDescanso = TimeSpan.FromSeconds(segs);
        return tempoMaxDescanso;
    }
}