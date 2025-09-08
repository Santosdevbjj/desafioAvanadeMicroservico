using Microsoft.AspNetCore.Mvc;
using VendasService.Models;
using VendasService.Services;

namespace VendasService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly PedidoService _service;

    public PedidoController(PedidoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pedidos = await _service.GetAllAsync();
        return Ok(pedidos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pedido = await _service.GetByIdAsync(id);
        if (pedido == null) return NotFound();
        return Ok(pedido);
    }

    [HttpPost]
    public async Task<IActionResult> CriarPedido([FromBody] Pedido pedido)
    {
        var novoPedido = await _service.CriarPedidoAsync(pedido);
        return CreatedAtAction(nameof(GetById), new { id = novoPedido.Id }, novoPedido);
    }
}




