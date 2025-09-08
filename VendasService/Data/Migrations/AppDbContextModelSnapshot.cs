using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using VendasService.Data;

#nullable disable

namespace VendasService.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("VendasService.Models.Pedido", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Cliente").IsRequired().HasMaxLength(200);
                b.Property<DateTime>("Data");
                b.HasKey("Id");
                b.ToTable("Pedidos");
            });

            modelBuilder.Entity("VendasService.Models.ItemPedido", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<int>("ProdutoId");
                b.Property<int>("Quantidade");
                b.Property<int>("PedidoId");
                b.HasKey("Id");
                b.HasIndex("PedidoId");
                b.ToTable("ItensPedido");
            });
        }
    }
}
