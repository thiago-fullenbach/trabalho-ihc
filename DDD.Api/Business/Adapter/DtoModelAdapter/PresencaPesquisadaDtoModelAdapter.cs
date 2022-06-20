using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.Business.Adapter.BusnModelAdapter;
public class PresencaPesquisadaDtoModelAdapter : PresencaPesquisadaDtoModel
{
    public PresencaPesquisadaDtoModelAdapter(PresencaBusnModel presencaBusnAdaptee)
    {
        Id = presencaBusnAdaptee.Id;
        Usuario_id = presencaBusnAdaptee.IdUsuarioPresente;
        Eh_entrada = presencaBusnAdaptee.EhEntrada;
        Datahora_presenca = presencaBusnAdaptee.Hora;
        Tem_visto = presencaBusnAdaptee.TemVisto;
    }
}