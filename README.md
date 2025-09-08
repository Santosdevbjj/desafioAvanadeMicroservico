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


