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
}
