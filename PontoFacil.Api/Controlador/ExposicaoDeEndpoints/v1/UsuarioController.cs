using System.Net;
using Microsoft.AspNetCore.Mvc;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoClienteParaServidor;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
using PontoFacil.Api.Controlador.Middleware;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Controlador.Repositorio.Convert;
using PontoFacil.Api.Controlador.Repositorio.Convert.ConvertUnique;
using PontoFacil.Api.Modelo;
using PontoFacil.Api.Modelo.Dominios;

namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1;
[ApiController]
[Route("api/v1/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly AcessoRepositorio _acessoRepositorio;
    private readonly UsuarioRepositorio _usuarioRepositorio;
    private readonly AcessoConvert _acessoConvert;
    private readonly UsuarioConvert _usuarioConvert;
    private readonly AcessoConvertUnique _acessoConvertUnique;
    private readonly UsuarioConvertUnique _usuarioConvertUnique;
    public UsuarioController(AcessoRepositorio acessoRepositorio,
                             UsuarioRepositorio usuarioRepositorio,
                             AcessoConvert acessoConvert,
                             UsuarioConvert usuarioConvert,
                             AcessoConvertUnique acessoConvertUnique,
                             UsuarioConvertUnique usuarioConvertUnique)
    {
        _acessoRepositorio = acessoRepositorio;
        _usuarioRepositorio = usuarioRepositorio;
        _acessoConvert = acessoConvert;
        _usuarioConvert = usuarioConvert;
        _acessoConvertUnique = acessoConvertUnique;
        _usuarioConvertUnique = usuarioConvertUnique;
    }

    [Autorizar]
    [HttpGet]
    [Route("listarTodos")]
    public IActionResult ListarTodos()
    {
        // _usuariosRepositorio.AutorizaUsuario(usuarioLogado, x => x.Pode_vis_demais_usuarios);

        var listaUsuarios = _usuarioRepositorio.RecuperarUsuariosPeloFiltro(new FiltroUsuarioDTO());
        var listaUsuariosEmDTO = new List<UsuarioPesquisadoDTO>();
        foreach (var iUsuario in listaUsuarios)
            { listaUsuariosEmDTO.Add(_usuarioConvertUnique.ParaUsuarioPesquisadoDTO(iUsuario)); }
        var usuarioLogado = _usuarioConvertUnique.ExtrairUsuarioLogado(HttpContext.Request.Headers);
        if (!(_acessoConvert.UsuarioTemRecursoHabilitado(usuarioLogado, EnRecurso.VisualizarUsuario)))
            { listaUsuariosEmDTO = new List<UsuarioPesquisadoDTO>(_usuarioRepositorio.UsuariosExceto(listaUsuariosEmDTO, usuarioLogado.Id)); }
        if (!(_acessoConvert.UsuarioTemRecursoHabilitado(usuarioLogado, EnRecurso.VisualizarDemaisUsuarios)))
            { listaUsuariosEmDTO = new List<UsuarioPesquisadoDTO>(_usuarioRepositorio.UsuariosApenas(listaUsuariosEmDTO, usuarioLogado.Id)); }
        
        var json = new DevolvidoMensagensDTO { Devolvido = listaUsuariosEmDTO };
        json.SetMensagemUnica(Mensagens.REQUISICAO_SUCESSO);
        return StatusCode((int)HttpStatusCode.OK, json);
    }
    
    [Autorizar]
    [HttpGet]
    [Route("pegaPeloId")]
    public IActionResult PegaPeloId(int id)
    {
        var usuario = _usuarioRepositorio.PegaUsuarioPeloId(id);
        var detalheUsuario = _usuarioConvert.ParaDetalheUsuarioDTO(usuario);
        var usuarioLogado = _usuarioConvertUnique.ExtrairUsuarioLogado(HttpContext.Request.Headers);

        var mensagens = new List<string>();
        if (detalheUsuario.Id == usuarioLogado.Id && !(_acessoConvert.UsuarioTemRecursoHabilitado(usuarioLogado, EnRecurso.VisualizarUsuario)))
            { mensagens.Add(Mensagens.ACESSO_NEGADO); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.Unauthorized);
        if (detalheUsuario.Id != usuarioLogado.Id && !(_acessoConvert.UsuarioTemRecursoHabilitado(usuarioLogado, EnRecurso.VisualizarDemaisUsuarios)))
            { mensagens.Add(Mensagens.ACESSO_NEGADO); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.Unauthorized);
        
        var json = new DevolvidoMensagensDTO { Devolvido = detalheUsuario };
        json.SetMensagemUnica(Mensagens.REQUISICAO_SUCESSO);
        return StatusCode((int)HttpStatusCode.OK, json);
    }
    
    [Autorizar]
    [HttpPost]
    [Route("excluiPeloId")]
    public async Task<IActionResult> ExcluiPeloId(int id)
    {
        var usuario = _usuarioRepositorio.PegaUsuarioPeloId(id);
        var usuarioLogado = _usuarioConvertUnique.ExtrairUsuarioLogado(HttpContext.Request.Headers);

        var mensagens = new List<string>();
        if (usuario.id != usuarioLogado.Id && !(_acessoConvert.UsuarioTemRecursoHabilitado(usuarioLogado, EnRecurso.CadastrarDemaisUsuarios)))
            { mensagens.Add(Mensagens.ACESSO_NEGADO); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.Unauthorized);
        if (usuario.id == usuarioLogado.Id)
            { mensagens.Add("Um usuário não pode excluir a si mesmo"); }
        NegocioException.ThrowErroSeHouver(mensagens, (int)HttpStatusCode.Forbidden);

        await _usuarioRepositorio.ExcluiUsuarioPeloId(id);
        
        var json = new DevolvidoMensagensDTO();
        json.SetMensagemUnica(Mensagens.REQUISICAO_SUCESSO);
        return StatusCode((int)HttpStatusCode.OK, json);
    }
    
    [Autorizar]
    [HttpPost]
    [Route("insere")]
    public async Task<IActionResult> Insere([FromBody]NovoUsuarioDTO novoUsuario)
    {
        var usuarioLogado = _usuarioConvertUnique.ExtrairUsuarioLogado(HttpContext.Request.Headers);
        _usuarioRepositorio.AutorizaUsuario(usuarioLogado, (int)EnRecurso.CadastrarDemaisUsuarios);

        var usuarioCadastrese = _usuarioConvertUnique.ParaCadUsuarioCadastreSeDTO(novoUsuario);
        _usuarioRepositorio.ValidarCadastreSe(usuarioCadastrese);
        if (!(_acessoConvert.UsuarioTemRecursoHabilitado(usuarioLogado, EnRecurso.CadastrarAcessoTodosUsuarios)))
        {
            novoUsuario.Acessos = new List<AcessoUsuarioDTO>();
            var acessosLogado = AcessoConvertUnique.AcessosPadrao;
            foreach (var iAcessoLogado in acessosLogado)
                { novoUsuario.Acessos.Add(_acessoConvertUnique.ParaAcessoUsuarioDTO(iAcessoLogado)); }
        }

        var usuarioCriado = await _usuarioRepositorio.CriaNovoUsuario(novoUsuario);
        var detalheUsuario = _usuarioConvert.ParaDetalheUsuarioDTO(usuarioCriado);
        
        var json = new DevolvidoMensagensDTO { Devolvido = detalheUsuario };
        json.SetMensagemUnica(Mensagens.REQUISICAO_SUCESSO);
        return StatusCode((int)HttpStatusCode.OK, json);
    }

    [Autorizar]
    [HttpPost]
    [Route("atualiza")]
    public async Task<IActionResult> Atualiza([FromBody]EditarUsuarioDTO editarUsuario)
    {
        var usuarioLogado = _usuarioConvertUnique.ExtrairUsuarioLogado(HttpContext.Request.Headers);
        _usuarioRepositorio.AutorizaUsuario(usuarioLogado, (int)EnRecurso.CadastrarDemaisUsuarios);

        _usuarioRepositorio.ValidaEditarUsuario(editarUsuario);
        if (!(_acessoConvert.UsuarioTemRecursoHabilitado(usuarioLogado, EnRecurso.CadastrarAcessoTodosUsuarios)) || editarUsuario.Id == usuarioLogado.Id)
        {
            editarUsuario.Acessos = new List<AcessoUsuarioDTO>();
            var acessosUsuario = _acessoRepositorio.TrazAcessosDoUsuario(editarUsuario.Id);
            foreach (var iAcesso in acessosUsuario)
                { editarUsuario.Acessos.Add(_acessoConvertUnique.ParaAcessoUsuarioDTO(iAcesso)); }
        }

        var usuarioAtualizado = await _usuarioRepositorio.AtualizaUsuario(editarUsuario);
        var detalheUsuario = _usuarioConvert.ParaDetalheUsuarioDTO(usuarioAtualizado);
        
        var json = new DevolvidoMensagensDTO { Devolvido = detalheUsuario };
        json.SetMensagemUnica(Mensagens.REQUISICAO_SUCESSO);
        return StatusCode((int)HttpStatusCode.OK, json);
    }
}
