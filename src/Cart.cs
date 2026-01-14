using System.Collections.Generic;

namespace ToyPlanet.Core;


public class Cart
{
    public List<CartItem> Items { get; } = new List<CartItem>();

    public void Add(Toy toy, int quantity = 1)
    {
        var existing = Items.FirstOrDefault(i => i.Toy == toy);
        if (existing != null) existing.Quantity += quantity;
        else Items.Add(new CartItem(toy, quantity));
    }

    public void Remove(Toy toy)
    {
        Items.RemoveAll(i => i.Toy == toy);
    }

    public decimal Subtotal() => Items.Sum(i => i.LineTotal);
}