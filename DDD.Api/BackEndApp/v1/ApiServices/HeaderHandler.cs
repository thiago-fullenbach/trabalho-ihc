using System.Net;
using System.Text.Json;
using DDD.Api.Business.Adapter.DtoModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.BusnModelAdapter;
using DDD.Api.Domain.Interface.Business.IntegratedAdapter.DtoModelAdapter;
using DDD.Api.Domain.Interface.Business.Services.DataServices;
using DDD.Api.Domain.Models.BusnModel.Exception;
using DDD.Api.Domain.Models.DtoModel;

namespace DDD.Api.BackEndApp.v1.ApiServices;
public class HeaderHandler
{
    private readonly ISessaoAutenticadaDataService _sessaoAutenticadaDataService;
    private readonly ISessaoBusnModelIntegratedAdapter _sessaoBusnModelIntegratedAdapter;
    private readonly IUsuarioLogadoDtoModelIntegratedAdapter _usuarioLogadoDtoModelIntegratedAdapter;
    public HeaderHandler(ISessaoAutenticadaDataService sessaoAutenticadaDataService,
                         ISessaoBusnModelIntegratedAdapter sessaoBusnModelIntegratedAdapter,
                         IUsuarioLogadoDtoModelIntegratedAdapter usuarioLogadoDtoModelIntegratedAdapter)
    {
        _sessaoAutenticadaDataService = sessaoAutenticadaDataService;
        _sessaoBusnModelIntegratedAdapter = sessaoBusnModelIntegratedAdapter;
        _usuarioLogadoDtoModelIntegratedAdapter = usuarioLogadoDtoModelIntegratedAdapter;
    }
    public async Task EscreverNoDataServiceAsync(IHeaderDictionary headerDictionary)
    {
        var mensagens = new List<string>();
        if (!headerDictionary.ContainsKey("sessao"))
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                MensagensErro = new List<string> { "Usuário não logado." }
            };
        }
        string sessaoJson = headerDictionary["sessao"];
        if (string.IsNullOrWhiteSpace(sessaoJson))
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                MensagensErro = new List<string> { "Usuário não logado." }
            };
        }
        var sessao = JsonSerializer.Deserialize<SessaoEnvioHeaderDtoModel>(sessaoJson);
        if (sessao == null || sessao.Id == null || sessao.Hex_verificacao == null)
        {
            throw new BusinessException
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                MensagensErro = new List<string> { "Usuário não logado." }
            };
        }
        var sessaoBusnModel = await _sessaoBusnModelIntegratedAdapter.ToSessaoAutenticadaJaExistenteAsync(sessao);
        _sessaoAutenticadaDataService.SetSessaoAutenticada(sessaoBusnModel);
    }
    public void EscreverNoHeader(IHeaderDictionary headerDictionary)
    {
        if (!_sessaoAutenticadaDataService.TemSessaoAutenticada())
        {
            return;
        }
        var sessao = _sessaoAutenticadaDataService.GetSessaoAutenticada();
        SessaoEnvioHeaderDtoModel sessaoEnvioHeader = new SessaoEnvioHeaderDtoModelAdapter(sessao);
        var usuarioLogadoDto = _usuarioLogadoDtoModelIntegratedAdapter.ToUsuarioLogadoDtoModelSessao(sessao.UsuarioAutenticado);
        headerDictionary["sessao"] = JsonSerializer.Serialize(sessaoEnvioHeader);
        headerDictionary["usuario"] = JsonSerializer.Serialize(usuarioLogadoDto);
    }
}
