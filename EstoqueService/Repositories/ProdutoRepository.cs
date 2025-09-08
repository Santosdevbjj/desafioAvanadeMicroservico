using EstoqueService.Data;
using EstoqueService.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueService.Repositories;

public class ProdutoRepository
{
    private readonly EstoqueContext _context;

    public ProdutoRepository(EstoqueContext context)
    {
        _context = context;
    }

    public async Task<List<Produto>> GetAll() => await _context.Produtos.ToListAsync();

    public async Task<Produto?> GetById(int id) =>
        await _context.Produtos.FindAsync(id);

    public async Task Add(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Produto produto)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto != null)
        {
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }
    }
}
