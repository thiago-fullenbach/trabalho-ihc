using Quartz;

namespace PontoFacil.Api.Batch;
public class BatchExclusaoSessoesJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var batch = new ProcessadorExclusaoSessoes();
        await batch.Processar();
    }
}