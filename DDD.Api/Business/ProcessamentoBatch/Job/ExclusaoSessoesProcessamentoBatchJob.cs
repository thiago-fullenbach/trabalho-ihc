using DDD.Api.Business.Services.DataServices.OutsideDI;
using DDD.Api.Domain.Interface.Business.ProcessamentoBatch;
using DDD.Api.Domain.Interface.Business.Services.DataServices;
using DDD.Api.Domain.Interface.Business.Services.SchedulerService;
using Quartz;

namespace DDD.Api.Business.ProcessamentoBatch.Job;
public class ExclusaoSessoesProcessamentoBatchJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var app = WebApplicationDataServiceOutsideDI.GetInstance().GetWebApplication();
        using (var serviceScope = app.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            var serviceProviderDataService = services.GetRequiredService<IServiceProviderDataService>();
            serviceProviderDataService.SetUltimoServiceProvider(services);
            var exclusaoSessoesProcessamentoBatch = services.GetRequiredService<IExclusaoSessoesProcessamentoBatch>();
            await exclusaoSessoesProcessamentoBatch.ProcessarAsync();
        }
    }
}