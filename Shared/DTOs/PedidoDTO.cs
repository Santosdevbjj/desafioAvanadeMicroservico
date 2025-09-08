namespace Shared.DTOs;

public class PedidoDTO
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public DateTime DataPedido { get; set; }
    public string Status { get; set; } = "Pendente";
}
