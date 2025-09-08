using Moq;
using Xunit;
using EstoqueService.Repositories;
using EstoqueService.Services;
using EstoqueService.Models;

namespace EstoqueService.Tests;

public class ProdutoServiceTests
{
    [Fact]
    public async Task Deve_Cadastrar_Produto()
    {
        var mockRepo = new Mock<IProdutoRepository>();
        var produtoService = new ProdutoService(mockRepo.Object);

        var produto = new Produto
        {
            Nome = "Mouse Gamer",
            Descricao = "Mouse com 6 botões",
            Preco = 150.0m,
            Quantidade = 10
        };

        await produtoService.AdicionarProduto(produto);

        mockRepo.Verify(r => r.AddAsync(It.IsAny<Produto>()), Times.Once);
    }
}
