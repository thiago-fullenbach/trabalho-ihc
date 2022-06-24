using Quartz;

namespace DDD.Api.Business.Services.DataServices;
public class SchedulerInfoDataService
{
    public IScheduler? Scheduler { get; set; }
    public IJobDetail? JobExclusaoSessoes { get; set; }
    public ITrigger? TriggerExclusaoSessoes { get; set; }
}