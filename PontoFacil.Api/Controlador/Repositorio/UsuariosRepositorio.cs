using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;
using Valida.CPF.CNPJ;

namespace PontoFacil.Api.Controlador.Repositorio;
public class UsuariosRepositorio
{
    private readonly PontoFacilContexto _contexto;
    private readonly CriptografiaServico _criptografiaServico;
    private readonly UsuarioConvertUnique _usuarioConvertUnique;
    private readonly RecursoConvertUnique _recursoConvertUnique;
    public UsuariosRepositorio(PontoFacilContexto contexto,
                               CriptografiaServico criptografiaServico,
                               UsuarioConvertUnique usuarioConvertUnique,
                               RecursoConvertUnique recursoConvertUnique)
    {
        _contexto = contexto;
        _criptografiaServico = criptografiaServico;
        _usuarioConvertUnique = usuarioConvertUnique;
        _recursoConvertUnique = recursoConvertUnique;
    }
    public Usuarios RecuperarUsuarioPeloLoginSenha(LoginXSenhaDTO loginSenha)
    {
        string senhaHasheada = _criptografiaServico.HashearSenha(loginSenha.Senha);
        var usuario = _contexto.Usuarios
            .AsNoTracking()
            .FirstOrDefault(x => x.login == loginSenha.Login
            && x.senha == senhaHasheada);
        var mensagens = new List<string>();
        if (usuario == null)
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY, "Login ou senha", "os")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.NotFound);

        return usuario;
    }
    public async Task<Usuarios> CriarUsuarioPeloCadastreSe(CadUsuarioCadastreSeDTO cadUsuario)
    {
        var inclUsuario = _usuarioConvertUnique.ParaUsuarios(cadUsuario);
        var dataAgr = DateTime.Now;
        inclUsuario.datahora_criacao = dataAgr;
        var recursoPadrao = RecursoConvertUnique.RecursoPadrao;
        var inclRecurso = _recursoConvertUnique.ParaRecursos(recursoPadrao);
        inclRecurso.datahora_criacao = dataAgr;
        inclRecurso.NavegacaoUsuarios = inclUsuario;
        await _contexto.Usuarios.AddAsync(inclUsuario);
        await _contexto.Recursos.AddAsync(inclRecurso);
        await _contexto.SaveChangesAsync();
        return inclUsuario;
    }
    public IList<Usuarios> RecuperarUsuariosPeloFiltro(FiltroUsuarioDTO filtro)
    {
        var usuarios = _contexto.Usuarios
            .AsNoTracking()
            .Where(x => (string.IsNullOrWhiteSpace(filtro.Nome) || x.nome.Contains(filtro.Nome))
            && (string.IsNullOrWhiteSpace(filtro.CPF) || x.cpf == filtro.CPF)
            && (!filtro.Data_nascimento.HasValue || x.data_nascimento == filtro.Data_nascimento)
            && (!filtro.Horas_diarias.HasValue || x.horas_diarias == filtro.Horas_diarias)
            && (string.IsNullOrWhiteSpace(filtro.Login) || x.login.Contains(filtro.Login)));
        return Utilitarios.ParaLista(usuarios);
    }
    public void ValidarLoginSenhaObrigatorios(LoginXSenhaDTO loginSenha)
    {
        var mensagens = new List<string>();
        if (string.IsNullOrWhiteSpace(loginSenha.Login))
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "Login", "o")); }
        if (string.IsNullOrWhiteSpace(loginSenha.Senha))
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "Senha", "a")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
    }
    public void ValidarCadastreSe(CadUsuarioCadastreSeDTO cadUsuario)
    {
        ValidarCadastreSeObrigatorios(cadUsuario);
        ValidarNomeCadUsuario(cadUsuario.Nome);
        ValidarCpfCadUsuario(cadUsuario.CPF);
        ValidarDataNascimentoCadUsuario(cadUsuario.Data_nascimento);
        ValidarHorasDiariasCadUsuario(cadUsuario.Horas_diarias);
        ValidarLoginCadUsuario(cadUsuario.Login);
        ValidarSenhaCadUsuario(cadUsuario.Senha);
    }
    public void ValidarCadastreSeObrigatorios(CadUsuarioCadastreSeDTO cadUsuario)
    {
        var mensagens = new List<string>();
        if (string.IsNullOrWhiteSpace(cadUsuario.Nome))
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "Nome", "o")); }
        if (string.IsNullOrWhiteSpace(cadUsuario.CPF))
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "CPF", "o")); }
        if (!cadUsuario.Data_nascimento.HasValue)
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "Data de Nascimento", "a")); }
        if (!cadUsuario.Horas_diarias.HasValue)
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "Quantidade de Horas Diárias", "a")); }
        if (string.IsNullOrWhiteSpace(cadUsuario.Login))
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "Login", "o")); }
        if (string.IsNullOrWhiteSpace(cadUsuario.Senha))
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "Senha", "a")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
    }
    public void ValidarNomeCadUsuario(string? nome)
    {
        var mensagens = new List<string>();
        if (string.IsNullOrWhiteSpace(nome))
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "Nome", "o")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
        
        if (nome.Length < 4)
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY_MOTIVO_ZZZZ, "Nome", "o", "deve possuir pelo menos 4 caracteres")); }
        if (Regex.IsMatch(nome, "[^A-Za-z.]+"))
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY_MOTIVO_ZZZZ, "Nome", "o", "aceita apenas letras e ponto (.)")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
    }
    public void ValidarCpfCadUsuario(string? cpf)
    {
        var mensagens = new List<string>();
        if (string.IsNullOrWhiteSpace(cpf))
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "CPF", "o")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
        
        if (!ValidaCPFCNPJ.ValidaCPF(cpf.Replace(".", "").Replace("-", "")))
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY, "CPF", "o")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);

        var filtroCpf = _usuarioConvertUnique.ParaFiltroUsuarioDTOPesquisavel(new FiltroUsuarioDTO { CPF = cpf });
        var listaUsuario = RecuperarUsuariosPeloFiltro(filtroCpf);
        if (listaUsuario.Count > 0)
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY_MOTIVO_ZZZZ, "CPF", "o", "esse CPF já está em uso")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
    }
    public void ValidarDataNascimentoCadUsuario(DateTime? dataNascimento)
    {
        var mensagens = new List<string>();
        if (!dataNascimento.HasValue)
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "Data de Nascimento", "a")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
        
        var dataHj = DateTime.Today;
        var menorDataNasc = new DateTime(dataHj.Year - 160, dataHj.Month, dataHj.Day);
        if (dataNascimento < menorDataNasc || dataNascimento > dataHj)
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY, "Data de Nascimento", "a")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
    }
    public void ValidarHorasDiariasCadUsuario(int? horasDiarias)
    {
        var mensagens = new List<string>();
        if (!horasDiarias.HasValue)
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "Quantidade de Horas Diárias", "a")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
        
        var dataHj = DateTime.Today;
        if (horasDiarias < 2 || horasDiarias > 12)
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY, "Quantidade de Horas Diárias", "a")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
    }
    public void ValidarLoginCadUsuario(string? login)
    {
        var mensagens = new List<string>();
        if (string.IsNullOrWhiteSpace(login))
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "Login", "o")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
        
        if (login.Length < 8)
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY_MOTIVO_ZZZZ, "Login", "o", "deve possuir pelo menos 8 caracteres")); }
        if (Regex.IsMatch(login, "[^A-Za-z0-9_.]+"))
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY_MOTIVO_ZZZZ, "Login", "o", "aceita apenas letras, números, underline (_) e ponto (.)")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);

        var filtroLogin = _usuarioConvertUnique.ParaFiltroUsuarioDTOPesquisavel(new FiltroUsuarioDTO { Login = login });
        var listaUsuario = RecuperarUsuariosPeloFiltro(filtroLogin);
        if (listaUsuario.Count > 0)
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY_MOTIVO_ZZZZ, "Login", "o", "esse Login já está em uso")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
    }
    public void ValidarSenhaCadUsuario(string? senha)
    {
        var mensagens = new List<string>();
        if (string.IsNullOrWhiteSpace(senha))
            { mensagens.Add(string.Format(Mensagens.XXXX_OBRIGATORIY, "Senha", "a")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);

        if (senha.Length < 8)
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY_MOTIVO_ZZZZ, "Senha", "a", "deve possuir pelo menos 8 caracteres")); }
        if (!Regex.IsMatch(senha, "[A-Za-z]+") || !Regex.IsMatch(senha, "[0-9]+"))
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY_MOTIVO_ZZZZ, "Senha", "a", "deve conter pelo menos uma letra e um número")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
    }
    public void ValidarFiltroUsuario(FiltroUsuarioDTO filtro)
    {
        if (string.IsNullOrWhiteSpace(filtro.CPF)) { return; }
        
        var mensagens = new List<string>();
        if (!ValidaCPFCNPJ.ValidaCPF(filtro.CPF.Replace(".", "").Replace("-", "")))
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY, "CPF", "o")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);
    }
}