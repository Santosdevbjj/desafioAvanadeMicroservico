namespace EstoqueService.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }
}
