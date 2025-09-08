# Etapa base (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

# Etapa de publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Etapa final (imagem enxuta para produção)
FROM base AS final
WORKDIR /app

# Copia a aplicação publicada
COPY --from=publish /app/publish .

# Copia o arquivo de configuração Ocelot
COPY ocelot.json .

# Copia o script de inicialização
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

# Define o entrypoint para aplicar checagem de dependências antes de rodar
ENTRYPOINT ["./entrypoint.sh"]
