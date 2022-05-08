namespace PontoFacil.Api.Controlador.ExposicaoDeEndpoints.v1.DTO.DoServidorParaCliente;
public class UsuarioRecursoDTO
{
    public bool? Pode_vis_usuario { get; set; }
    public bool? Pode_cad_usuario { get; set; }
    public bool? Pode_vis_demais_usuarios { get; set; }
    public bool? Pode_cad_demais_usuarios { get; set; }
    public bool? Pode_cad_administrador_acessos { get; set; }
    public bool? Pode_vis_ponto { get; set; }
    public bool? Pode_cad_ponto { get; set; }
    public bool? Pode_vis_ponto_demais_usuarios { get; set; }
    public bool? Pode_vis_ajuste { get; set; }
    public bool? Pode_vis_ajuste_demais_usuarios { get; set; }
    public bool? Pode_cad_ajuste_demais_usuarios { get; set; }
}