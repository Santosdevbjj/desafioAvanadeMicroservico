using VendasService.Models;
using VendasService.Data;
using VendasService.Messaging;
using Microsoft.EntityFrameworkCore;

namespace VendasService.Services;

public class PedidoService
{
    private readonly AppDbContext _context;
    private readonly RabbitMqPublisher _publisher;

    public PedidoService(AppDbContext context, RabbitMqPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<List<Pedido>> GetAllAsync()
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .ToListAsync();
    }

    public async Task<Pedido?> GetByIdAsync(int id)
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pedido> CriarPedidoAsync(Pedido pedido)
    {
        pedido.Data = DateTime.UtcNow;

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Publica no RabbitMQ
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



