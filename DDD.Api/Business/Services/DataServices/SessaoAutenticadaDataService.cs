using DDD.Api.Domain.Interface.Business.Services.DataServices;
using DDD.Api.Domain.Models.BusnModel;

namespace DDD.Api.Business.Services.DataServices;
public class SessaoAutenticadaDataService : ISessaoAutenticadaDataService
{
    private SessaoBusnModel? _sessaoAutenticada;
    public bool TemSessaoAutenticada()
    {
        return _sessaoAutenticada != null;
    }

    public SessaoBusnModel? GetSessaoAutenticada()
    {
        return _sessaoAutenticada;
    }

    public void SetSessaoAutenticada(SessaoBusnModel sessao)
    {
        _sessaoAutenticada = sessao;
    }
}