using Microsoft.EntityFrameworkCore;
using PontoFacil.Api;
using PontoFacil.Api.Modelo.Contexto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string conexao_mariaDb = builder.Configuration.GetConnectionString("MariaDb");
builder.Services.AddDbContext<PontoFacilContext>(options => {
    options.UseMySql(conexao_mariaDb, ServerVersion.AutoDetect(conexao_mariaDb));
});
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
                                             .WithExposedHeaders("sessao");
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(permissoesCorsNome);

app.UseAuthorization();

app.MapControllers();

app.Run();
