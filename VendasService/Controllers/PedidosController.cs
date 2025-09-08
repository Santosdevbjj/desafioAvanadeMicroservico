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
    public IActionResult GetAll() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var pedido = _service.GetById(id);
        if (pedido == null) return NotFound();
        return Ok(pedido);
    }

    [HttpPost]
    public IActionResult CriarPedido([FromBody] Pedido pedido)
    {
        var novoPedido = _service.CriarPedido(pedido);
        return CreatedAtAction(nameof(GetById), new { id = novoPedido.Id }, novoPedido);
    }
}
