using DDD.Api.Business.ProcessamentoBatch.Job;
using DDD.Api.Business.Services.DataServices;
using DDD.Api.Domain.Interface.Business.Services;
using DDD.Api.Domain.Interface.Business.Services.DataServices;
using DDD.Api.Domain.Interface.Business.Services.SchedulerService;
using DDD.Api.Domain.Interface.Infra.Configuration.BatchApp;
using Quartz;
using Quartz.Impl;

namespace DDD.Api.Business.Services.SchedulerService;
public class SchedulerService : ISchedulerService
{
    private readonly IServiceProviderDataService _serviceProviderDataService;
    private readonly SchedulerInfoDataService _schedulerInfoDataService;
    private readonly IBatchAppConfiguration _batchAppConfiguration;
    public SchedulerService(IServiceProviderDataService serviceProviderDataService,
                            SchedulerInfoDataService schedulerInfoDataService,
                            IBatchAppConfiguration batchAppConfiguration)
    {
        _serviceProviderDataService = serviceProviderDataService;
        _schedulerInfoDataService = schedulerInfoDataService;
        _batchAppConfiguration = batchAppConfiguration;
    }
    public async Task ScheduleExcluirSessoesAsync()
    {
        var autorizacaoService = _serviceProviderDataService.GetService<IAutorizacaoService>();
        var expiracaoMaisRecente = await autorizacaoService.ObterExpiracaoMaisRecenteSessaoAsync();
        if (_schedulerInfoDataService.Scheduler == null)
        {
            var factory = new StdSchedulerFactory();
            var scheduler = await factory.GetScheduler();
            _schedulerInfoDataService.Scheduler = scheduler;
            await scheduler.Start();
        }
        if (_schedulerInfoDataService.JobExclusaoSessoes == null)
        {
            _schedulerInfoDataService.JobExclusaoSessoes = JobBuilder.Create<ExclusaoSessoesProcessamentoBatchJob>()
                .WithIdentity("JobExclusaoSessoes", "GrupoJobExclusaoSessoes")
                .WithDescription("Exclui sessões expiradas")
                .StoreDurably(false)
                .RequestRecovery(false)
                .Build();
        }
        if (_schedulerInfoDataService.TriggerExclusaoSessoes != null)
        {
            await _schedulerInfoDataService.Scheduler.UnscheduleJob(_schedulerInfoDataService.TriggerExclusaoSessoes.Key);
        }
        var proximaExec = expiracaoMaisRecente ?? DateTime.Now + _batchAppConfiguration.GetTempoMaixmoDescanso();
        proximaExec = new DateTime(Math.Max(proximaExec.Ticks, (DateTime.Now + _batchAppConfiguration.GetTempoMinimoDescanso()).Ticks));
        var cronProximaExec = proximaExec.GetCronExpression();
        var trigger = TriggerBuilder.Create()
            .WithIdentity("TriggerJobExclusaoSessoes", "GrupoTriggerJobExclusaoSessoes")
            .WithDescription("Roda após sessão expirar, ou após inatividade por muito tempo")
            .WithCronSchedule(cronProximaExec)
            .ForJob(_schedulerInfoDataService.JobExclusaoSessoes.Key)
            .Build();
        _schedulerInfoDataService.TriggerExclusaoSessoes = trigger;
        await _schedulerInfoDataService.Scheduler.ScheduleJob(_schedulerInfoDataService.JobExclusaoSessoes, trigger);
    }
}