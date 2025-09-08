## Manual Técnico — Sistema de Microserviços (Estoque + Vendas)

🔹 Documento técnico detalhado para desenvolvedores e administradores.
Contém arquitetura, explicação de pastas/arquivos, fluxo de mensagens, comandos para rodar, debug, testes automatizados (dotnet test) e boas práticas.


---

Índice

1. Visão geral da arquitetura


2. Componentes & responsabilidades


3. Estrutura do repositório (arquivo por arquivo)


4. Configuração (appsettings / variáveis de ambiente)


5. Como rodar (Docker Compose — recomendado)


6. Como rodar localmente (sem Docker)


7. Migrations (criar / aplicar)


8. Mensageria RabbitMQ — formato de mensagens e consumers/publishers


9. API Gateway (Ocelot) — rotas e exemplos


10. Testes automatizados (xUnit + Moq) — como rodar e conceitos


11. Debugging / Troubleshooting (logs, health checks, comandos)


12. Boas práticas, segurança e próximos passos


13. Comandos úteis & anexo (exemplos SQL, payloads JSON)




---

1 — Visão geral da arquitetura

O sistema é composto por microserviços desacoplados:

ApiGateway (Ocelot) — único ponto de entrada das requisições externas; roteia para EstoqueService e VendasService e expõe o endpoint de autenticação (/auth) que retorna JWT.

VendasService — expõe endpoints para criação e consulta de pedidos; persiste pedidos no MySQL (vendasdb) e publica eventos de pedidos (fila pedidos) no RabbitMQ.

EstoqueService — expõe endpoints CRUD para produtos; persiste produtos no MySQL (estoquedb) e consome eventos da fila pedidos para reduzir o estoque.

RabbitMQ — mensageria assíncrona entre VendasService (publisher) e EstoqueService (consumer).

Shared — biblioteca compartilhada (DTOs, JwtTokenService) para geração de tokens JWT.

Tests — projetos de teste com xUnit e Moq.


Fluxo de venda (resumido):

1. Cliente faz POST /vendas/pedido → VendasService persiste pedido e publica PedidoMessage em pedidos.


2. EstoqueService consome PedidoMessage → dá baixa nas quantidades do produto.


3. (Opcional) EstoqueService pode publicar confirmação/erro de estoque.




---

2 — Componentes & responsabilidades

ApiGateway/

ocelot.json — mapeia rotas upstream → downstream.

Program.cs, entrypoint.sh, Dockerfile.

Roteia /estoque/... → estoqueservice, /vendas/... → vendasservice, /auth/... → vendasservice (AuthController).


VendasService/

Program.cs — configura EF Core, JWT auth, DI.

Controllers/PedidoController.cs — endpoints REST.

Data/VendasContext.cs (ou AppDbContext.cs) — DbSet<Pedido>, DbSet<ItemPedido>.

Repositories/PedidoRepository.cs — padrão Repository.

Services/PedidoService.cs — regras de negócio; usa IPedidoRepository e RabbitMqPublisher.

Messaging/RabbitMqPublisher.cs — publica PedidoMessage para fila pedidos.

Migrations/ — migrations EF Core (InitialCreate).

Dockerfile, entrypoint.sh.


EstoqueService/

Program.cs — configura EF Core, JWT auth, DI.

Controllers/EstoqueController.cs — CRUD produtos.

Data/EstoqueContext.cs — DbSet<Produto>.

Repositories/ProdutoRepository.cs e IProdutoRepository.cs.

Services/ProdutoService.cs — lógica do estoque (inclui método para decrementar estoque).

Messaging/RabbitMqConsumer.cs — BackgroundService/consumer que abre fila pedidos e processa mensagens.

Migrations/ — migrations EF Core (InitialCreate).

Dockerfile, entrypoint.sh.


Shared/

Shared.csproj — referência comum.

DTOs/ProdutoDTO.cs, DTOs/PedidoDTO.cs.

Auth/JwtTokenService.cs — geração de tokens JWT.

Auth/AuthController.cs — endpoint /auth/login (mock) que retorna token.


Tests/

EstoqueService.Tests/ — ProdutoServiceTests.cs (xUnit + Moq).

VendasService.Tests/ — PedidoServiceTests.cs.

ApiGateway.Tests/ — AuthTests.cs.




---

3 — Estrutura de pastas (arquivo por arquivo — referência)

(Adaptar caso você tenha alterado nomes — mantenha consistência entre DbContext e Program.cs.)

/ (repo root)
├─ ApiGateway/
│  ├─ Program.cs
│  ├─ ApiGateway.csproj
│  ├─ ocelot.json
│  ├─ Dockerfile
│  └─ entrypoint.sh
├─ EstoqueService/
│  ├─ EstoqueService.csproj
│  ├─ Program.cs
│  ├─ appsettings.json
│  ├─ Dockerfile
│  ├─ entrypoint.sh
│  ├─ Controllers/EstoqueController.cs
│  ├─ Data/EstoqueContext.cs
│  ├─ Data/Migrations/InitialCreate...
│  ├─ Models/Produto.cs
│  ├─ Repositories/IProdutoRepository.cs
│  ├─ Repositories/ProdutoRepository.cs
│  └─ Messaging/RabbitMqConsumer.cs
├─ VendasService/
│  ├─ VendasService.csproj
│  ├─ Program.cs
│  ├─ appsettings.json
│  ├─ Dockerfile
│  ├─ entrypoint.sh
│  ├─ Controllers/PedidoController.cs
│  ├─ Data/VendasContext.cs (ou AppDbContext.cs)
│  ├─ Data/Migrations/InitialCreate...
│  ├─ Models/Pedido.cs
│  ├─ Models/ItemPedido.cs
│  ├─ Repositories/IPedidoRepository.cs
│  ├─ Repositories/PedidoRepository.cs
│  ├─ Services/PedidoService.cs
│  └─ Messaging/RabbitMqPublisher.cs
├─ Shared/
│  ├─ Shared.csproj
│  ├─ DTOs/ProdutoDTO.cs
│  └─ Auth/JwtTokenService.cs
├─ Tests/
│  ├─ EstoqueService.Tests/
│  ├─ VendasService.Tests/
│  └─ ApiGateway.Tests/
├─ docker-compose.yml
└─ Manual/
   ├─ Manual_Leigos.md
   ├─ Manual_Tecnico.md  <-- (este arquivo)
   └─ DesafioMicroservicos.postman_collection.json


---

4 — Configuração (appsettings.json / variáveis de ambiente)

Exemplo (VendasService/appsettings.json):

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=mysql;Database=vendasdb;User=root;Password=123456;"
  },
  "Jwt": {
    "Key": "minha_chave_super_secreta_vendas",
    "Issuer": "VendasServiceAuth"
  },
  "Logging": { "LogLevel": { "Default": "Information" } }
}

Exemplo (EstoqueService/appsettings.json):

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=mysql;Database=estoquedb;User=root;Password=123456;"
  },
  "Jwt": {
    "Key": "minha_chave_super_secreta_estoque",
    "Issuer": "EstoqueServiceAuth"
  }
}

Notas importantes

Em produção NUNCA comitar Jwt:Key em texto claro — use Key Vault / Secrets Manager / variables em CI/CD.

No docker-compose.yml você pode sobrescrever com variáveis de ambiente:

environment:
  - ConnectionStrings__DefaultConnection=Server=mysql;Database=...;
  - Jwt__Key=${JWT_KEY}
  - Jwt__Issuer=VendasServiceAuth



---

5 — Como rodar (Docker Compose) — passo a passo

1. Na raiz do repositório:



docker-compose up --build -d

2. Verificar containers:



docker ps

3. Logs:



docker-compose logs -f vendasservice
docker-compose logs -f estoqueservice
docker-compose logs -f apigateway

4. Testar endpoints via API Gateway (ex.: gateway mapeado para porta 8080 no docker-compose):



Auth (login): POST http://localhost:8080/auth/login

Criar produto: POST http://localhost:8080/estoque/produto

Criar pedido: POST http://localhost:8080/vendas/pedido


Importantíssimo: cada serviço possui entrypoint.sh que aguarda MySQL e executa dotnet ef database update antes de iniciar a API — assim migrations são aplicadas automaticamente.


---

6 — Como rodar localmente (sem Docker)

Pré-requisitos

.NET 8 SDK instalado

MySQL local (ou container)

RabbitMQ local (ou container)


Passos

1. Ajuste appsettings.json de cada serviço para apontar seu MySQL local.


2. No terminal, dentro da pasta do serviço:



cd VendasService
dotnet restore
dotnet ef database update        # aplica migrations localmente
dotnet run

Repita para EstoqueService. Inicie o RabbitMQ (pode ser via Docker).


---

7 — Migrations (criar / aplicar / boas práticas)

Gerar nova migration

No projeto (ex.: VendasService):

cd VendasService
dotnet ef migrations add NomeDaMigration -o Data/Migrations

Aplicar migrations (local)

dotnet ef database update

No Docker

Os entrypoint.sh já executam:

dotnet ef database update --project <ProjectFile>

(aguardam MySQL e então aplicam migrations).

Boas práticas

Commitar as pastas Migrations/ no Git (para que CI e Docker possam aplicar).

Use nomes significativos para migrations.

Em ambientes com múltiplas instâncias, usar rollout controlado (evitar corrida de migrations).



---

8 — RabbitMQ — formato de mensagem / consumo

Publisher (VendasService) → fila pedidos

Exemplo JSON publicado

{
  "PedidoId": 1,
  "Itens": [
    { "ProdutoId": 1, "Quantidade": 2 },
    { "ProdutoId": 3, "Quantidade": 1 }
  ]
}

Consumer (EstoqueService)

Escuta fila pedidos.

Para cada item: chama ProdutoService.BaixarEstoque(produtoId, quantidade) ou ProdutoRepository para decrementar.

Em caso de erro (estoque insuficiente), pode:

Registrar em log/Dead Letter Queue.

Emitir evento de falha (não implementado por padrão — recomendação futura).



Recomendações

Configure autoAck=false e confirme a mensagem apenas após sucesso do processamento; em caso de exceção requeue/encaminhe para DLQ.

Para alta confiabilidade, implemente SAGA (compensating transactions) com MassTransit ou outro orquestrador.



---

9 — API Gateway (Ocelot) — rotas e exemplos

Arquivo ApiGateway/ocelot.json define rotas. Exemplos (via gateway):

Produtos:

GET  http://localhost:8080/estoque/produto

POST http://localhost:8080/estoque/produto


Pedidos:

POST http://localhost:8080/vendas/pedido

GET  http://localhost:8080/vendas/pedido


Auth:

POST http://localhost:8080/auth/login → retorna { "token": "..." }



Observação: Ocelot aceita validação do token; neste projeto o Gateway roteia e a validação é feita nos microserviços (opção simples). Você pode mover verificação de token para o Gateway se desejar centralizar autenticação.


---

10 — Testes automatizados (xUnit + Moq)

Estrutura dos testes

Tests/EstoqueService.Tests/ — testa ProdutoService com Moq do IProdutoRepository.

Tests/VendasService.Tests/ — testa PedidoService com Moq do IPedidoRepository.

Tests/ApiGateway.Tests/ — testa JwtTokenService (gera token).


Rodar testes (cada projeto)

Na raiz do repo, execute:

dotnet test ./Tests/EstoqueService.Tests/EstoqueService.Tests.csproj
dotnet test ./Tests/VendasService.Tests/VendasService.Tests.csproj
dotnet test ./Tests/ApiGateway.Tests/ApiGateway.Tests.csproj

Ou rodar todos os testes:

dotnet test ./Tests

(Se os TestProject estiverem configurados para discovery, dotnet test recursivamente rodará todos.)

Boas práticas para testes

Testes unitários devem usar Moq para isolar dependências (repositórios, publisher).

Para testes de integração (end-to-end), use WebApplicationFactory<T> (Microsoft.AspNetCore.Mvc.Testing) ou contêineres testáveis (Testcontainers).

Cobertura: foco em regras de negócio (validação de estoque, lógica de criação/rollback de pedido).



---

11 — Debugging & Troubleshooting

Comandos úteis Docker

Ver containers:


docker ps

Ver logs:


docker-compose logs -f vendasservice
docker-compose logs -f estoqueservice
docker-compose logs -f apigateway
docker-compose logs -f rabbitmq
docker-compose logs -f mysql

Parar e remover:


docker-compose down --volumes --remove-orphans

Health checks

Acesse RabbitMQ Management: http://localhost:15672 (usuário/ senha conforme docker-compose.yml) para checar filas e mensagens.

Use phpMyAdmin (se adicionado) ou cliente MySQL para conferir tabelas e dados.


Problemas comuns

Migrations não aplicadas: verifique logs do entrypoint.sh nos containers; veja se a string de conexão está correta.

Consumer não consome: verifique se publisher publicou na fila pedidos, se filas existem e se RabbitMqConsumer se conectou ao host correto (rabbitmq container).

JWT inválido: verifique Jwt:Key e Issuer em appsettings.json (eles devem bater com o que JwtTokenService usa).

Conflito de portas: ajuste docker-compose.yml para evitar conflitos locais.



---

12 — Boas práticas & próximas melhorias

Segurança

Nunca commit secrets. Use env vars ou secret-store.

Habilite HTTPS nas APIs.

Use rotação de chaves JWT e expiração curta.


Resiliência

Políticas de retry/exponential backoff (Polly).

Dead-letter queue (DLQ) para mensagens que falham várias vezes.

Observability: OpenTelemetry + Prometheus + Grafana.


Consistência distribuída

Implementar SAGA (MassTransit tem suporte) para garantir consistência entre serviços (reservar produto → confirmar pagamento → finalizar pedido ou rollback).


CI / CD

GitHub Actions para: build → dotnet test → docker build → push image.

Use imagens versionadas no registry privado.




---

13 — Comandos úteis & Anexos

Comandos principais

# Build & up
docker-compose up --build -d

# Ver logs
docker-compose logs -f

# Rodar testes individuais
dotnet test ./Tests/EstoqueService.Tests/EstoqueService.Tests.csproj

# Gerar migration (ex.: VendasService)
cd VendasService
dotnet ef migrations add AddColumnX -o Data/Migrations
dotnet ef database update

Exemplo SQL / consultas úteis

Produtos com estoque abaixo da média:


SELECT p.Id, p.Nome, p.Quantidade
FROM Produtos p
WHERE p.Quantidade < (SELECT AVG(Quantidade) FROM Produtos);

Top produtos vendidos (precisa de histórico):


SELECT oi.ProdutoId, SUM(oi.Quantidade) AS TotalVendido
FROM ItensPedido oi
GROUP BY oi.ProdutoId
ORDER BY TotalVendido DESC
LIMIT 5;

Payload Exemplo — Pedido (para publicar no RabbitMQ)

{
  "PedidoId": 123,
  "Itens": [
    { "ProdutoId": 1, "Quantidade": 2 },
    { "ProdutoId": 5, "Quantidade": 1 }
  ]
}


---

Observação de consistência (importante)

Durante o desenvolvimento iterativo foram usados nomes como AppDbContext, VendasContext e EstoqueContext em distintos momentos. Importante: confirme que o DbContext definido nos arquivos dentro de Data/ possui o mesmo nome que é usado no Program.cs (AddDbContext<SeuDbContext>). Se houver discrepância, alinhe o nome da classe ou o AddDbContext<> para o nome correto.


---

**Conclusão**

Este manual dá a visão completa e as instruções práticas para rodar, desenvolver, testar e depurar o sistema. Se quiser, eu posso:

Gerar um GitHub Actions workflow pronto (build + test + docker build).

Implementar testes de integração (end-to-end) com Testcontainers.

Gerar um Makefile ou scripts bash para facilitar comandos repetidos.


---


