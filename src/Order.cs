using System.Collections.Generic;

namespace ToyPlanet.Core;

public class OrderItem
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public int ToyId { get; set; }
    public Toy? Toy { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public OrderItem()
    {
    }

    public OrderItem(Toy toy, int quantity)
    {
        Toy = toy;
        ToyId = toy.Id;
        Quantity = quantity;
        Price = toy.Price;
    }

    public decimal LineTotal => Price * Quantity;
}

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? UserId { get; set; }
    public string? UserEmail { get; set; }

    public void AddItem(Toy toy, int quantity = 1)
    {
        Items.Add(new OrderItem(toy, quantity));
    }

    public decimal Total => Items.Sum(i => i.LineTotal);
}