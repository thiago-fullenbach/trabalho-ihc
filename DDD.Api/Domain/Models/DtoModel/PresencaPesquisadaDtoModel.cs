namespace DDD.Api.Domain.Models.DtoModel;
public class PresencaPesquisadaDtoModel
{
    public int Id { get; set; }
    public int Usuario_id { get; set; }
    public string Usuario_nome { get; set; } = string.Empty;
    public int Usuario_horas_diarias { get; set; }
    public bool Eh_entrada { get; set; }
    public DateTime Datahora_presenca { get; set; }
    public string Local_resumo { get; set; } = string.Empty;
    public bool Tem_visto { get; set; }
}