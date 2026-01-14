using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ToyPlanet.Data;
using Microsoft.EntityFrameworkCore;

namespace ToyPlanet.Web.Controllers;

[Authorize]
public class CatalogController : Controller
{
    private readonly ToyPlanetDbContext _db;
    
    public CatalogController(ToyPlanetDbContext db)
    {
        _db = db;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Details(int id)
    {
        if (id < 1 || id > 6) return NotFound();
        return View(id);
    }
}
