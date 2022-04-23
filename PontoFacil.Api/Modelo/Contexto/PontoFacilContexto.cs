using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Modelo;

namespace PontoFacil.Api.Modelo.Contexto;
public class PontoFacilContexto : DbContext
{
    public PontoFacilContexto(DbContextOptions<PontoFacilContexto> options)
    : base(options)
    {
    }
    public DbSet<Recursos> Recursos { get; set; }
    public DbSet<Sessoes> Sessoes { get; set; }
    public DbSet<Usuarios> Usuarios { get; set; }
    public DbSet<Apelidos> Apelidos { get; set; }
    public DbSet<Locais> Locais { get; set; }
    public DbSet<TiposLogradouro> TiposLogradouro { get; set; }
    public DbSet<Presencas> Presencas { get; set; }
    public DbSet<Ajustes> Ajustes { get; set; }
    public DbSet<TiposAjuste> TiposAjuste { get; set; }
    public DbSet<Cidades> Cidades { get; set; }
    public DbSet<UFs> UFs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recursos>(recursos => {
            recursos.HasKey(x => x.id);
            recursos.HasOne(x => x.NavegacaoUsuarios).WithOne(y => y.NavegacaoRecursos).HasForeignKey("Recursos", "usuarios_id");
        });
        modelBuilder.Entity<Sessoes>(sessoes => {
            sessoes.HasKey(x => x.id);
            sessoes.HasOne(x => x.NavegacaoUsuarios).WithMany(y => y.NavegacaoMuitosSessoes).HasForeignKey(x => x.usuarios_id);
        });
        modelBuilder.Entity<Usuarios>(usuarios => {
            usuarios.HasKey(x => x.id);
        });
        modelBuilder.Entity<Apelidos>(apelidos => {
            apelidos.HasKey(x => new { x.usuarios_id, x.locais_id });
            apelidos.HasOne(x => x.NavegacaoUsuarios).WithMany(y => y.NavegacaoMuitosApelidos).HasForeignKey(x => x.usuarios_id);
            apelidos.HasOne(x => x.NavegacaoLocais).WithMany(y => y.NavegacaoMuitosApelidos).HasForeignKey(x => x.locais_id);
        });
        modelBuilder.Entity<Locais>(locais => {
            locais.HasKey(x => x.id);
            locais.HasOne(x => x.NavegacaoTiposLogradouro).WithMany(y => y.NavegacaoMuitosLocais).HasForeignKey(x => x.tiposLogradouro_id);
            locais.HasOne(x => x.NavegacaoCidades).WithMany(y => y.NavegacaoMuitosLocais).HasForeignKey(x => x.cidades_id);
        });
        modelBuilder.Entity<TiposLogradouro>(tiposLogradouro => {
            tiposLogradouro.HasKey(x => x.id);
        });
        modelBuilder.Entity<Presencas>(presencas => {
            presencas.HasKey(x => x.id);
            presencas.HasOne(x => x.NavegacaoUsuarios).WithMany(y => y.NavegacaoMuitosPresencas).HasForeignKey(x => x.usuarios_id);
            presencas.HasOne(x => x.NavegacaoLocais).WithMany(y => y.NavegacaoMuitosPresencas).HasForeignKey(x => x.locais_id);
        });
        modelBuilder.Entity<Ajustes>(ajustes => {
            ajustes.HasKey(x => x.id);
            ajustes.HasOne(x => x.NavegacaoUsuariosModificador).WithMany(y => y.NavegacaoMuitosAjustes).HasForeignKey(x => x.usuarios_id_modificador);
            ajustes.HasOne(x => x.NavegacaoPresencas).WithOne(y => y.NavegacaoAjustes).HasForeignKey("Ajustes", "presencas_id");
            ajustes.HasOne(x => x.NavegacaoLocaisAjuste).WithMany(y => y.NavegacaoMuitosAjustes).HasForeignKey(x => x.locais_id_ajuste);
            ajustes.HasOne(x => x.NavegacaoTiposAjuste).WithMany(y => y.NavegacaoMuitosAjustes).HasForeignKey(x => x.tipo_ajuste);
        });
        modelBuilder.Entity<TiposAjuste>(tiposAjuste => {
            tiposAjuste.HasKey(x => x.id);
        });
        modelBuilder.Entity<Cidades>(cidades => {
            cidades.HasKey(x => x.id);
            cidades.HasOne(x => x.NavegacaoUFs).WithMany(y => y.NavegacaoMuitosCidades).HasForeignKey(x => x.ufs_id);
        });
        modelBuilder.Entity<UFs>(ufs => {
            ufs.HasKey(x => x.id);
        });
    }
}
