namespace VendasService.Models;

public class Pedido
{
    public int Id { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public DateTime Data { get; set; } = DateTime.UtcNow;
    public List<ItemPedido> Itens { get; set; } = new();
}
