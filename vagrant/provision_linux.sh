#!/bin/bash

# Provisioning скрипт для Linux VM

set -e

echo "========== Оновлення системи =========="
sudo apt-get update
sudo apt-get upgrade -y

echo "========== Встановлення .NET SDK =========="
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version latest --install-dir /usr/local/dotnet

# Додавання до PATH
echo 'export PATH=$PATH:/usr/local/dotnet' >> ~/.bashrc
export PATH=$PATH:/usr/local/dotnet

echo "========== Встановлення Git =========="
sudo apt-get install -y git

echo "========== Встановлення Docker =========="
sudo apt-get install -y docker.io docker-compose
sudo usermod -aG docker ubuntu

echo "========== Встановлення PostgreSQL клієнта =========="
sudo apt-get install -y postgresql-client

echo "========== Встановлення BaGet =========="
docker pull loicsharma/baget:latest

echo "========== Клонування репозиторію =========="
cd /opt
git clone https://github.com/viciusername/toy-shopp.git toyplanet

echo "========== Налаштування NuGet =========="
mkdir -p ~/.nuget/NuGet
cat > ~/.nuget/NuGet/NuGet.Config << 'EOF'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
EOF

echo "========== Встановлення залежностей =========="
cd /opt/toyplanet
dotnet restore

echo "========== Збірка проекту =========="
dotnet build -c Release

echo "========== Бази даних =========="
docker run -d \
  --name toyplanet-db \
  -e POSTGRES_USER=toyplanet \
  -e POSTGRES_PASSWORD=password123 \
  -e POSTGRES_DB=toyplanet \
  -p 5432:5432 \
  postgres:15

echo "========== Міграція БД =========="
sleep 5
cd /opt/toyplanet/src/ToyPlanet.Web
dotnet ef database update --connection "Host=localhost;Port=5432;Database=toyplanet;Username=toyplanet;Password=password123"

echo "========== Готово! =========="
echo "Додаток запускається на http://localhost:5000"
