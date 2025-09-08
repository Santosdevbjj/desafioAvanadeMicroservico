FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EstoqueService/EstoqueService.csproj", "EstoqueService/"]
RUN dotnet restore "EstoqueService/EstoqueService.csproj"
COPY . .
WORKDIR "/src/EstoqueService"
RUN dotnet build "EstoqueService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EstoqueService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY EstoqueService/entrypoint.sh .
RUN chmod +x entrypoint.sh
ENTRYPOINT ["./entrypoint.sh"]
