#!/bin/bash
set -e

echo "🚀 Aguardando MySQL ficar pronto..."
until dotnet ef database update --project VendasService.csproj; do
  echo "⏳ Banco ainda não disponível, tentando novamente..."
  sleep 5
done

echo "✅ Migrações aplicadas com sucesso!"
exec dotnet VendasService.dll
