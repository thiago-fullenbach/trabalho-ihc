using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Modelo;

namespace PontoFacil.Api.Modelo.Contexto;
public class PontoFacilContexto : DbContext
{
    public PontoFacilContexto(DbContextOptions<PontoFacilContexto> options)
    : base(options)
    {
    }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Acesso> Acessos { get; set; }
    public DbSet<Sessao> Sessoes { get; set; }
    public DbSet<Local> Locais { get; set; }
    public DbSet<Presenca> Presencas { get; set; }
    public DbSet<Ajuste> Ajustes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(usuario => {
            usuario.HasKey(x => x.id);
        });
        modelBuilder.Entity<Acesso>(acesso => {
            acesso.HasKey(x => new { x.usuario_id, x.recurso_cod_en });
            acesso.HasOne(x => x.NavegacaoUsuario).WithMany(y => y.NavegacaoMuitosAcessos).HasForeignKey(x => x.usuario_id);
        });
        modelBuilder.Entity<Sessao>(sessao => {
            sessao.HasKey(x => x.id);
            sessao.HasOne(x => x.NavegacaoUsuario).WithMany(y => y.NavegacaoMuitasSessoes).HasForeignKey(x => x.usuario_id);
        });
        modelBuilder.Entity<Local>(local => {
            local.HasKey(x => x.id);
        });
        modelBuilder.Entity<Presenca>(presenca => {
            presenca.HasKey(x => x.id);
            presenca.HasOne(x => x.NavegacaoUsuario).WithMany(y => y.NavegacaoMuitasPresencas).HasForeignKey(x => x.usuario_id);
            presenca.HasOne(x => x.NavegacaoLocal).WithMany(y => y.NavegacaoMuitasPresencas).HasForeignKey(x => x.local_id);
        });
        modelBuilder.Entity<Ajuste>(ajuste => {
            ajuste.HasKey(x => x.id);
            ajuste.HasOne(x => x.NavegacaoUsuarioAjustador).WithMany(y => y.NavegacaoMuitosAjustesRegistrados).HasForeignKey(x => x.usuario_ajustador_id);
            ajuste.HasOne(x => x.NavegacaoPresenca).WithOne(y => y.NavegacaoAjuste).HasForeignKey("Ajuste", "presenca_id");
            ajuste.HasOne(x => x.NavegacaoUsuarioPresente).WithMany(y => y.NavegacaoMuitosAjustesPresentes).HasForeignKey(x => x.usuario_presente_id);
            ajuste.HasOne(x => x.NavegacaoLocal).WithMany(y => y.NavegacaoMuitosAjustes).HasForeignKey(x => x.local_id);
        });
    }
}
