using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Batch;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api;
public class PrimeirasExecucoes
{
    public PrimeirasExecucoes(WebApplicationBuilder builder, WebApplication app)
    {
        _builder = builder;
        _app = app;
        _serviceProvider = app.Services.CreateScope().ServiceProvider;
        _configServico = _serviceProvider.GetService<ConfiguracoesServico>();
        _usuariosRepositorio = _serviceProvider.GetService<UsuariosRepositorio>();
        _ehBancoDadosRelacional = builder.Configuration["BancoDadosRelacional"] == "S";
    }
    private readonly WebApplicationBuilder _builder;
    private readonly WebApplication _app;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConfiguracoesServico _configServico;
    private readonly UsuariosRepositorio _usuariosRepositorio;
    private readonly bool _ehBancoDadosRelacional;
    public async Task MigraBancoDadosSeRelacionalAsync()
    {
        if (_ehBancoDadosRelacional)
        {
            var contexto = _serviceProvider.GetService<PontoFacilContexto>();
            await contexto.Database.MigrateAsync();
        }
    }
    public async Task CarregaBatchExclusaoSessoesAsync()
    {
        await GerenciadorAgendamento.Instancia(_configServico);
        ParametrosBatchExclusaoSessoes.UrlsServidor = new List<string>(_builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey).Replace("*", "localhost").Split(';'));
    }
    public async Task CarregaDadosIniciaisAsync()
    {
        await _usuariosRepositorio.CriarUsuarioPeloCadastreSe(_configServico.UsuarioImportarExportar);
        var adminRaiz = await _usuariosRepositorio.CriarUsuarioPeloCadastreSe(_configServico.UsuarioAdminRaiz);
        await _usuariosRepositorio.TornaAdministrador(adminRaiz.id);
    }
}