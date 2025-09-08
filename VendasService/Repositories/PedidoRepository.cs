using Microsoft.EntityFrameworkCore;
using VendasService.Data;
using VendasService.Models;

namespace VendasService.Repositories
{
    /// <summary>
    /// Repositório para operações CRUD de Pedido.
    /// Usa AppDbContext (VendasService/Data/VendasContext.cs).
    /// </summary>
    public interface IPedidoRepository
    {
        Task<Pedido> AddAsync(Pedido pedido, CancellationToken ct = default);
        Task<Pedido?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<Pedido>> GetAllAsync(CancellationToken ct = default);
        Task UpdateAsync(Pedido pedido, CancellationToken ct = default);
    }

    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido> AddAsync(Pedido pedido, CancellationToken ct = default)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync(ct);
            return pedido;
        }

        public async Task<Pedido?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<List<Pedido>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .ToListAsync(ct);
        }

        public async Task UpdateAsync(Pedido pedido, CancellationToken ct = default)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync(ct);
        }
    }
}
