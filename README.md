## Desafio Técnico Avanade

![Avanade001](https://github.com/user-attachments/assets/63abb0a4-1fcb-46d2-a4c3-6e7b9bb0fd7e)



**Bootcamp Avanade - Back-end com .NET e IA**

---


---

Desenvolver uma aplicação com arquitetura de microserviços para gerenciamento de estoque de produtos e vendas em uma plataforma de e-commerce

---

# 🚀 Desafio de Microserviços – Vendas & Estoque

Este projeto demonstra uma **arquitetura de microserviços moderna** em .NET 8, com **RabbitMQ, MySQL, API Gateway (Ocelot)** e **autenticação JWT**.  
A aplicação é dividida em **VendasService** e **EstoqueService**, comunicando-se via mensagens assíncronas e expostos de forma centralizada através de um **API Gateway**.

---

## 📂 Estrutura do Projeto


<img width="816" height="1626" alt="Screenshot_20250908-164409" src="https://github.com/user-attachments/assets/b7c686be-d451-4a53-ba06-dafcfc4da218" />


---

## 🛠️ Tecnologias Utilizadas

- **.NET 8** – Web APIs minimalistas
- **Entity Framework Core** – ORM para persistência em MySQL
- **RabbitMQ** – Mensageria assíncrona entre serviços
- **Ocelot API Gateway** – Roteamento centralizado
- **JWT Authentication** – Segurança e autenticação
- **Docker & Docker Compose** – Containerização e orquestração
- **xUnit + Moq** – Testes automatizados

---

## ⚙️ Como Rodar o Projeto

### 1. Pré-requisitos
- [Docker Desktop](https://www.docker.com/products/docker-desktop)  
- [Postman](https://www.postman.com/downloads/) (opcional, para testar os endpoints)

### 2. Subir os serviços
Na raiz do projeto, execute:
```bash
docker-compose up --build

Isso irá subir:

API Gateway → http://localhost:8000

RabbitMQ (painel) → http://localhost:15672 (user: guest / senha: guest)

MySQL → usado pelos microserviços para persistência


3. Autenticação (JWT)

Antes de usar os endpoints, é necessário login:

POST http://localhost:8000/auth/login

{
  "username": "admin",
  "password": "123"
}

Resposta:

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5..."
}

👉 Copie o token e use no Authorization Header:

Authorization: Bearer <seu_token>


---

📦 Endpoints Disponíveis

🔑 Auth

POST /auth/login → retorna JWT válido


📦 Estoque

POST /estoque/produtos → cadastra produto

GET /estoque/produtos → lista produtos

GET /estoque/produtos/{id} → consulta produto por ID


🛒 Vendas

POST /vendas/pedidos → cria pedido (e publica mensagem no RabbitMQ)

GET /vendas/pedidos → lista pedidos

GET /vendas/pedidos/{id} → consulta pedido por ID



---

🧪 Testes Automatizados

Cada serviço possui sua suíte de testes.
Para rodar os testes, execute:

dotnet test

EstoqueService.Tests → Testes de Produto

VendasService.Tests → Testes de Pedido

ApiGateway.Tests → Testes de autenticação via JWT



---

📑 Documentação

Além deste README, há dois manuais disponíveis em /Manual:

Manual_Leigos.md → passo a passo simples para usuários não técnicos

Manual_Tecnico.md → explicação da arquitetura, código e testes


Também há a coleção do Postman pronta para importação:

📂 Manual/DesafioMicroservicos.postman_collection.json



---

🎯 Conclusão

Este projeto cobre conceitos essenciais de microserviços modernos:

Comunicação assíncrona com RabbitMQ

Persistência com MySQL e EF Core

API Gateway com Ocelot

Autenticação segura via JWT

Testes automatizados com xUnit


Pronto para ser usado como base de estudo ou como ponto de partida para sistemas distribuídos reais 🚀

---














