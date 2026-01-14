using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ToyPlanet.Web.Controllers;

[Authorize]
public class CatalogController : Controller
{
    public IActionResult Index()
    {
        // Здесь будет список товаров
        return View();
    }
    
    public IActionResult Details(int id)
    {
        // Здесь будет просмотр товара по id
        return View();
    }
}
