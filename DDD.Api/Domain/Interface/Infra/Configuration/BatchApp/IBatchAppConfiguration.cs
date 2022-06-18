namespace DDD.Api.Domain.Interface.Infra.Configuration.BatchApp;
public interface IBatchAppConfiguration
{
    TimeSpan GetTempoMinimoDescanso();
    TimeSpan GetTempoMaixmoDescanso();
}