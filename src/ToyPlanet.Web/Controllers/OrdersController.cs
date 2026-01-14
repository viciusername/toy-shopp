using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ToyPlanet.Data;
using ToyPlanet.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ToyPlanet.Web.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly ToyPlanet.Data.ToyPlanetDbContext _db;
    public OrdersController(ToyPlanet.Data.ToyPlanetDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var orders = await _db.Orders.Where(o => o.UserId == userId).ToListAsync();
        ViewBag.Orders = orders;
        return View();
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
        if (order == null) return NotFound();
        ViewBag.Order = order;
        return View();
    }
}
