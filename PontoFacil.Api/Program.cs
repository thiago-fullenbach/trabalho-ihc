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


builder.AdicionarContextoDeConexao();
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
builder.ExporUrlsForaContainer();

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

var processador = new PrimeirasExecucoes(builder, app);
await processador.MigraBancoDadosSeRelacionalAsync();
await processador.CarregaBatchExclusaoSessoesAsync();
await processador.CarregaDadosIniciaisAsync();

app.Run();