using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Domain.Interface.Business.IntegratedAdapter.BusnModelAdapter;
public interface IPresencaBusnModelIntegratedAdapter
{
    Task<PresencaBusnModel> ToPresencaPesquisadaAsync(Presenca presencaAdaptee);
    PresencaBusnModel ToNovaPresenca(NovaPresencaDtoModel novaPresencaDtoAdaptee);
}