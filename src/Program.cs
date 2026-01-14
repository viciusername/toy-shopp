using ToyPlanet.Core;
using ToyPlanet.Core.Services;

var toy1 = new Toy { Name = "Медвежонок", Price = 600 };
var toy2 = new Toy { Name = "Кукла", Price = 500 };

var cart = new Cart();
cart.Add(toy1, 1);
cart.Add(toy2, 1);

var discountService = new DiscountService();
var priceCalc = new PriceCalculator(discountService);

Console.WriteLine($"Товаров в корзине: {cart.Items.Count}");
Console.WriteLine($"Сумма без скидки: {cart.Subtotal()}");
Console.WriteLine($"Скидка: {discountService.GetDiscountAmount(cart.Subtotal())}");
Console.WriteLine($"Итого к оплате: {priceCalc.CalculateCartTotal(cart.Items)}");