using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using EstoqueService.Data;

#nullable disable

namespace EstoqueService.Migrations
{
    [DbContext(typeof(EstoqueContext))]
    partial class EstoqueContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EstoqueService.Models.Produto", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                b.Property<string>("Nome")
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                b.Property<string>("Descricao")
                    .HasColumnType("varchar(500)");

                b.Property<decimal>("Preco")
                    .HasColumnType("decimal(18,2)");

                b.Property<int>("Quantidade")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.ToTable("Produtos");
            });
#pragma warning restore 612, 618
        }
    }
}
