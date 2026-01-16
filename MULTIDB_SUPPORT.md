# Multi-Database Support для EF Core

## Конфігурація в appsettings.json

```json
{
  "DatabaseProvider": "SqlServer",
  "ConnectionStrings": {
    "SqlServer": "Server=localhost;Database=ToyPlanetDb;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true;",
    "PostgreSQL": "Host=localhost;Port=5432;Database=ToyPlanetDb;Username=postgres;Password=password;",
    "SQLite": "Data Source=toyplanet.db;",
    "InMemory": "InMemoryConnection"
  }
}
```

## Конфігурація DbContext

```csharp
public class ToyPlanetDbContext : DbContext
{
    public DbSet<Toy> Toys { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }

    public ToyPlanetDbContext(DbContextOptions<ToyPlanetDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Seeding даних
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Категорії
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "М'які іграшки" },
            new Category { Id = 2, Name = "Конструктори" },
            new Category { Id = 3, Name = "Транспорт" }
        );

        // Товари
        modelBuilder.Entity<Toy>().HasData(
            new Toy { Id = 1, Name = "Плюшевий ведмідь", Price = 250m, CategoryId = 1 },
            new Toy { Id = 2, Name = "LEGO набір", Price = 1500m, CategoryId = 2 },
            new Toy { Id = 3, Name = "Машинка", Price = 150m, CategoryId = 3 }
        );
    }
}
```

## Startup Configuration

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var provider = configuration["DatabaseProvider"];
        var connectionString = configuration.GetConnectionString(provider);

        return provider switch
        {
            "SqlServer" => services.AddDbContext<ToyPlanetDbContext>(options =>
                options.UseSqlServer(connectionString, b =>
                    b.MigrationsAssembly("ToyPlanet.Web"))),

            "PostgreSQL" => services.AddDbContext<ToyPlanetDbContext>(options =>
                options.UseNpgsql(connectionString, b =>
                    b.MigrationsAssembly("ToyPlanet.Web"))),

            "SQLite" => services.AddDbContext<ToyPlanetDbContext>(options =>
                options.UseSqlite(connectionString, b =>
                    b.MigrationsAssembly("ToyPlanet.Web"))),

            "InMemory" => services.AddDbContext<ToyPlanetDbContext>(options =>
                options.UseInMemoryDatabase("ToyPlanetDb")),

            _ => throw new InvalidOperationException(
                $"Unsupported database provider: {provider}")
        };
    }
}
```

## Program.cs налаштування

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddControllersWithViews();

var app = builder.Build();

// EF Migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ToyPlanetDbContext>();
    db.Database.Migrate();
}

app.Run();
```

## Команди для міграцій

```bash
# SQL Server
dotnet ef migrations add InitialCreate --context ToyPlanetDbContext
dotnet ef database update --context ToyPlanetDbContext

# PostgreSQL
set DatabaseProvider=PostgreSQL
dotnet ef database update

# SQLite
set DatabaseProvider=SQLite
dotnet ef database update

# In-Memory (не потребує міграцій)
set DatabaseProvider=InMemory
```