using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.Business.Adapter.DtoModelAdapter;
public class UsuarioPesquisadoDtoModelAdapter : UsuarioPesquisadoDtoModel
{
    public UsuarioPesquisadoDtoModelAdapter(UsuarioBusnModel usuarioBusnAdaptee)
    {
        Id = usuarioBusnAdaptee.Id;
        Nome = usuarioBusnAdaptee.Nome;
        CPF = usuarioBusnAdaptee.CPF;
        Data_nascimento = usuarioBusnAdaptee.DataNascimento;
        Horas_diarias = usuarioBusnAdaptee.HorasDiarias;
        Login = usuarioBusnAdaptee.Login;
    }
}