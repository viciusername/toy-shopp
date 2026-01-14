using Microsoft.EntityFrameworkCore;

namespace ToyPlanet.Core;


public class ToyPlanetDbContext : DbContext
{
    public DbSet<Toy> Toys { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<User> Users { get; set; }

    public ToyPlanetDbContext(DbContextOptions<ToyPlanetDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Seed example data
        modelBuilder.Entity<Category>().HasData(new Category("Мягкие игрушки") { Id = Guid.NewGuid() });
    }
}
