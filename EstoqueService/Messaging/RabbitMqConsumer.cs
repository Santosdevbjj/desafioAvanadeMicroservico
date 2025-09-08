using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using EstoqueService.Services;

namespace EstoqueService.Messaging;

public class RabbitMqConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMqConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

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

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var pedido = JsonSerializer.Deserialize<PedidoMessage>(message);

                if (pedido != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var produtoService = scope.ServiceProvider.GetRequiredService<ProdutoService>();

                    foreach (var item in pedido.Itens)
                    {
                        await produtoService.BaixarEstoque(item.ProdutoId, item.Quantidade);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao processar mensagem: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue: "pedidos",
                             autoAck: true,
                             consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
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
