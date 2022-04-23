namespace PontoFacil.Api.Controlador;
public class Mensagens
{
    public static readonly string ERRO_NEGOCIO_CABECALHO = "Atenção!\n";
    public static readonly string ERRO_NEGOCIO_ITEM_XXXX = "> {0}";
    public static readonly string XXXX_OBRIGATORIY = "{0} obrigatóri{1}";
    public static readonly string XXXX_INVALIDY = "{0} inválid{1}";
    public static readonly string XXXX_INVALIDY_MOTIVO_ZZZZ = "{0} inválid{1}: {2}";
    public static readonly string REQUISICAO_SUCESSO = "Requisição processada com sucesso.";
    public static readonly string FALHA_REQUISICAO = "Falha ao processar requisição.";
    public static readonly string ACESSO_NEGADO = "Acesso negado.";
    public static readonly string FALHA_RECUPERAR_XXXX_AO_CHAMAR_GETSERVICE_XXXX = "Não foi possível recuperar {0} ao chamar context.HttpContext.RequestServices.GetService<{0}}>();";
}