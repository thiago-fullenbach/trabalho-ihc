using System.Collections.ObjectModel;
using System.Reflection;
using PontoFacil.Api.Controlador.Repositorio.Comum;
using PontoFacil.Api.Controlador.Servico;
using PontoFacil.Api.Modelo;
using Quartz;
using Quartz.Impl;

namespace PontoFacil.Api.Batch;
public class GerenciadorAgendamento
{
    private static GerenciadorAgendamento _instancia;
    public static async Task<GerenciadorAgendamento> Instancia(ConfiguracoesServico configuracoesServico)
    {
        if (_instancia == null)
            { _instancia = await Instanciar(configuracoesServico); }
        return _instancia;
    }
    private static async Task<GerenciadorAgendamento> Instanciar(ConfiguracoesServico configuracoesServico)
    {
        var instancia = new GerenciadorAgendamento();
        var factory = new StdSchedulerFactory();
        var scheduler = await factory.GetScheduler();
        var batchExclusaoSessoesJob = JobBuilder.Create<BatchExclusaoSessoesJob>()
            .WithIdentity("BatchExclusaoSessoesJob", "BatchExclusaoSessoesJobGroup")
            .WithDescription("Faz uma requisição POST em /Autorizacao/excluirSessoesExpiradas")
            .StoreDurably(true)
            .RequestRecovery(false)
            .Build();
        Utilitarios.SetFieldPorReflection(instancia, "_datahoraProximoBatchExclusaoSessoes", CalcularDataHoraExclusaoAposInatividade(configuracoesServico));
        Utilitarios.SetFieldPorReflection(instancia, "_schedulerFactory", factory);
        Utilitarios.SetFieldPorReflection(instancia, "_scheduler", scheduler);
        Utilitarios.SetFieldPorReflection(instancia, "_batchExclusaoSessoesJob", batchExclusaoSessoesJob);
        var datahoraExclusaoInatividade = CalcularDataHoraExclusaoAposInatividade(configuracoesServico);
        await scheduler.Start();
        await instancia.AgendarBatchExclusaoSessoesPara(datahoraExclusaoInatividade);
        return instancia;
    }
    private DateTime _datahoraProximoBatchExclusaoSessoes;
    private ISchedulerFactory _schedulerFactory;
    private IScheduler _scheduler;
    private IJobDetail _batchExclusaoSessoesJob;
    public async Task DefineProximoBatchExclusaoSessoes(IEnumerable<Sessao> sessoes, ConfiguracoesServico configuracoesServico)
    {
        DateTime? sessoesDatahoraProximaColeta = null;
        if (sessoes.Count() > 0)
            { sessoesDatahoraProximaColeta = sessoes.Select(x => GerenciadorAgendamento.CalcularDatahoraExclusao(x, configuracoesServico)).Min(); }
        var dataAgr = DateTime.Now;
        if (sessoesDatahoraProximaColeta.HasValue && sessoesDatahoraProximaColeta < dataAgr + configuracoesServico.TempoBatchExclusaoSessoesDescanso)
            { sessoesDatahoraProximaColeta = dataAgr + configuracoesServico.TempoBatchExclusaoSessoesDescanso; }
        if (!sessoesDatahoraProximaColeta.HasValue || sessoesDatahoraProximaColeta > GerenciadorAgendamento.CalcularDataHoraExclusaoAposInatividade(configuracoesServico))
            { sessoesDatahoraProximaColeta = GerenciadorAgendamento.CalcularDataHoraExclusaoAposInatividade(configuracoesServico); }
        await ReagendarBatchExclusaoSessoesPara(sessoesDatahoraProximaColeta.Value);
    }
    public static DateTime CalcularDatahoraExclusao(Sessao sessao, ConfiguracoesServico configuracoesServico)
    {
        return (sessao.datahora_ultima_autenticacao + configuracoesServico.TempoExpirarSessao) + TimeSpan.FromSeconds(1);
    }
    public static DateTime CalcularDataHoraExclusaoAposInatividade(ConfiguracoesServico configuracoesServico)
    {
        return DateTime.Now + configuracoesServico.TempoBatchExclusaoSessoesInatividadeMaxima;
    }
    public async Task ReagendarBatchExclusaoSessoesPara(DateTime datahoraBatch)
    {
        await CancelarBatchExclusaoSessoes();
        await AgendarBatchExclusaoSessoesPara(datahoraBatch);
    }
    public async Task AgendarBatchExclusaoSessoesPara(DateTime datahoraBatch)
    {
        var cronExclusao = Utilitarios.ParaExpressaoCron(datahoraBatch);
        var batchExclusaoSessoesTrigger = TriggerBuilder.Create()
            .WithIdentity("BatchExclusaoSessoesTrigger", "BatchExclusaoSessoesTriggerGroup")
            .WithDescription("Roda após sessão expirar, ou após inatividade por muito tempo")
            .WithCronSchedule(cronExclusao)
            .ForJob(_batchExclusaoSessoesJob.Key)
            .Build();
        await _scheduler.ScheduleJob(_batchExclusaoSessoesJob, batchExclusaoSessoesTrigger);
    }
    public async Task CancelarBatchExclusaoSessoes()
    {
        var triggeres = await _scheduler.DeleteJob(_batchExclusaoSessoesJob.Key);
    }
}