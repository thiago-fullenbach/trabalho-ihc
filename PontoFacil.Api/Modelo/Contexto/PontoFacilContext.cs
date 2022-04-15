using Microsoft.EntityFrameworkCore;
using PontoFacil.Api.Modelo;

namespace PontoFacil.Api.Modelo.Contexto;
public class PontoFacilContext : DbContext
{
    public PontoFacilContext(DbContextOptions<PontoFacilContext> options)
    : base(options)
    {
    }

    public DbSet<Acesso> Acesso { get; set; }
    public DbSet<Conta> Conta { get; set; }
    public DbSet<Recurso> Recurso { get; set; }
    public DbSet<SessaoAberta> SessaoAberta { get; set; }
    public DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelB)
    {
        modelB.Entity<Acesso>(acesso => {
            acesso.HasKey(x => new { x.IdRecurso, x.IdUsuario } );
            acesso.HasOne(x => x.IdRecursoNavegacao)
                .WithMany(y => y.AcessoMuitosNavegacao)
                .HasForeignKey(x => x.IdRecurso);
            acesso.HasOne(x => x.IdUsuarioNavegacao)
                .WithMany(y => y.AcessoMuitosNavegacao)
                .HasForeignKey(x => x.IdUsuario);
        });
        modelB.Entity<Conta>(conta => {
            conta.HasKey(x => x.Id);
            conta.HasOne(x => x.IdUsuarioNavegacao)
                .WithMany(y => y.ContaMuitosNavegacao)
                .HasForeignKey(x => x.IdUsuario);
        });
        modelB.Entity<Recurso>(recurso => {
            recurso.HasKey(x => x.Id);
        });
        modelB.Entity<SessaoAberta>(sessaoAberta => {
            sessaoAberta.HasKey(x => x.Id);
            sessaoAberta.HasOne(x => x.IdUsuarioNavegacao)
                .WithMany(y => y.SessaoAbertaMuitosNavegacao)
                .HasForeignKey(x => x.IdUsuario);
        });
        modelB.Entity<Usuario>(usuario => {
            usuario.HasKey(x => x.Id);
        });
    }
}
