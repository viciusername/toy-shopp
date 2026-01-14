namespace ToyPlanet.Core;

public class CartItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ToyId { get; set; }
    public int Quantity { get; set; }

    // Для работы Cart и PriceCalculator
    public Toy Toy { get; set; } // Связь с объектом Toy
    public decimal LineTotal => Toy != null ? Toy.Price * Quantity : 0;

    public CartItem() { }
    public CartItem(Toy toy, int quantity = 1)
    {
        Toy = toy;
        ToyId = toy != null ? toy.GetHashCode() : 0;
        Quantity = quantity;
    }
}
