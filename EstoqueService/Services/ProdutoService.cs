using EstoqueService.Models;
using EstoqueService.Repositories;

namespace EstoqueService.Services;

public class ProdutoService
{
    private readonly ProdutoRepository _repository;

    public ProdutoService(ProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Produto>> GetAll() => await _repository.GetAll();

    public async Task<Produto?> GetById(int id) => await _repository.GetById(id);

    public async Task Add(Produto produto) => await _repository.Add(produto);

    public async Task Update(Produto produto) => await _repository.Update(produto);

    public async Task Delete(int id) => await _repository.Delete(id);

    public async Task BaixarEstoque(int produtoId, int quantidade)
    {
        var produto = await _repository.GetById(produtoId);
        if (produto == null) throw new Exception("Produto não encontrado");

        if (produto.Quantidade < quantidade)
            throw new Exception("Estoque insuficiente");

        produto.Quantidade -= quantidade;
        await _repository.Update(produto);

        Console.WriteLine($"[Estoque] Produto {produtoId} atualizado. Novo estoque: {produto.Quantidade}");
    }
}
