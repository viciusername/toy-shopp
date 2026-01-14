using Xunit;
using ToyPlanet.Core;
using ToyPlanet.Core.Services;

public class ModelsAndServicesTests
{
    [Fact]
    public void Category_CreatedWithName()
    {
        var cat = new Category("Dolls", "Soft toys");

        Assert.Equal("Dolls", cat.Name);
        Assert.Equal("Soft toys", cat.Description);
    }

    [Fact]
    public void Order_TotalIsSumOfLines()
    {
        var order = new Order();

        order.AddItem(new Toy { Name = "T1", Price = 100 }, 2);
        order.AddItem(new Toy { Name = "T2", Price = 50 }, 1);

        // 100*2 + 50*1 = 250
        Assert.Equal(250, order.Total);
    }

    [Fact]
    public void Cart_SubtotalAndAddRemove()
    {
        var cart = new Cart();
        var toy = new Toy { Name = "T1", Price = 100 };

        cart.Add(toy, 2);
        Assert.Equal(200, cart.Subtotal());

        cart.Remove(toy);
        Assert.Equal(0, cart.Subtotal());
    }

    [Fact]
    public void DiscountService_GivesCorrectDiscountPercentAndAmount()
    {
        var ds = new DiscountService();

        Assert.Equal(0.10m, ds.GetDiscountPercent(1200));
        Assert.Equal(0.05m, ds.GetDiscountPercent(500));
        Assert.Equal(0m, ds.GetDiscountPercent(100));

        Assert.Equal(120, ds.GetDiscountAmount(1200));
        Assert.Equal(25, ds.GetDiscountAmount(500));
        Assert.Equal(0, ds.GetDiscountAmount(100));
    }

    [Fact]
    public void PriceCalculator_CalculatesTotalWithDiscount()
    {
        var discountService = new DiscountService();
        var priceCalculator = new PriceCalculator(discountService);

        var cart = new Cart();
        cart.Add(new Toy { Name = "T1", Price = 600 }, 1);
        cart.Add(new Toy { Name = "T2", Price = 500 }, 1);

        // subtotal = 1100
        // discount = 10% = 110
        // total = 990
        Assert.Equal(990, priceCalculator.CalculateCartTotal(cart.Items));
    }
}
