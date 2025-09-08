using Microsoft.EntityFrameworkCore;
using VendasService.Models;

namespace VendasService.Data
{
    /// <summary>
    /// Contexto EF Core para o microserviço de Vendas.
    /// Arquivo: VendasService/Data/VendasContext.cs
    /// Classe: AppDbContext (mantida para compatibilidade)
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Tabelas
        public DbSet<Pedido> Pedidos { get; set; } = null!;
        public DbSet<ItemPedido> ItensPedido { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações simples de mapeamento
            modelBuilder.Entity<Pedido>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.Cliente).IsRequired().HasMaxLength(200);
                b.Property(p => p.Data).IsRequired();
                b.HasMany(p => p.Itens)
                 .WithOne()
                 .HasForeignKey(i => i.PedidoId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ItemPedido>(b =>
            {
                b.HasKey(i => i.Id);
                b.Property(i => i.ProdutoId).IsRequired();
                b.Property(i => i.Quantidade).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
