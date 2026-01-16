# Кросс-платформний клієнт ToyPlanet (Xamarin, Avalonia, .NET MAUI)

## Кроки для розробки:

### 1. Вибір технології та налаштування проекту

#### Для Xamarin:
```bash
dotnet new xamarin-shell -n ToyPlanetXamarin
```

#### Для Avalonia:
```bash
dotnet new avalonia -n ToyPlanetAvalonia
```

#### Для .NET MAUI:
```bash
dotnet new maui -n ToyPlanetMAUI
```

### 2. Структура проекту

```
ToyPlanet.Client/
├── Models/
│   ├── Toy.cs
│   ├── Category.cs
│   ├── Order.cs
│   └── User.cs
├── Views/
│   ├── LoginPage.xaml
│   ├── CatalogPage.xaml
│   ├── CartPage.xaml
│   ├── OrdersPage.xaml
│   ├── StatisticsPage.xaml
│   ├── AboutPage.xaml
│   └── LoadingPage.xaml
├── ViewModels/
│   ├── LoginViewModel.cs
│   ├── CatalogViewModel.cs
│   ├── CartViewModel.cs
│   ├── OrdersViewModel.cs
│   ├── StatisticsViewModel.cs
│   └── AboutViewModel.cs
├── Services/
│   ├── AuthenticationService.cs
│   ├── ApiService.cs
│   └── IdentityServerService.cs
└── App.xaml
```

### 3. Встановлення залежностей

```bash
dotnet add package IdentityModel
dotnet add package HttpClientFactory
dotnet add package CommunityToolkit.Mvvm
dotnet add package OxyPlot
dotnet add package LiveCharts2
```

### 4. Функціональність

#### a. Введення даних у 3 таблиці:
- Toys (назва, ціна, категорія)
- Categories (назва, опис)
- Orders (дата, користувач, кошик)

#### b. Граф на основі даних:
- Графік продаж по категоріям
- Графік заказів по дням

#### c. About вікно:
- Інформація про додаток
- Версія
- Автори

#### d. GitHub:
- Клонування з репозиторію
- Push змін

#### e. MVVM паттерн:
- ViewModel базовий клас
- Binding для UI
- INotifyPropertyChanged