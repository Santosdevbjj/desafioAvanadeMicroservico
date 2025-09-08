# 📄 Manual Técnico - Sistema de Pedidos e Estoque

Este manual é destinado a desenvolvedores, administradores de sistemas e profissionais de TI.

---

## 1. Arquitetura
A solução é composta por **microserviços** integrados via **RabbitMQ** e **API Gateway**:

- **VendasService** → Gerencia pedidos e publica eventos.
- **EstoqueService** → Consome eventos de pedidos e controla estoque.
- **Shared** → Contém DTOs e serviços compartilhados (ex.: autenticação JWT).
- **API Gateway (Ocelot)** → Centraliza e roteia requisições para os microserviços.
- **MySQL** → Banco de dados para persistência de Vendas e Estoque.
- **RabbitMQ** → Mensageria para comunicação assíncrona entre serviços.

---

## 2. Requisitos
- **Docker** e **Docker Compose**
- **.NET 8 SDK**
- **MySQL Client** (opcional para debug)

---

## 3. Subindo o ambiente
Para rodar todos os serviços:
```bash
docker-compose up --build

Serviços disponíveis:

API Gateway → http://localhost:5000

RabbitMQ Management → http://localhost:15672 (user: guest, pass: guest)

MySQL → localhost:3306 (user: root, pass: rootpassword)



---

4. Banco de Dados

Cada serviço tem seu banco:

VendasDb

EstoqueDb


As migrations são aplicadas automaticamente na inicialização via entrypoint.sh.


---

5. Comunicação Assíncrona

O VendasService publica mensagens de pedidos no RabbitMQ.

O EstoqueService consome essas mensagens e atualiza o estoque.

Se o estoque não for suficiente, um evento de falha pode ser enviado (futuro).



---

6. Estrutura do Projeto

├── ApiGateway/
├── VendasService/
│   ├── Controllers/
│   ├── Data/
│   ├── Repositories/
│   ├── Services/
├── EstoqueService/
│   ├── Controllers/
│   ├── Data/
│   ├── Repositories/
│   ├── Services/
├── Shared/
│   ├── DTOs/
│   └── Auth/
├── Tests/
│   ├── EstoqueService.Tests/
│   ├── VendasService.Tests/
│   └── ApiGateway.Tests/
└── Manual/


---

7. Endpoints principais

Estoque

GET /estoque/produtos

POST /estoque/produtos


Vendas

POST /vendas/pedidos

GET /vendas/pedidos



---

8. Testes

Testes unitários em cada serviço (xUnit):

ProdutoServiceTests.cs

PedidoServiceTests.cs

AuthTests.cs


Rodar os testes:

dotnet test


---

9. Segurança

Autenticação baseada em JWT.

Token gerado no Shared/Auth/JwtTokenService.cs.

Ocelot pode validar tokens para proteger os endpoints.



---

10. Próximos Passos

Implementar eventos de rollback em caso de falha no estoque.

Monitoramento com Prometheus + Grafana.

Deploy em Kubernetes para alta disponibilidade.


---
