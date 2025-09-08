using Moq;
using Xunit;
using VendasService.Repositories;
using VendasService.Models;
using VendasService.Services;

namespace VendasService.Tests;

public class PedidoServiceTests
{
    [Fact]
    public async Task Deve_Criar_Pedido()
    {
        var mockRepo = new Mock<IPedidoRepository>();
        var pedidoService = new PedidoService(mockRepo.Object);

        var pedido = new Pedido
        {
            ProdutoId = 1,
            Quantidade = 2,
            DataPedido = DateTime.Now
        };

        await pedidoService.CriarPedido(pedido);

        mockRepo.Verify(r => r.AddAsync(It.IsAny<Pedido>()), Times.Once);
    }
}
