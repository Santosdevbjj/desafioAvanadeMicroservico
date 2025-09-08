namespace VendasService.Models;

public class Pedido
{
    public int Id { get; set; }
    public DateTime Data { get; set; } = DateTime.Now;
    public List<PedidoItem> Itens { get; set; } = new();
}
