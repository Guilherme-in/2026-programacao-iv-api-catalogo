using umfgcloud.loja.dominio.service.DTO;

namespace umfgcloud.aplicacao.service.testes.Classes
{
    [TestClass]
    public sealed class ProdutoServicoTestes : AbstractServicoTestes
    {
        private const string C_OWNER = "Guilherme Neves";
        private const string C_CATEGORY = "produto";

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_DeveAdicionarProdutoComSucesso()
        {
            // Arrange
            using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
            var servico = GetProdutoServicoValidJWT(context);

            var dto = new ProdutoDTO.ProdutoRequest
            {
                Descricao = "Produto teste",
                EAN = "123456789",
                ValorCompra = 39.90m,
                ValorVenda = 89.90m
            };

            // Act
            await servico.AdicionarAsync(dto);
            var produto = (await servico.ObterTodosAsync()).FirstOrDefault();

            // Assert
            Assert.IsNotNull(produto);
            Assert.AreNotEqual(Guid.Empty, produto.Id);
            Assert.AreEqual("PRODUTO TESTE", produto.Descricao);
            Assert.AreEqual("123456789", produto.EAN);
            Assert.AreEqual(39.90m, produto.ValorCompra);
            Assert.AreEqual(89.90m, produto.ValorVenda);
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_ObterTodosAsync_DeveRetornarProdutosAtivos()
        {
            // Arrange
            using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
            var servico = GetProdutoServicoValidJWT(context);

            await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest
            {
                Descricao = "Produto 1",
                EAN = "111",
                ValorCompra = 10m,
                ValorVenda = 20m
            });

            await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest
            {
                Descricao = "Produto 2",
                EAN = "222",
                ValorCompra = 30m,
                ValorVenda = 40m
            });

            // Act
            var produtos = await servico.ObterTodosAsync();

            // Assert
            Assert.AreEqual(2, produtos.Count());
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_ObterPorIdAsync_DeveRetornarProduto()
        {
            // Arrange
            using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
            var servico = GetProdutoServicoValidJWT(context);

            await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest
            {
                Descricao = "Produto teste",
                EAN = "123",
                ValorCompra = 10m,
                ValorVenda = 20m
            });

            var produtoCriado = (await servico.ObterTodosAsync()).First();

            // Act
            var produto = await servico.ObterPorIdAsync(produtoCriado.Id);

            // Assert
            Assert.IsNotNull(produto);
            Assert.AreEqual(produtoCriado.Id, produto.Id);
            Assert.AreEqual("PRODUTO TESTE", produto.Descricao);
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AtualizarAsync_DeveAtualizarProduto()
        {
            // Arrange
            using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
            var servico = GetProdutoServicoValidJWT(context);

            await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest
            {
                Descricao = "Produto antigo",
                EAN = "123",
                ValorCompra = 10m,
                ValorVenda = 20m
            });

            var produtoCriado = (await servico.ObterTodosAsync()).First();

            var dto = new ProdutoDTO.ProdutoRequestWithId
            {
                Id = produtoCriado.Id,
                Descricao = "Produto atualizado",
                EAN = "999",
                ValorCompra = 50m,
                ValorVenda = 80m
            };

            // Act
            await servico.AtualizarAsync(dto);
            var produtoAtualizado = await servico.ObterPorIdAsync(produtoCriado.Id);

            // Assert
            Assert.AreEqual("PRODUTO ATUALIZADO", produtoAtualizado.Descricao);
            Assert.AreEqual("999", produtoAtualizado.EAN);
            Assert.AreEqual(50m, produtoAtualizado.ValorCompra);
            Assert.AreEqual(80m, produtoAtualizado.ValorVenda);
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_RemoverAsync_DeveRemoverProduto()
        {
            // Arrange
            using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
            var servico = GetProdutoServicoValidJWT(context);

            await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest
            {
                Descricao = "Produto remover",
                EAN = "123",
                ValorCompra = 10m,
                ValorVenda = 20m
            });

            var produtoCriado = (await servico.ObterTodosAsync()).First();

            // Act
            await servico.RemoverAsync(produtoCriado.Id);

            // Assert
            await Assert.ThrowsExceptionAsync<ApplicationException>(
                () => servico.ObterPorIdAsync(produtoCriado.Id)
            );
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_DeveFalharQuandoValorCompraForNegativo()
        {
            // Arrange
            using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
            var servico = GetProdutoServicoValidJWT(context);

            var dto = new ProdutoDTO.ProdutoRequest
            {
                Descricao = "Produto inválido",
                EAN = "123",
                ValorCompra = -10m,
                ValorVenda = 20m
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<InvalidDataException>(
                () => servico.AdicionarAsync(dto)
            );
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_DeveFalharQuandoValorVendaForNegativo()
        {
            // Arrange
            using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
            var servico = GetProdutoServicoValidJWT(context);

            var dto = new ProdutoDTO.ProdutoRequest
            {
                Descricao = "Produto inválido",
                EAN = "123",
                ValorCompra = 10m,
                ValorVenda = -20m
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<InvalidDataException>(
                () => servico.AdicionarAsync(dto)
            );
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public void ProdutoServico_Instanciar_DeveFalharComJwtInvalido()
        {
            // Arrange
            using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

            // Act / Assert
            Assert.ThrowsException<InvalidDataException>(
                () => GetProdutoServicoInvalidJWT(context)
            );
        }
    }
}