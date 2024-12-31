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
        public DbSet<Encomenda> Encomendas { get; set; }
        public DbSet<ItemEncomenda> ItensEncomenda { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Configurações adicionais podem ser feitas aqui
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaId);

            modelBuilder.Entity<Produto>()
                .HasOne(p => p.ModoEntrega)
                .WithMany(m => m.Produtos)
                .HasForeignKey(p => p.ModoEntregaId);

            modelBuilder.Entity<Produto>()
            .Property(p => p.EmStock)
            .HasColumnType("decimal(18,2)");

            // Relação entre ItemVenda e Produto
            modelBuilder.Entity<ItemEncomenda>()
                .HasOne(iv => iv.Produto)
                .WithMany(p => p.ItensEncomenda)
                .HasForeignKey(iv => iv.ProdutoId);

            // Relação entre ItemVenda e Venda
            modelBuilder.Entity<ItemEncomenda>()
                .HasOne(iv => iv.Encomenda)
                .WithMany(v => v.ItensEncomenda)
                .HasForeignKey(iv => iv.EncomendaId);

            // Relação entre ItemVenda e Produto
            modelBuilder.Entity<ItemEncomenda>()
                .HasKey(iv => iv.ItemEncomendaId); // Definindo a chave primária


            modelBuilder.Entity<Encomenda>(entity =>
            {
                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18,2)");

                // Outras configurações da entidade Encomenda
            });

            modelBuilder.Entity<ItemEncomenda>(entity =>
            {
                entity.Property(e => e.PrecoUnitario)
                    .HasColumnType("decimal(18,2)");

                // Outras configurações da entidade ItemEncomenda
            });

            // Relação entre Venda e ModoEntrega
            modelBuilder.Entity<Encomenda>()
                .HasOne(v => v.ModoEntrega)
                .WithMany(m => m.Encomendas)
                .HasForeignKey(v => v.ModoEntregaId);

            // Relação entre Venda e ApplicationUser (usuário)
            modelBuilder.Entity<Encomenda>()
                .HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.ClienteId);
        }
    }
}
