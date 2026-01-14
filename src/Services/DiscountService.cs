namespace ToyPlanet.Core.Services;

public class DiscountService
{
    // Simple rules: 10% off for totals >= 1000, 5% off for totals >= 500
    public decimal GetDiscountPercent(decimal subtotal)
    {
        if (subtotal >= 1000) return 0.10m;
        if (subtotal >= 500) return 0.05m;
        return 0m;
    }

    public decimal GetDiscountAmount(decimal subtotal) => Math.Round(subtotal * GetDiscountPercent(subtotal), 2);
}