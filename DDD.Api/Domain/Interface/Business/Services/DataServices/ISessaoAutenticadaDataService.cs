using DDD.Api.Domain.Models.BusnModel;

namespace DDD.Api.Domain.Interface.Business.Services.DataServices;
public interface ISessaoAutenticadaDataService
{
    bool TemSessaoAutenticada();
    SessaoBusnModel? GetSessaoAutenticada();
    void SetSessaoAutenticada(SessaoBusnModel sessao);
}