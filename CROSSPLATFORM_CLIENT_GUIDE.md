# Кросс-платформний клієнт ToyPlanet - Інструкції для розробки

## Огляд проекту

Це кросс-платформний клієнт для системи управління магазином іграшок, розроблений з використанням:
- **Xamarin.Forms** / **Avalonia** / **.NET MAUI** для UI
- **MVVM паттерну** для архітектури
- **Identity Server** для аутентифікації
- **OxyPlot / LiveCharts** для графіків

## Архітектура MVVM

### Структура папок

```
src/ToyPlanet.Client/
├── Models/           # Моделі даних
├── Views/            # UI представлення (XAML)
├── ViewModels/       # Логіка представлення
└── Services/         # Сервіси (API, Auth, тощо)
```

### Компоненти

1. **Models** - POCO класи для даних
2. **Views** - XAML сторінки з UI
3. **ViewModels** - Логіка з INotifyPropertyChanged
4. **Services** - Бізнес-логіка та API клієнти

## Функціональність

### 1. Аутентифікація (Identity Server)
- Вхід користувача
- Реєстрація
- Управління токенами
- Безпечні запити до API

### 2. Введення даних у 3 таблиці

#### Товари (Toys)
```csharp
public class Toy
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
```

#### Категорії (Categories)
```csharp
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Toy> Toys { get; set; }
}
```

#### Замовлення (Orders)
```csharp
public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal TotalPrice { get; set; }
}
```

### 3. Графіки

- **Графік продаж по категоріям** - Pie Chart
- **Графік продаж по дням** - Line Chart

### 4. Екран про додаток (About)

Інформація про:
- Назву додатка
- Версію
- Авторів
- Опис
- Дату випуску
- Посилання на GitHub

### 5. Анімований екран завантаження

- Вживаються при завантаженні даних
- Прогрес-бар з анімацією
- Текстові повідомлення

## Розгортання на різні платформи

### Windows
```bash
dotnet build -f net8.0-windows
dotnet run
```

### Linux
```bash
dotnet build -f net8.0-linux
dotnet run
```

### macOS
```bash
dotnet build -f net8.0-macos
dotnet run
```

## Встановлення залежностей

```bash
cd src/ToyPlanet.Client
dotnet add package IdentityModel
dotnet add package CommunityToolkit.Mvvm
dotnet add package OxyPlot.Avalonia
dotnet add package Avalonia
```

## Запуск додатка

### Для Avalonia
```bash
cd src/ToyPlanet.Client.Avalonia
dotnet run
```

### Для Xamarin
```bash
cd src/ToyPlanet.Client.Xamarin
dotnet build -t:Run -f net8.0-android
```

### Для .NET MAUI
```bash
cd src/ToyPlanet.Client.MAUI
dotnet build -t:Run -f net8.0-windows
```

## Налаштування Identity Server

1. Переконайтеся, що Identity Server запущений на `http://localhost:5000`
2. Додайте клієнта у конфігурацію Identity Server:
   ```csharp
   new Client
   {
       ClientId = "toyplanet-client",
       ClientName = "ToyPlanet Desktop Client",
       AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
       ClientSecrets = { new Secret("your-secret-key".Sha256()) },
       AllowedScopes = { "openid", "profile", "api" }
   }
   ```

## Сторінки додатка

1. **Login Page** - Вхід та реєстрація
2. **Catalog Page** - Перегляд та пошук товарів
3. **Cart Page** - Управління кошиком
4. **Orders Page** - Історія замовлень
5. **Statistics Page** - Графіки та статистика
6. **About Page** - Інформація про додаток

## Тестування

```bash
dotnet test ToyPlanet.Client.Tests
```

## GitHub

Клонування репозиторію:
```bash
git clone https://github.com/viciusername/toy-shopp.git
cd toy-shopp
```

Push змін:
```bash
git add .
git commit -m "Description of changes"
git push origin main
```

## Забезпечення якості

- MVVM паттерн дотримується архітектури
- Data Binding забезпечує реактивність UI
- INotifyPropertyChanged для оновлення вигляду
- Асинхронні операції без блокування UI
- Обробка помилок та exception handling

## Підтримувані платформи

- ✅ Windows 10/11 (WPF, UWP)
- ✅ Linux (GTK, X11)
- ✅ macOS (Cocoa)
- ✅ iOS (Xamarin)
- ✅ Android (Xamarin)

## Версія

**v1.0.0** - Початкова версія з базовою функціональністю

## Автори

ToyPlanet Development Team

## Дата випуску

16 січня 2026 року