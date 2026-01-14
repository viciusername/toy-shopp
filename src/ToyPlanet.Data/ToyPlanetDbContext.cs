using Microsoft.EntityFrameworkCore;
using ToyPlanet.Core;
using OpenIddict.EntityFrameworkCore.Models;

namespace ToyPlanet.Data
{
    public class ToyPlanetDbContext : DbContext
    {
        public ToyPlanetDbContext(DbContextOptions<ToyPlanetDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Toy> Toys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.Toy)
                    .WithMany()
                    .HasForeignKey(e => e.ToyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Налаштування Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasMany(e => e.Items)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Налаштування Toy -> Category
            modelBuilder.Entity<Toy>(entity =>
            {
                entity.HasOne(t => t.Category)
                    .WithMany(c => c.Toys)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ========== SEED DATA - ПОНІ ІГРАШКИ ==========
            
            // Категорії Поні
            var categoryId1 = Guid.NewGuid();
            var categoryId2 = Guid.NewGuid();
            var categoryId3 = Guid.NewGuid();
            var categoryId4 = Guid.NewGuid();

            var categories = new[]
            {
                new Category("🌈 Поні-герої", "Улюблені персонажі з мультфільму My Little Pony") 
                { 
                    Id = categoryId1 
                },
                new Category("🦄 Легендарні Поні", "Рідкісні та легендарні поні-мегзвезди") 
                { 
                    Id = categoryId2 
                },
                new Category("💎 Колекціонерські Поні", "Дорогоцінні та обмежені видання") 
                { 
                    Id = categoryId3 
                },
                new Category("✨ Крилаті Поні", "Пегаси та їх чарівні варіанти") 
                { 
                    Id = categoryId4 
                }
            };

            modelBuilder.Entity<Category>().HasData(categories);

            // Товари - Поні іграшки
            var toys = new[]
            {
                new Toy { Id = 1, Name = "Rainbow Dash 🌈", Price = 450m, CategoryId = categoryId1, CategoryName = "🌈 Поні-герої" },
                new Toy { Id = 2, Name = "Twilight Sparkle ⭐", Price = 520m, CategoryId = categoryId1, CategoryName = "🌈 Поні-герої" },
                new Toy { Id = 3, Name = "Applejack 🍎", Price = 380m, CategoryId = categoryId1, CategoryName = "🌈 Поні-герої" },
                new Toy { Id = 4, Name = "Fluttershy 🦋", Price = 420m, CategoryId = categoryId1, CategoryName = "🌈 Поні-герої" },
                new Toy { Id = 5, Name = "Pinkie Pie 🎉", Price = 480m, CategoryId = categoryId1, CategoryName = "🌈 Поні-герої" },
                
                new Toy { Id = 6, Name = "Celestia - Королева Сонця 👑", Price = 890m, CategoryId = categoryId2, CategoryName = "🦄 Легендарні Поні" },
                new Toy { Id = 7, Name = "Luna - Королева Місяця 🌙", Price = 890m, CategoryId = categoryId2, CategoryName = "🦄 Легендарні Поні" },
                new Toy { Id = 8, Name = "Discord - Дух Хаосу 🎭", Price = 750m, CategoryId = categoryId2, CategoryName = "🦄 Легендарні Поні" },
                
                new Toy { Id = 9, Name = "Поні 24K Золото (限定版)", Price = 2500m, CategoryId = categoryId3, CategoryName = "💎 Колекціонерські Поні" },
                new Toy { Id = 10, Name = "Кристальна Поні (Дорогоцінна)", Price = 1850m, CategoryId = categoryId3, CategoryName = "💎 Колекціонерські Поні" },
                new Toy { Id = 11, Name = "Алмазна Поні Редакція", Price = 3200m, CategoryId = categoryId3, CategoryName = "💎 Колекціонерські Поні" },
                
                new Toy { Id = 12, Name = "Pegasus White Wings ❄️", Price = 550m, CategoryId = categoryId4, CategoryName = "✨ Крилаті Поні" },
                new Toy { Id = 13, Name = "Спектра - Райдужне Крило", Price = 620m, CategoryId = categoryId4, CategoryName = "✨ Крилаті Поні" },
                new Toy { Id = 14, Name = "Skystar - Королева Хмар", Price = 780m, CategoryId = categoryId4, CategoryName = "✨ Крилаті Поні" }
            };

            modelBuilder.Entity<Toy>().HasData(toys);

            // Користувачі (демо)
            var userId1 = 1;
            var userId2 = 2;

            var users = new[]
            {
                new User { Id = userId1, Email = "pony.lover@example.com", Name = "Поніш Петренко", PasswordHash = "hashed_password_1", CreatedAt = DateTime.UtcNow.AddDays(-30) },
                new User { Id = userId2, Email = "rainbow.fan@example.com", Name = "Райнбоу Сидоренко", PasswordHash = "hashed_password_2", CreatedAt = DateTime.UtcNow.AddDays(-15) }
            };

            modelBuilder.Entity<User>().HasData(users);

            // Замовлення (демо з 2+ JOIN операціями)
            var orderId1 = Guid.NewGuid();
            var orderId2 = Guid.NewGuid();
            var orderId3 = Guid.NewGuid();

            var orders = new[]
            {
                new Order 
                { 
                    Id = orderId1, 
                    UserId = userId1, 
                    UserEmail = "pony.lover@example.com",
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new Order 
                { 
                    Id = orderId2, 
                    UserId = userId1, 
                    UserEmail = "pony.lover@example.com",
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new Order 
                { 
                    Id = orderId3, 
                    UserId = userId2, 
                    UserEmail = "rainbow.fan@example.com",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                }
            };

            modelBuilder.Entity<Order>().HasData(orders);

            // OrderItems (демо)
            var orderItems = new[]
            {
                new OrderItem { Id = 1, OrderId = orderId1, ToyId = 1, Quantity = 1, Price = 450m },
                new OrderItem { Id = 2, OrderId = orderId1, ToyId = 4, Quantity = 1, Price = 420m },
                
                new OrderItem { Id = 3, OrderId = orderId2, ToyId = 7, Quantity = 1, Price = 890m },
                new OrderItem { Id = 4, OrderId = orderId2, ToyId = 12, Quantity = 1, Price = 550m },
                
                new OrderItem { Id = 5, OrderId = orderId3, ToyId = 2, Quantity = 1, Price = 520m },
                new OrderItem { Id = 6, OrderId = orderId3, ToyId = 9, Quantity = 1, Price = 2500m }
            };

            modelBuilder.Entity<OrderItem>().HasData(orderItems);

            // Додати OpenIddict до моделі
            modelBuilder.UseOpenIddict();
        }
    }
}
