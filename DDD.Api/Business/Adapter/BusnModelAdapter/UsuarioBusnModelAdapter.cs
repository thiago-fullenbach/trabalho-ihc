using DDD.Api.Domain.Models.BusnModel;
using DDD.Api.Domain.Models.DtoModel;
using DDD.Api.Domain.Models.RepoModel;

namespace DDD.Api.Business.Adapter.BusnModelAdapter;
public class UsuarioBusnModelAdapter : UsuarioBusnModel
{
    public UsuarioBusnModelAdapter(Usuario usuarioAdaptee)
    {
        Id = usuarioAdaptee.Id.ParseZeroIfFails();
        Nome = usuarioAdaptee.nome;
        CPF = usuarioAdaptee.cpf;
        DataNascimento = usuarioAdaptee.data_nascimento;
        HorasDiarias = usuarioAdaptee.horas_diarias;
        Login = usuarioAdaptee.login;
        Senha = usuarioAdaptee.senha;
        UrlEndpointHashearSenha = usuarioAdaptee.url_hasheia_senha_sem_parametros;
    }

    public UsuarioBusnModelAdapter(LoginXSenhaDtoModel loginXSenhaAdaptee)
    {
        Login = loginXSenhaAdaptee.Login ?? string.Empty;
        Senha = loginXSenhaAdaptee.Senha ?? string.Empty;
    }

    public UsuarioBusnModelAdapter(NovoUsuarioDtoModel novoUsuarioAdaptee)
    {
        Nome = novoUsuarioAdaptee.Nome;
        CPF = novoUsuarioAdaptee.CPF;
        DataNascimento = novoUsuarioAdaptee.Data_nascimento;
        HorasDiarias = novoUsuarioAdaptee.Horas_diarias;
        Login = novoUsuarioAdaptee.Login;
        Senha = novoUsuarioAdaptee.Nova_senha;
    }

    public UsuarioBusnModelAdapter(EditarUsuarioDtoModel editarUsuarioAdaptee)
    {
        Id = editarUsuarioAdaptee.Id;
        Nome = editarUsuarioAdaptee.Nome;
        CPF = editarUsuarioAdaptee.CPF;
        DataNascimento = editarUsuarioAdaptee.Data_nascimento;
        HorasDiarias = editarUsuarioAdaptee.Horas_diarias;
        Login = editarUsuarioAdaptee.Login;
        Senha = editarUsuarioAdaptee.Nova_senha ?? string.Empty;
    }
}