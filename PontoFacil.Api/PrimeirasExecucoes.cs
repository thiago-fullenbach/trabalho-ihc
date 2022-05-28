using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Batch;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo.Contexto;

namespace PontoFacil.Api;
public class PrimeirasExecucoes
{
    public PrimeirasExecucoes()
    {
        var instcAplicacao = AplicacaoMementoSingleton.PegaInstancia();
        _builder = instcAplicacao.Builder;
        _app = instcAplicacao.App;
        instcAplicacao.ServiceProvider = _app.Services.CreateScope().ServiceProvider;
        _serviceProvider = instcAplicacao.ServiceProvider;
        _configServico = _serviceProvider.GetService<ConfiguracoesServico>();
        _usuarioRepositorio = _serviceProvider.GetService<UsuarioRepositorio>();
        _ehBancoDadosRelacional = _builder.Configuration["BancoDadosRelacional"] == "S";
    }
    private readonly WebApplicationBuilder _builder;
    private readonly WebApplication _app;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConfiguracoesServico _configServico;
    private readonly UsuarioRepositorio _usuarioRepositorio;
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
        await _usuarioRepositorio.CriarUsuarioPeloCadastreSe(_configServico.UsuarioImportarExportar);
        var adminRaiz = await _usuarioRepositorio.CriarUsuarioPeloCadastreSe(_configServico.UsuarioAdminRaiz);
        await _usuarioRepositorio.TornaAdministrador(adminRaiz.id);
    }
}