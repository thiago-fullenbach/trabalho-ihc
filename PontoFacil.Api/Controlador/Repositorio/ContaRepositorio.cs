using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Repositorio;
public class ContaRepositorio
{
    private readonly PontoFacilContext _c;
    private readonly IConfiguration _config;
    private readonly HMACSHA512 _hmacSha512;
    public ContaRepositorio(PontoFacilContext c, IConfiguration config)
    {
        _c = c;
        _config = config;
        string segredo = config["Autorizacao:Segredo"].ToString();
        _hmacSha512 = new HMACSHA512(Encoding.UTF8.GetBytes(segredo));
    }
    public string HashHmacSha512SenhaConta(string senhaConta)
    {
        var bytesSenha = Encoding.UTF8.GetBytes(senhaConta);
        var hash = _hmacSha512.ComputeHash(bytesSenha);
        return BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
    }
    public Conta? ObterContaPeloLoginXSenha(string login, string senha)
    {
        string senhaHash = HashHmacSha512SenhaConta(senha);
        var conta = _c.Conta.AsNoTracking()
                            .FirstOrDefault(x => x.Login == login && x.Senha == senhaHash);
        return conta;
    }
    public bool LoginExiste(string login)
    {
        var contaExiste = _c.Conta.AsNoTracking().Any(x => x.Login == login);
        return contaExiste;
    }
    public bool UsuarioTemConta(int idUsuario)
    {
        var usuarioTemConta = _c.Conta.AsNoTracking().Any(x => x.IdUsuario == idUsuario);
        return usuarioTemConta;
    }
}
