using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.Business.Adapter.DtoModelAdapter;
public class UsuarioLogadoDtoModelAdapter : UsuarioLogadoDtoModel
{
    public UsuarioLogadoDtoModelAdapter(UsuarioBusnModel usuarioBusnModelAdaptee)
    {
        Id = usuarioBusnModelAdaptee.Id;
        Nome = usuarioBusnModelAdaptee.Nome;
        CPF = usuarioBusnModelAdaptee.CPF;
        Data_nascimento = usuarioBusnModelAdaptee.DataNascimento;
        Horas_diarias = usuarioBusnModelAdaptee.HorasDiarias;
        Login = usuarioBusnModelAdaptee.Login;
    }
}