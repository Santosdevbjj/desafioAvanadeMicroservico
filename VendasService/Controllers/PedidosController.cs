using Microsoft.AspNetCore.Mvc;
using VendasService.Models;
using VendasService.Repositories;

namespace VendasService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly PedidoRepository _repository;

    public PedidosController(PedidoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _repository.GetAll());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Pedido pedido)
    {
        await _repository.Add(pedido);
        return CreatedAtAction(nameof(GetAll), new { id = pedido.Id }, pedido);
    }
}
