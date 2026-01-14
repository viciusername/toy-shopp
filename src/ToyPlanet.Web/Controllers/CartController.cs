using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ToyPlanet.Data;
using ToyPlanet.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ToyPlanet.Web.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ToyPlanet.Data.ToyPlanetDbContext _db;
    public CartController(ToyPlanet.Data.ToyPlanetDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var cartItems = await _db.CartItems
            .Include(c => c.Toy)
            .Where(c => c.UserId == userId)
            .ToListAsync();
        return View(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int id, string returnUrl = null)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            
            if (userId == 0)
            {
                // Якщо користувач не авторизований, перенаправляємо на логін
                return RedirectToAction("Login", "Account");
            }
            
            var toy = await _db.Toys.FindAsync(id);
            if (toy == null) 
            {
                TempData["Error"] = "Товар не знайдено";
                return RedirectToAction("Index", "Catalog");
            }
            
            var item = await _db.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ToyId == id);
            if (item == null)
            {
                _db.CartItems.Add(new CartItem { UserId = userId, ToyId = id, Quantity = 1 });
            }
            else
            {
                item.Quantity++;
            }
            
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Товар '{toy.Name}' додано до кошика!";
            
            // Якщо returnUrl вказаний і це "cart", перенаправляємо в кошик
            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.ToLower() == "cart")
            {
                return RedirectToAction("Index");
            }
            
            // Інакше залишаємось у каталозі
            return RedirectToAction("Index", "Catalog");
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Помилка додавання товару: {ex.Message}";
            return RedirectToAction("Index", "Catalog");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int id)
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var item = await _db.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ToyId == id);
        if (item != null)
        {
            if (item.Quantity > 1) item.Quantity--;
            else _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveAll(int id)
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var item = await _db.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ToyId == id);
        if (item != null)
        {
            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var cartItems = await _db.CartItems
            .Include(c => c.Toy)
            .Where(c => c.UserId == userId)
            .ToListAsync();
        if (!cartItems.Any())
        {
            TempData["Error"] = "Ваш кошик порожній";
            return RedirectToAction("Index");
        }
        return View(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(string fullName, string address, string postalCode, string phone)
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var cartItems = await _db.CartItems
            .Include(c => c.Toy)
            .Where(c => c.UserId == userId)
            .ToListAsync();
        
        if (!cartItems.Any()) return RedirectToAction("Index");
        
        var order = new Order { UserId = userId, UserEmail = userEmail, CreatedAt = DateTime.UtcNow };
        foreach (var ci in cartItems)
        {
            if (ci.Toy != null)
            {
                order.Items.Add(new OrderItem(ci.Toy, ci.Quantity));
            }
        }
        
        _db.Orders.Add(order);
        _db.CartItems.RemoveRange(cartItems);
        await _db.SaveChangesAsync();
        
        TempData["OrderId"] = order.Id.ToString();
        TempData["FullName"] = fullName;
        TempData["Address"] = address;
        TempData["PostalCode"] = postalCode;
        TempData["Phone"] = phone;
        
        return RedirectToAction("Success");
    }

    public IActionResult Success()
    {
        if (TempData["OrderId"] == null)
        {
            return RedirectToAction("Index", "Catalog");
        }
        return View();
    }
}
