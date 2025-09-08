#!/bin/bash
set -e

echo "Aguardando MySQL iniciar..."
until nc -z -v -w30 mysql 3306
do
  echo "Aguardando conexão com MySQL..."
  sleep 5
done

echo "Aplicando migrations do EstoqueService..."
dotnet ef database update --project EstoqueService.csproj

echo "Iniciando EstoqueService..."
exec dotnet run --project EstoqueService.csproj --urls "http://+:5002"
