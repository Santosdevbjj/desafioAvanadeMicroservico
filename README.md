## Desafio Técnico Avanade

![Avanade001](https://github.com/user-attachments/assets/63abb0a4-1fcb-46d2-a4c3-6e7b9bb0fd7e)



**Bootcamp Avanade - Back-end com .NET e IA**

---


Desenvolver uma aplicação com arquitetura de microserviços para gerenciamento de estoque de produtos e vendas em uma plataforma de e-commerce


---

🔹 **Passos para rodar**

1. Criar o banco de dados:



CREATE DATABASE vendasdb;

2. Instalar pacotes NuGet:



dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.EntityFrameworkCore.Design

3. Rodar migrations:



dotnet ef migrations add InitialCreate
dotnet ef database update



---

📌 Subindo tudo

Na raiz do projeto:

docker-compose up -d

Verificar os containers:

docker ps

Acessos:

MySQL → localhost:3306 (root / 123456)

RabbitMQ Dashboard → http://localhost:15672 (admin / admin)

phpMyAdmin → http://localhost:8081



---



---

📌 Como rodar

1. Gerar migrations localmente:



cd VendasService
dotnet ef migrations add InitialCreate
cd ../EstoqueService
dotnet ef migrations add InitialCreate

2. Subir os containers:



docker-compose up --build -d

3. Verificar logs:



docker logs vendasservice
docker logs estoqueservice


---

---

📌 Como usar o API Gateway

1. Subir tudo:



docker-compose up --build -d

2. Testar rotas via Gateway:



Produtos:

curl http://localhost:5000/estoque/produto

Pedidos:

curl http://localhost:5000/vendas/pedido


3. Acessar RabbitMQ em http://localhost:15672


4. Acessar banco via phpMyAdmin em http://localhost:8081




---

---

✅ Detalhes importantes

1. Rotas do EstoqueService:

GET /estoque/produto → Lista todos os produtos

POST /estoque/produto → Cria um novo produto

GET /estoque/produto/{id} → Consulta produto por ID

PUT /estoque/produto/{id} → Atualiza produto

DELETE /estoque/produto/{id} → Deleta produto



2. Rotas do VendasService:

GET /vendas/pedido → Lista todos os pedidos

POST /vendas/pedido → Cria um novo pedido

GET /vendas/pedido/{id} → Consulta pedido por ID



3. BaseUrl global:

Dentro do Docker, o Gateway acessa os serviços pelo nome do container (estoqueservice, vendasservice).





---






