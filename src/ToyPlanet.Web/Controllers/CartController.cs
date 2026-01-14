using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ToyPlanet.Data;
using ToyPlanet.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

[Authorize]
public class CartController : Controller
{
    private readonly ToyPlanet.Data.ToyPlanetDbContext _db;
    public CartController(ToyPlanet.Data.ToyPlanetDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var cartItems = await _db.CartItems.Where(c => c.UserId == userId).ToListAsync();
        ViewBag.CartItems = cartItems;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(int id)
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var item = await _db.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ToyId == id);
        if (item == null)
            _db.CartItems.Add(new CartItem { UserId = userId, ToyId = id, Quantity = 1 });
        else
            item.Quantity++;
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
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
    public async Task<IActionResult> Checkout()
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var cartItems = await _db.CartItems.Where(c => c.UserId == userId).ToListAsync();
        if (!cartItems.Any()) return RedirectToAction("Index");
        var order = new Order { UserId = userId, UserEmail = userEmail, CreatedAt = DateTime.UtcNow };
        foreach (var ci in cartItems)
        {
            order.Items.Add(new OrderItem(new Toy { Id = ci.ToyId }, ci.Quantity));
        }
        _db.Orders.Add(order);
        _db.CartItems.RemoveRange(cartItems);
        await _db.SaveChangesAsync();
        TempData["OrderSuccess"] = true;
        return RedirectToAction("Index");
    }
}
