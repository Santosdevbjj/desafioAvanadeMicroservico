using VendasService.Models;
using VendasService.Messaging;

namespace VendasService.Services;

public class PedidoService
{
    private readonly List<Pedido> _pedidos = new();
    private readonly RabbitMqPublisher _publisher;

    public PedidoService(RabbitMqPublisher publisher)
    {
        _publisher = publisher;
    }

    public List<Pedido> GetAll() => _pedidos;

    public Pedido? GetById(int id) => _pedidos.FirstOrDefault(p => p.Id == id);

    public Pedido CriarPedido(Pedido pedido)
    {
        pedido.Id = _pedidos.Count + 1;
        pedido.Data = DateTime.UtcNow;
        _pedidos.Add(pedido);

        // Monta mensagem para RabbitMQ
        var pedidoMsg = new PedidoMessage
        {
            PedidoId = pedido.Id,
            Itens = pedido.Itens.Select(i => new ItemPedidoMessage
            {
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade
            }).ToList()
        };

        _publisher.PublicarPedido(pedidoMsg);

        return pedido;
    }
}
