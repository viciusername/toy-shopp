using System.Collections.Generic;

namespace ToyPlanet.Core.Services;

public class PriceCalculator
{
    private readonly DiscountService _discountService;

    public PriceCalculator(DiscountService discountService)
    {
        _discountService = discountService;
    }

    public decimal CalculateCartTotal(IEnumerable<CartItem> items)
    {
        var subtotal = items.Sum(i => i.LineTotal);
        var discount = _discountService.GetDiscountAmount(subtotal);
        return subtotal - discount;
    }

    public decimal CalculateOrderTotal(Order order)
    {
        var subtotal = order.Total;
        var discount = _discountService.GetDiscountAmount(subtotal);
        return subtotal - discount;
    }
}