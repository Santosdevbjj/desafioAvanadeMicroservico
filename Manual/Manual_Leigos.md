## 📝 Manual do Usuário Leigo – Sistema de Microserviços de Estoque e Vendas

Bem-vindo ao sistema de gestão de estoque e vendas!
Este manual foi feito para pessoas sem conhecimento técnico, de forma simples e didática.


---

🚀 O que é este sistema?

É um sistema que simula uma plataforma de e-commerce.
Com ele você pode:

📦 Cadastrar produtos no estoque

🔍 Consultar produtos disponíveis

🛒 Realizar vendas (pedidos)

📑 Consultar pedidos feitos


Tudo isso com segurança, usando login com token (JWT).


---

🖥️ O que eu preciso para usar?

O sistema já vem pronto em containers (Docker).

Você só precisa ter o Docker e Docker Compose instalados no computador.


🔗 Downloads:

Docker Desktop



---

▶️ Como rodar o sistema?

1. Abra o terminal na pasta do projeto (onde está o arquivo docker-compose.yml).


2. Execute o comando:

docker-compose up --build


3. Aguarde até todos os serviços subirem.



Após subir, você terá:

🛡️ API Gateway → http://localhost:8000

📦 EstoqueService (via Gateway) → http://localhost:8000/estoque/...

🛒 VendasService (via Gateway) → http://localhost:8000/vendas/...

🐇 RabbitMQ (painel de mensagens) → http://localhost:15672 (login: guest / senha: guest)

🗄️ MySQL (banco de dados) rodando internamente para salvar os dados.



---

🔑 Como fazer login e usar o sistema?

Todos os endpoints exigem login com token JWT.
Veja como funciona:

1. Fazer Login

Endpoint: POST http://localhost:8000/auth/login

Corpo da requisição (JSON):

{
  "username": "admin",
  "password": "123"
}

Resposta:

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5..."
}


👉 Guarde o token! Ele será usado em todas as próximas requisições.


---

🧪 Usando o Postman (fácil!)

Nós já preparamos uma coleção do Postman com todos os testes prontos.
O arquivo está em:
📂 Manual/DesafioMicroservicos.postman_collection.json

Como usar:

1. Baixe e instale o Postman → Postman Download


2. Abra o Postman → clique em Import


3. Escolha o arquivo DesafioMicroservicos.postman_collection.json


4. A coleção aparecerá na sua barra lateral.



Fluxo dentro do Postman:

1. Execute a requisição Auth - Login

Copie o valor de "token" da resposta

Cole na variável jwt_token (canto superior direito → botão Environment → Edit).



2. Agora, basta executar os endpoints:

Estoque - Cadastrar Produto → adiciona um novo produto

Estoque - Listar Produtos → mostra produtos disponíveis

Vendas - Criar Pedido → faz uma compra de produto

Vendas - Listar Pedidos → mostra os pedidos feitos





---

📦 Exemplos práticos

1. Cadastrar produto

Envie:

{
  "nome": "Notebook Dell",
  "descricao": "i7, 16GB RAM",
  "preco": 4500.00,
  "quantidade": 10
}

Resposta esperada:

{
  "id": 1,
  "nome": "Notebook Dell",
  "descricao": "i7, 16GB RAM",
  "preco": 4500.0,
  "quantidade": 10
}


2. Criar pedido

Envie:

{
  "produtoId": 1,
  "quantidade": 2
}

Resposta esperada:

{
  "id": 1,
  "produtoId": 1,
  "quantidade": 2,
  "dataPedido": "2025-09-08T15:00:00Z"
}



---

🎉 Conclusão

Parabéns! Você agora consegue:
✅ Fazer login
✅ Cadastrar produtos
✅ Consultar estoque
✅ Criar pedidos
✅ Consultar pedidos

Tudo isso de forma simples e segura 🎯


---


