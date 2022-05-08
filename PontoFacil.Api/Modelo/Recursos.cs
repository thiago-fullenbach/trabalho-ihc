namespace PontoFacil.Api.Modelo;
public class Recursos
{
    public int id { get; set; }
    public int usuarios_id { get; set; }
    public bool? pode_vis_usuario { get; set; }
    public bool? pode_cad_usuario { get; set; }
    public bool? pode_vis_demais_usuarios { get; set; }
    public bool? pode_cad_demais_usuarios { get; set; }
    public bool? pode_cad_administrador_acessos { get; set; }
    public bool? pode_vis_ponto { get; set; }
    public bool? pode_cad_ponto { get; set; }
    public bool? pode_vis_ponto_demais_usuarios { get; set; }
    public bool? pode_vis_ajuste { get; set; }
    public bool? pode_vis_ajuste_demais_usuarios { get; set; }
    public bool? pode_cad_ajuste_demais_usuarios { get; set; }
    public DateTime datahora_criacao { get; set; }
    public DateTime? datahora_modificacao { get; set; }
    public virtual Usuarios NavegacaoUsuarios { get; set; }
}