using Microsoft.AspNetCore.Mvc;
using ToyPlanet.Data;
using ToyPlanet.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ToyPlanet.Web.Controllers;

public class CategoriesController : Controller
{
    private readonly ToyPlanet.Data.ToyPlanetDbContext _db;
    
    public CategoriesController(ToyPlanet.Data.ToyPlanetDbContext db) => _db = db;

    // GET: Categories/Index - Список всих категорій
    public async Task<IActionResult> Index()
    {
        var categories = await _db.Categories.ToListAsync();
        return View(categories);
    }

    // GET: Categories/Details/5 - Деталі категорії з товарами
    public async Task<IActionResult> Details(Guid id)
    {
        var category = await _db.Categories
            .Include(c => c.Toys) // Завантажити пов'язані товари
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound();

        return View(category);
    }
}
