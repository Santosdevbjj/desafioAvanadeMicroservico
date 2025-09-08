#!/bin/bash
set -e

# Entrypoint do ApiGateway: aguarda serviços dependentes e inicia o gateway (ApiGateway.dll)
# Observação: este script usa /dev/tcp para checar disponibilidade de socket dentro da rede Docker.

WAIT_HOST_PORT() {
  local host="$1"
  local port="$2"
  local retries=0
  local max_retries=120   # ~4 minutos (120 * 2s)
  echo "⏳ Aguardando $host:$port ficar disponível..."
  until bash -c "cat < /dev/tcp/${host}/${port}" >/dev/null 2>&1; do
    retries=$((retries+1))
    if [ "$retries" -ge "$max_retries" ]; then
      echo "❌ Timeout ao aguardar $host:$port"
      return 1
    fi
    sleep 2
  done
  echo "✅ $host:$port disponível!"
  return 0
}

# Hosts e portas (ajuste conforme necessário)
# Estes nomes/portas devem bater com os serviços definidos no docker-compose (servicename:porta_interna).
DEPENDENCIES=(
  "vendasservice:8080"
  "estoqueservice:8081"
)

# Espera dependências
for dep in "${DEPENDENCIES[@]}"; do
  host="${dep%%:*}"
  port="${dep##*:}"
  WAIT_HOST_PORT "$host" "$port" || {
    echo "[ERRO] Dependência $host:$port não ficou pronta. Abortando."
    exit 1
  }
done

echo "🎯 Todas dependências disponíveis. Iniciando ApiGateway..."

# Iniciar app
exec dotnet ApiGateway.dll
