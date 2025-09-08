using EstoqueService.Data;
using EstoqueService.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueService.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly EstoqueContext _context;

        public ProdutoRepository(EstoqueContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto?> GetByIdAsync(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task AddAsync(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
        }

        public async Task UpdateAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
        }

        public async Task DeleteAsync(int id)
        {
            var produto = await GetByIdAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
