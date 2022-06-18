using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.Adapter.RepoModelAdapter;
public class UsuarioAdapter : Usuario
{
    public UsuarioAdapter(UsuarioBusnModel usuarioBusnModelAdaptee)
    {
        Id = usuarioBusnModelAdaptee.Id.ToString();
        nome = usuarioBusnModelAdaptee.Nome;
        cpf = usuarioBusnModelAdaptee.CPF;
        data_nascimento = usuarioBusnModelAdaptee.DataNascimento;
        horas_diarias = usuarioBusnModelAdaptee.HorasDiarias;
        login = usuarioBusnModelAdaptee.Login;
        senha = usuarioBusnModelAdaptee.Senha;
        url_hasheia_senha_sem_parametros = usuarioBusnModelAdaptee.UrlEndpointHashearSenha;
    }
}