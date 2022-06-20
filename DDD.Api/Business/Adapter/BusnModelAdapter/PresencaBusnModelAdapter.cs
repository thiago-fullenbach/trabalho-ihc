using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.Adapter.BusnModelAdapter;
public class PresencaBusnModelAdapter : PresencaBusnModel
{
    public PresencaBusnModelAdapter(Presenca presencaAdaptee)
    {
        Id = presencaAdaptee.Id.ParseZeroIfFails();
        IdUsuarioPresente = presencaAdaptee.usuario_id.ParseZeroIfFails();
        EhEntrada = presencaAdaptee.eh_entrada ?? false;
        Hora = presencaAdaptee.datahora_presenca;
        IdLocal = presencaAdaptee.local_id.ParseZeroIfFails();
        TemVisto = presencaAdaptee.tem_visto ?? false;
    }

    public PresencaBusnModelAdapter(NovaPresencaDtoModel novaPresencaDtoAdaptee)
    {
    }
}