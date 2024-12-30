using GestaoLoja.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestaoLoja.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ModoEntrega> ModosEntrega { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Configurações adicionais podem ser feitas aqui
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.categoria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaId);

            modelBuilder.Entity<Produto>()
                .HasOne(p => p.modoentrega)
                .WithMany(m => m.produtos)
                .HasForeignKey(p => p.ModoEntregaId);

            modelBuilder.Entity<Produto>()
            .Property(p => p.EmStock)
            .HasColumnType("decimal(18,2)");
        }
    }
}
