using Microsoft.AspNetCore.Mvc;

namespace ToyPlanet.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    private static readonly object[] Catalog = new[] {
        new { Id=1, Name="Пони 1", Price=1990, Description="Очаровательная пастельная пони с мягкой гривой." },
        new { Id=2, Name="Пони 2", Price=2490, Description="Пастельная пони с блестящей гривой и хвостом." },
        new { Id=3, Name="Пони 3", Price=2290, Description="Яркая пони с уникальным окрасом." },
        new { Id=4, Name="Пони 4", Price=2590, Description="Пони с золотистой гривой и добрым взглядом." },
        new { Id=5, Name="Пони 5", Price=2990, Description="Эксклюзивная пони с радужной гривой." },
        new { Id=6, Name="Пони 6", Price=3990, Description="Самая редкая пони в коллекции ToyPlanet." }
    };

    [HttpGet("catalog")]
    public IActionResult GetCatalog() => Ok(Catalog);

    [HttpGet("catalog/{id}")]
    public IActionResult GetPony(int id)
    {
        var pony = Catalog.FirstOrDefault(x => (int)x.GetType().GetProperty("Id")!.GetValue(x)! == id);
        if (pony == null) return NotFound();
        return Ok(pony);
    }
}
