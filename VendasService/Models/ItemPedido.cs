using System.ComponentModel.DataAnnotations;

namespace VendasService.Models;

public class ItemPedido
{
    [Key]
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }

    // Chave estrangeira
    public int PedidoId { get; set; }
}

