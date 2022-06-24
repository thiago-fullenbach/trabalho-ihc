using DDD.Api.Domain.Interface.Business.ProcessamentoBatch;
using DDD.Api.Domain.Interface.Business.Services;

namespace DDD.Api.Business.ProcessamentoBatch;
public class ExclusaoSessoesProcessamentoBatch : IExclusaoSessoesProcessamentoBatch
{
    private readonly IAutorizacaoService _autorizacaoService;
    public ExclusaoSessoesProcessamentoBatch(IAutorizacaoService autorizacaoService)
    {
        _autorizacaoService = autorizacaoService;
    }
    public async Task ProcessarAsync()
    {
        await _autorizacaoService.ExcluirSessoesExpiradasAsync();
    }
}