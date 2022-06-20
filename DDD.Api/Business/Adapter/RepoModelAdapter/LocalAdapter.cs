using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.Adapter.RepoModelAdapter;
public class LocalAdapter : Local
{
    public LocalAdapter(LocalBusnModel localBusnAdaptee)
    {
        Id = localBusnAdaptee.Id.ToString();
        cep = localBusnAdaptee.CodigoCep;
        logradouro = localBusnAdaptee.NomeLogradouro;
        numero = localBusnAdaptee.NumeroLogradouro;
        complemento = localBusnAdaptee.ComplementoLogradouro;
        bairro = localBusnAdaptee.NomeBairro;
        cidade = localBusnAdaptee.NomeCidade;
        estado = localBusnAdaptee.NomeEstado;
    }
}