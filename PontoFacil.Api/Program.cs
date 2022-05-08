using Microsoft.EntityFrameworkCore;
using PontoFacil.Api;
using PontoFacil.Api.Batch;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo.Contexto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string conexao_mariaDb = builder.Configuration.GetConnectionString("MariaDb");
builder.Services.AdicionarContextoDeConexaoMySql(conexao_mariaDb);
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
                          policy.WithOrigins("http://localhost:3000")
                                             .AllowAnyHeader()
                                             .AllowAnyMethod()
                                             .WithExposedHeaders("sessao", "usuario");
                      });
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(permissoesCorsNome);
app.UsarMiddlewaresCustomizados();

app.UseAuthorization();

app.MapControllers();

var services = app.Services.CreateScope().ServiceProvider;
var contexto = services.GetService<PontoFacilContexto>();
await contexto.Database.MigrateAsync();
var configServico = services.GetService<ConfiguracoesServico>();
await GerenciadorAgendamento.Instancia(configServico);
var contextAccessor = services.GetService<IHttpContextAccessor>();
ParametrosBatchExclusaoSessoes.UrlsServidor = new List<string>(builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey).Split(';'));

app.Run();