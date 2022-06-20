using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.Adapter.BusnModelAdapter;
public class LocalBusnModelAdapter : LocalBusnModel
{
    public LocalBusnModelAdapter(Local localAdaptee)
    {
        Id = localAdaptee.Id.ParseZeroIfFails();
        CodigoCep = localAdaptee.cep;
        NomeLogradouro = localAdaptee.logradouro;
        NumeroLogradouro = localAdaptee.numero;
        ComplementoLogradouro = localAdaptee.complemento;
        NomeBairro = localAdaptee.bairro;
        NomeCidade = localAdaptee.cidade;
        NomeEstado = localAdaptee.estado;
    }

    public LocalBusnModelAdapter(LocalNovaPresencaDtoModel localNovaPresencaDtoAdaptee)
    {
        CodigoCep = localNovaPresencaDtoAdaptee.Cep;
        NomeLogradouro = localNovaPresencaDtoAdaptee.Nome_logradouro;
        NumeroLogradouro = localNovaPresencaDtoAdaptee.Numero_logradouro;
        ComplementoLogradouro = localNovaPresencaDtoAdaptee.Complemento_logradouro;
        NomeBairro = localNovaPresencaDtoAdaptee.Nome_bairro;
        NomeCidade = localNovaPresencaDtoAdaptee.Nome_cidade;
        NomeEstado = localNovaPresencaDtoAdaptee.Nome_estado;
    }
}