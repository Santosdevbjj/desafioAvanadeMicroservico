using VendasService.Models;
using VendasService.Repositories;

namespace VendasService.Services
{
    /// <summary>
    /// Serviço de regras de negócio para pedidos.
    /// Agora depende de IPedidoRepository (em vez de DbContext direto).
    /// </summary>
    public class PedidoService
    {
        private readonly IPedidoRepository _repository;
        private readonly RabbitMqPublisher _publisher;

        public PedidoService(IPedidoRepository repository, RabbitMqPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<Pedido> CriarPedidoAsync(Pedido pedido)
        {
            pedido.Data = DateTime.UtcNow;

            // Salva no banco
            var pedidoCriado = await _repository.AddAsync(pedido);

            // Publica no RabbitMQ (para EstoqueService consumir)
            _publisher.PublishPedidoCriado(pedidoCriado);

            return pedidoCriado;
        }

        public async Task<List<Pedido>> ListarPedidosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Pedido?> BuscarPorIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
