using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Domain.Interface.Infra.Configuration.BackEndApp;
public interface IBackEndAppConfiguration
{
    string GetSegredo();
    IBackEndAmbiente GetBackEndAmbiente();
    string GetUrlDominioExpostaExternamente();
    string GetUrlHashearSenhaSemParametrosExpostaExternamente();
    NovoUsuarioDtoModel GetNovoUsuarioDtoModelAdminRoot();
    NovoUsuarioDtoModel GetNovoUsuarioDtoModelImportarExportar();
    TimeSpan GetTempoExpirarSessao();
    bool EhAmbienteDesenv();
}