using EstoqueService.Models;
using EstoqueService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstoqueController : ControllerBase
    {
        private readonly IProdutoRepository _repository;

        public EstoqueController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var produtos = await _repository.GetAllAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var produto = await _repository.GetByIdAsync(id);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Produto produto)
        {
            await _repository.AddAsync(produto);
            await _repository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Produto produto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Nome = produto.Nome;
            existing.Quantidade = produto.Quantidade;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
    }
}
