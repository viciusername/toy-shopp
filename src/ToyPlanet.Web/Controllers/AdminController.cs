using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ToyPlanet.Data;
using ToyPlanet.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ToyPlanet.Web.Controllers;

// Тимчасово доступно всім для демонстрації
[AllowAnonymous]
public class AdminController : Controller
{
    private readonly ToyPlanet.Data.ToyPlanetDbContext _db;
    public AdminController(ToyPlanet.Data.ToyPlanetDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var toys = await _db.Toys.ToListAsync();
        var users = await _db.Users.ToListAsync();
        ViewBag.Toys = toys;
        ViewBag.Users = users;
        return View();
    }

    // CRUD для товаров
    [HttpGet]
    public async Task<IActionResult> EditToy(int id)
    {
        var toy = await _db.Toys.FindAsync(id);
        if (toy == null) return NotFound();
        return View(toy);
    }
    [HttpPost]
    public async Task<IActionResult> EditToy(Toy toy)
    {
        _db.Toys.Update(toy);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult CreateToy() => View();
    [HttpPost]
    public async Task<IActionResult> CreateToy(Toy toy)
    {
        _db.Toys.Add(toy);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> DeleteToy(int id)
    {
        var toy = await _db.Toys.FindAsync(id);
        if (toy != null)
        {
            _db.Toys.Remove(toy);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    // Просмотр и удаление пользователей
    [HttpPost]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user != null)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
