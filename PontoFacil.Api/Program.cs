using Microsoft.EntityFrameworkCore;
using PontoFacil.Api;
using PontoFacil.Api.Batch;
using PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1;
using PontoFacil.Api.Controlador.Repositorio;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo.Contexto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

bool ehBancoDadosRelacional = builder.Configuration["BancoDadosRelacional"] == "S";
if (ehBancoDadosRelacional)
{
    string conexao_mariaDb = builder.Configuration.GetConnectionString("MariaDb");
    builder.Services.AdicionarContextoDeConexaoMySql(conexao_mariaDb);
}
else { builder.Services.AdicionarContextoDeConexaoInMemory(); }
builder.Services.AdicionarBibliotecaDeServicos();
builder.Services.AdicionarBibliotecaDeConversoesUnicas();
builder.Services.AdicionarBibliotecaDeConversoes();
builder.Services.AdicionarBibliotecaDeRepositorios();
string permissoesCorsNome = "_permissoesCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: permissoesCorsNome,
                      policy  =>
                      {
                          policy.AllowAnyOrigin()
                                             .AllowAnyHeader()
                                             .AllowAnyMethod()
                                             .WithExposedHeaders("sessao", "usuario");
                      });
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.WebHost.UseKestrel()
    .UseUrls("http://*:5086") //Add this line
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseIISIntegration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    DatabaseController.IsDevelopment = true;
}

// app.UseHttpsRedirection();

app.UseCors(permissoesCorsNome);
app.UsarMiddlewaresCustomizados();

app.UseAuthorization();

app.MapControllers();

var services = app.Services.CreateScope().ServiceProvider;
if (ehBancoDadosRelacional) {
    var contexto = services.GetService<PontoFacilContexto>();
    await contexto.Database.MigrateAsync();
}
var configServico = services.GetService<ConfiguracoesServico>();
await GerenciadorAgendamento.Instancia(configServico);
ParametrosBatchExclusaoSessoes.UrlsServidor = new List<string>(builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey).Replace("*", "localhost").Split(';'));
var usuariosRepositorio = services.GetService<UsuariosRepositorio>();
await usuariosRepositorio.CriarUsuarioPeloCadastreSe(configServico.UsuarioImportarExportar);
var adminRaiz = await usuariosRepositorio.CriarUsuarioPeloCadastreSe(configServico.UsuarioAdminRaiz);
await usuariosRepositorio.TornaAdministrador(adminRaiz.id);

app.Run();