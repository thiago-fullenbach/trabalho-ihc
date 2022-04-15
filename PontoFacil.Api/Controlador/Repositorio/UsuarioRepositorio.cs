using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Repositorio;
public class UsuarioRepositorio
{
    private readonly PontoFacilContext _c;
    private readonly ContaRepositorio _contaR;
    private readonly IConfiguration _config;
    public UsuarioRepositorio(PontoFacilContext c, ContaRepositorio contaR, IConfiguration config)
    {
        _c = c;
        _contaR = contaR;
        _config = config;
    }

    public List<Usuario> ObterUsuarioPeloFiltro(FiltrarUsuarioDTO filtro)
    {
        var saida_usuarios = new List<Usuario>();
        var doSql_usuarios = _c.Usuario.AsNoTracking().Where(x => (string.IsNullOrEmpty(filtro.Nome) || (x.Nome ?? string.Empty).ToUpper().Contains(filtro.Nome.ToUpper()))
                                                                  && (string.IsNullOrEmpty(filtro.CPF) || (x.CPF ?? string.Empty) == filtro.CPF)
                                                                  && (filtro.DataNascimento == null || x.DataNascimento == filtro.DataNascimento));
        if (doSql_usuarios != null && doSql_usuarios.Any())
        {
            foreach (var doSql_iUsuario in doSql_usuarios)
            {
                saida_usuarios.Add(doSql_iUsuario);
            }
        }
        return saida_usuarios;
    }

    public async Task<Usuario> RegistrarUsuarioEConta(ManterUsuarioComContaDTO usuarioComConta)
    {
        var inSql_usuario = new Usuario
        {
            Nome = usuarioComConta.Nome,
            CPF = usuarioComConta.CPF,
            DataNascimento = usuarioComConta.DataNascimento.Value
        };
        var inSql_conta = new Conta
        {
            Login = usuarioComConta.Login,
            Senha = _contaR.HashHmacSha512SenhaConta(usuarioComConta.Senha ?? string.Empty),
            DataHoraUltimaAlteracaoLogin = DateTime.MinValue,
            IdUsuarioNavegacao = inSql_usuario
        };

        await _c.Usuario.AddAsync(inSql_usuario);
        await _c.Conta.AddAsync(inSql_conta);
        await _c.SaveChangesAsync();

        return inSql_usuario;
    }

    public async Task<Usuario?> RegistrarContaDeUsuarioJaExistente(int idUsuario, ManterUsuarioComContaDTO usuarioComConta)
    {
        var upSql_usuario = _c.Usuario.FirstOrDefault(x => x.Id == idUsuario);
        if (upSql_usuario == null) return null;

        upSql_usuario.Nome = usuarioComConta.Nome;
        upSql_usuario.CPF = usuarioComConta.CPF;
        upSql_usuario.DataNascimento = usuarioComConta.DataNascimento.Value;
        var inSql_conta = new Conta
        {
            Login = usuarioComConta.Login,
            Senha = _contaR.HashHmacSha512SenhaConta(usuarioComConta.Senha ?? string.Empty),
            DataHoraUltimaAlteracaoLogin = DateTime.MinValue,
            IdUsuario = idUsuario
        };

        _c.Usuario.Update(upSql_usuario);
        await _c.Conta.AddAsync(inSql_conta);
        await _c.SaveChangesAsync();

        return upSql_usuario;
    }
}