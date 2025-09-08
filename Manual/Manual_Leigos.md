# 📄 Manual do Sistema de Pedidos e Estoque (Para Leigos)

Bem-vindo! Este manual foi criado para pessoas que **não são técnicas** e querem entender como usar o sistema.

---

## 1. O que é este sistema?
Este sistema permite:
- Fazer pedidos de produtos (ex.: vendas);
- Controlar o estoque de produtos;
- Garantir que o pedido só seja aceito se houver produto disponível.

---

## 2. Como funciona na prática?
1. O **cliente faz um pedido** (ex.: "Quero comprar 3 camisetas").
2. O **serviço de vendas** registra esse pedido.
3. O **serviço de estoque** verifica se há camisetas disponíveis.
4. Se tiver:
   - O pedido é confirmado.
   - O estoque é atualizado.
5. Se não tiver:
   - O pedido é recusado.
   - O cliente é avisado.

---

## 3. Como usar?
- **API Gateway**: é como a porta de entrada, todos os acessos passam por ele.
- **Serviço de Vendas**: cadastra pedidos.
- **Serviço de Estoque**: mostra e controla produtos disponíveis.

Você não precisa se preocupar com a parte técnica. Basta usar os **endereços da API**:
- `http://localhost:5000/vendas` → fazer pedidos.
- `http://localhost:5000/estoque` → consultar estoque.

---

## 4. Exemplos de uso
### Consultar produtos disponíveis

GET http://localhost:5000/estoque/produtos

### Fazer um pedido

POST http://localhost:5000/vendas/pedidos { "produtoId": 1, "quantidade": 2 }

---

## 5. Conclusão
Esse sistema foi feito para **facilitar a vida de quem vende** e de **quem controla estoque**, funcionando de forma automática e confiável.


---
