using Microsoft.AspNetCore.Mvc;
using EstoqueService.Models;
using EstoqueService.Repositories;

namespace EstoqueService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly ProdutoRepository _repository;

    public ProdutosController(ProdutoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _repository.GetAll());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Produto produto)
    {
        await _repository.Add(produto);
        return CreatedAtAction(nameof(GetAll), new { id = produto.Id }, produto);
    }
}
