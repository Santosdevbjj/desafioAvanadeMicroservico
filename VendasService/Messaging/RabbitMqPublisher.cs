using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace VendasService.Messaging;

public class RabbitMqPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqPublisher()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "pedidos",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void PublicarPedido(PedidoMessage pedido)
    {
        var message = JsonSerializer.Serialize(pedido);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
                             routingKey: "pedidos",
                             basicProperties: null,
                             body: body);

        Console.WriteLine($"[RabbitMQ] Pedido {pedido.PedidoId} publicado com {pedido.Itens.Count} itens.");
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}

public class PedidoMessage
{
    public int PedidoId { get; set; }
    public List<ItemPedidoMessage> Itens { get; set; } = new();
}

public class ItemPedidoMessage
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}
