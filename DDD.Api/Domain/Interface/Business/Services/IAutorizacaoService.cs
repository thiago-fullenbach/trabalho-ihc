using DDD.Api.Domain.Models.BusnModel;

namespace DDD.Api.Domain.Interface.Business.Services;
public interface IAutorizacaoService
{
    Task LogarAsync(UsuarioBusnModel usuario);
    Task ReautenticarSessaoAsync();
    string CriptografarSenhaAppInterno(string senha);
    Task<string> CriptografarSenhaAppExternoAsync(string urlEndpointCriptografarSenha, string senha);
    Task<string> CriptografarSenhaByUrlAsync(string urlEndpointCriptografarSenha, string senha);
    Task ExcluirSessoesExpiradasAsync();
}