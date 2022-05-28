using System.Net;
using Newtonsoft.Json;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Externo;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
public class UsuarioConvertUnique
{
    private readonly PontoFacilContexto _contexto;
    private readonly ConfiguracoesServico _configuracoesServico;
    private readonly CriptografiaServico _criptografiaServico;
    public UsuarioConvertUnique(PontoFacilContexto contexto,
                                ConfiguracoesServico configuracoesServico,
                                CriptografiaServico criptografiaServico)
    {
        _contexto = contexto;
        _configuracoesServico = configuracoesServico;
        _criptografiaServico = criptografiaServico;
    }
    public UsuarioLogadoDTO ParaUsuarioLogadoDTO(Usuario usuario)
    {
        var resultado = new UsuarioLogadoDTO
        {
            Id = usuario.id,
            Nome = usuario.nome,
            CPF = usuario.cpf,
            Data_nascimento = usuario.data_nascimento,
            Horas_diarias = usuario.horas_diarias,
            Login = usuario.login
        };
        return Utilitarios.DevolverComNovoEspacoNaMemoria(resultado);
    }
    public Usuario ParaUsuario(CadUsuarioCadastreSeDTO cadUsuario)
    {
        var instcAplicacao = AplicacaoMementoSingleton.PegaInstancia();
        var cadUsuarioCadastravel = new Usuario
        {
            nome = cadUsuario.Nome.ToUpper(),
            cpf = cadUsuario.CPF.Replace(".", "").Replace("-", ""),
            data_nascimento = cadUsuario.Data_nascimento.Value,
            horas_diarias = cadUsuario.Horas_diarias.Value,
            login = cadUsuario.Login.ToLower(),
            senha = _criptografiaServico.HashearSenha(cadUsuario.Senha),
            url_hasheia_senha_sem_parametros = instcAplicacao.PegaUrlHasheiaSenhaSemParametros()
        };
        return Utilitarios.DevolverComNovoEspacoNaMemoria(cadUsuarioCadastravel);
    }
    public FiltroUsuarioDTO ParaFiltroUsuarioDTOPesquisavel(FiltroUsuarioDTO filtro)
    {
        var filtroPesquisavel = new FiltroUsuarioDTO
        {
            Nome = filtro.Nome,
            CPF = filtro.CPF,
            Data_nascimento = filtro.Data_nascimento,
            Horas_diarias = filtro.Horas_diarias,
            Login = filtro.Login
        };
        if (!string.IsNullOrWhiteSpace(filtro.Nome))
            { filtro.Nome = filtro.Nome.ToUpper(); }
        if (!string.IsNullOrWhiteSpace(filtro.CPF))
            { filtro.CPF = filtro.CPF.Replace(".", "").Replace("-", ""); }
        if (!string.IsNullOrWhiteSpace(filtro.Login))
            { filtro.Login = filtro.Login.ToLower(); }
        return Utilitarios.DevolverComNovoEspacoNaMemoria(filtroPesquisavel);
    }
    public UsuarioPesquisadoDTO ParaUsuarioPesquisadoDTO(Usuario usuario)
    {
        var resultado = new UsuarioPesquisadoDTO
        {
            Id = usuario.id,
            Nome = usuario.nome,
            CPF = usuario.cpf,
            Data_nascimento = usuario.data_nascimento,
            Horas_diarias = usuario.horas_diarias,
            Login = usuario.login
        };
        return Utilitarios.DevolverComNovoEspacoNaMemoria(resultado);
    }
    public UsuarioLogadoDTO ExtrairUsuarioLogado(IHeaderDictionary headers)
    {
        var mensagens = new List<string>();
        if (!headers.ContainsKey("usuario"))
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY, "Usuário logado", "o")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);

        var usuarioJson = headers["usuario"];
        if (string.IsNullOrWhiteSpace(usuarioJson))
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY, "Usuário logado", "o")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);

        var usuario = JsonConvert.DeserializeObject<UsuarioLogadoDTO>(usuarioJson);
        if (usuario == null)
            { mensagens.Add(string.Format(Mensagens.XXXX_INVALIDY, "Usuário logado", "o")); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.BadRequest);

        return Utilitarios.DevolverComNovoEspacoNaMemoria(usuario);
    }
}