using DDD.Api.BackEndApp.InversionOfControl;
using DDD.Api.BackEndApp.v1.Middleware;
using DDD.Api.Business.Services.DataServices.OutsideDI;
using DDD.Api.Domain.Interface.Business.Services;
using DDD.Api.Domain.Interface.Business.Services.DataServices;
using DDD.Api.Domain.Interface.Business.Services.SchedulerService;
using DDD.Api.InversionOfControl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AdicionarModuloInfra();
builder.Services.AdicionarModuloBusiness();
builder.Services.AdicionarModuloBackEnd();
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
builder.Services.AddHttpContextAccessor();
string aspnetcoreUrls = builder.Configuration["ASPNETCORE_URLS"];
builder.WebHost.UseKestrel()
    .UseUrls(aspnetcoreUrls)
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseIISIntegration();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors(permissoesCorsNome);
app.Use(AlimentacaoServiceProviderMiddleware.ProcessarAsync);
app.Use(CargaMinimaMiddleware.ProcessarAsync);
app.Use(ExceptionMiddleware.ProcessarAsync);

app.UseAuthorization();

app.MapControllers();

WebApplicationDataServiceOutsideDI.GetInstance().SetWebApplication(app);
using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var serviceProviderDataService = services.GetRequiredService<IServiceProviderDataService>();
    serviceProviderDataService.SetUltimoServiceProvider(services);
    var schedulerService = services.GetRequiredService<ISchedulerService>();
    await schedulerService.ScheduleExcluirSessoesAsync();
}

app.Run();
