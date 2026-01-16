# NuGet Package та BaGet Setup для ToyPlanet

## 1. Створення NuGet пакета для ToyPlanet.Core

### Крок 1: Оновлення .csproj файлу

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
    <PackageId>ToyPlanet.Core</PackageId>
    <Version>1.0.0</Version>
    <Authors>ToyPlanet Development Team</Authors>
    <Description>Core library for ToyPlanet application with EF models</Description>
    <PackageProjectUrl>https://github.com/viciusername/toy-shopp</PackageProjectUrl>
    <RepositoryUrl>https://github.com/viciusername/toy-shopp</RepositoryUrl>
    <RepositoryType>git</RepositoryType>
    <PackageTags>toyplanet;orm;entityframework</PackageTags>
    <License>MIT</License>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
  </ItemGroup>

</Project>
```

### Крок 2: Упаковування

```bash
cd src/ToyPlanet.Core
dotnet pack -c Release
```

Файл буде створено у `bin/Release/ToyPlanet.Core.1.0.0.nupkg`

## 2. BaGet - Приватний NuGet Repository

### Налаштування BaGet у Vagrant

```bash
# На хост-машині
docker run -p 5555:80 loicsharma/baget:latest
```

BaGet доступний на `http://localhost:5555`

### Конфігурація у Visual Studio

```xml
<!-- ~/.nuget/NuGet/NuGet.Config -->
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="BaGet" value="http://localhost:5555/v3/index.json" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
```

### Запуск BaGet у Docker Compose

```yaml
version: '3.4'

services:
  baget:
    image: loicsharma/baget:latest
    ports:
      - "5555:80"
    environment:
      - ApiKeyHash=09DCD29A471FD37AFDEAC1BC11A79EBD4BADE17EBFBFF72F4FA829A14E2A4D23
    volumes:
      - baget-data:/var/baget
    networks:
      - toyplanet

volumes:
  baget-data:

networks:
  toyplanet:
```

## 3. Публікація пакета до BaGet

```bash
dotnet nuget push bin/Release/ToyPlanet.Core.1.0.0.nupkg \
  -s http://localhost:5555/v3/index.json
```

## 4. Встановлення з приватного репозиторію

```bash
dotnet add package ToyPlanet.Core -s http://localhost:5555/v3/index.json
```

## 5. Vagrant Provisioning скрипт

```bash
# provision.sh
#!/bin/bash

# Оновлення системи
sudo apt-get update
sudo apt-get upgrade -y

# Встановлення .NET SDK
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version latest

# Налаштування NuGet
mkdir -p ~/.nuget/NuGet
cat > ~/.nuget/NuGet/NuGet.Config << EOF
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="BaGet" value="http://192.168.1.10:5555/v3/index.json" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
EOF

# Встановлення ToyPlanet пакета
dotnet add package ToyPlanet.Core -s http://192.168.1.10:5555/v3/index.json

# Запуск додатка
cd /opt/toyplanet
dotnet restore
dotnet build
dotnet run
```