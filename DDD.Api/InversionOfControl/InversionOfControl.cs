using DDD.Api.Business.IntegratedAdapter.BusnModelAdapter;
using DDD.Api.Business.IntegratedAdapter.DtoModelAdapter;
using DDD.Api.Business.Services;
using DDD.Api.Business.Services.DataServices;
using DDD.Api.Business.Services.MicroService;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.DtoModelAdapter;
using DDD.Api.Domain.Interface.Business.Services;
using DDD.Api.Domain.Interface.Business.Services.DataServices;
using DDD.Api.Domain.Interface.Business.Services.MicroService;
using DDD.Api.Domain.Interface.Infra.Configuration.BackEndApp;
using DDD.Api.Domain.Interface.Infra.Configuration.BatchApp;
using DDD.Api.Domain.Interface.Infra.Configuration.Database;
using DDD.Api.Domain.Interface.Infra.Repositories;
using DDD.Api.Domain.Interface.Infra.UnitOfWork;
using DDD.Api.Infra.Configuration.BackEndApp;
using DDD.Api.Infra.Configuration.BatchApp;
using DDD.Api.Infra.Configuration.Database;
using DDD.Api.Infra.Repositories;
using DDD.Api.Infra.UnitOfWork;

namespace DDD.Api.InversionOfControl;
public static class InversionOfControl
{
    public static IServiceCollection AdicionarModuloInfra(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseConfiguration, MongoDatabaseConfiguration>();
        services.AddSingleton<IBackEndAppConfiguration, BackEndAppConfiguration>();
        services.AddSingleton<IBatchAppConfiguration, BatchAppConfiguration>();
        services.AddScoped<MongoDbConnection>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ISessaoRepository, SessaoRepository>();
        services.AddScoped<IAcessoRepository, AcessoRepository>();
        services.AddScoped<ILocalRepository, LocalRepository>();
        services.AddScoped<IPresencaRepository, PresencaRepository>();
        return services;
    }

    public static IServiceCollection AdicionarModuloBusiness(this IServiceCollection services)
    {
        services.AddScoped<ISessaoBusnModelIntegratedAdapter, SessaoBusnModelIntegratedAdapter>();
        services.AddScoped<IUsuarioBusnModelIntegratedAdapter, UsuarioBusnModelIntegratedAdapter>();
        services.AddScoped<IDetalheUsuarioDtoModelIntegratedAdapter, DetalheUsuarioDtoModelIntegratedAdapter>();
        services.AddScoped<IUsuarioLogadoDtoModelIntegratedAdapter, UsuarioLogadoDtoModelIntegratedAdapter>();
        services.AddScoped<IUsuarioPesquisadoDtoModelIntegratedAdapter, UsuarioPesquisadoDtoModelIntegratedAdapter>();
        services.AddScoped<ISessaoAutenticadaDataService, SessaoAutenticadaDataService>();
        services.AddScoped<IMicroService, MicroService>();
        services.AddScoped<IAutorizacaoService, AutorizacaoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        return services;
    }
}