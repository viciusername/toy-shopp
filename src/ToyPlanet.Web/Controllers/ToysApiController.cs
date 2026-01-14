using Microsoft.AspNetCore.Mvc;
using ToyPlanet.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ToyPlanet.Web.Controllers;

/// <summary>
/// API контролер для товарів (версія 1)
/// </summary>
[ApiController]
[Route("api/v1/toys")]
public class ToysController : ControllerBase
{
    private readonly ToyPlanet.Data.ToyPlanetDbContext _db;

    public ToysController(ToyPlanet.Data.ToyPlanetDbContext db) => _db = db;

    /// <summary>
    /// Отримати всі товари (v1 - простий список)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ToyDto>), 200)]
    public async Task<ActionResult<List<ToyDto>>> GetAll()
    {
        var toys = await _db.Toys
            .Select(t => new ToyDto
            {
                Id = t.Id,
                Name = t.Name,
                Price = t.Price,
                CategoryId = t.CategoryId,
                CategoryName = t.CategoryName
            })
            .ToListAsync();

        return Ok(toys);
    }

    /// <summary>
    /// Отримати товар по ID (v1)
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ToyDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ToyDto>> GetById(int id)
    {
        var toy = await _db.Toys.FindAsync(id);
        if (toy == null)
            return NotFound(new { message = "Товар не найден" });

        return Ok(new ToyDto
        {
            Id = toy.Id,
            Name = toy.Name,
            Price = toy.Price,
            CategoryId = toy.CategoryId,
            CategoryName = toy.CategoryName
        });
    }

    /// <summary>
    /// Отримати товари за категорією (v1)
    /// </summary>
    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(typeof(List<ToyDto>), 200)]
    public async Task<ActionResult<List<ToyDto>>> GetByCategory(Guid categoryId)
    {
        var toys = await _db.Toys
            .Where(t => t.CategoryId == categoryId)
            .Select(t => new ToyDto
            {
                Id = t.Id,
                Name = t.Name,
                Price = t.Price,
                CategoryId = t.CategoryId,
                CategoryName = t.CategoryName
            })
            .ToListAsync();

        return Ok(toys);
    }

    /// <summary>
    /// Створити новий товар (v1)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ToyDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ToyDto>> Create([FromBody] ToyDto toyDto)
    {
        if (string.IsNullOrWhiteSpace(toyDto.Name))
            return BadRequest(new { message = "Назва товара не може бути порожною" });

        var toy = new ToyPlanet.Core.Toy
        {
            Name = toyDto.Name,
            Price = toyDto.Price,
            CategoryId = toyDto.CategoryId,
            CategoryName = toyDto.CategoryName
        };

        _db.Toys.Add(toy);
        await _db.SaveChangesAsync();

        toyDto.Id = toy.Id;
        return CreatedAtAction(nameof(GetById), new { id = toy.Id }, toyDto);
    }

    /// <summary>
    /// Оновити товар (v1)
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(int id, [FromBody] ToyDto toyDto)
    {
        var toy = await _db.Toys.FindAsync(id);
        if (toy == null)
            return NotFound(new { message = "Товар не найден" });

        toy.Name = toyDto.Name;
        toy.Price = toyDto.Price;
        toy.CategoryId = toyDto.CategoryId;
        toy.CategoryName = toyDto.CategoryName;

        _db.Toys.Update(toy);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Товар успішно оновлено" });
    }

    /// <summary>
    /// Видалити товар (v1)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var toy = await _db.Toys.FindAsync(id);
        if (toy == null)
            return NotFound(new { message = "Товар не найден" });

        _db.Toys.Remove(toy);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}

/// <summary>
/// API контролер для товарів (версія 2)
/// Розширена версія з додатковою інформацією про категорії
/// </summary>
[ApiController]
[Route("api/v2/toys")]
public class ToysV2Controller : ControllerBase
{
    private readonly ToyPlanet.Data.ToyPlanetDbContext _db;

    public ToysV2Controller(ToyPlanet.Data.ToyPlanetDbContext db) => _db = db;

    /// <summary>
    /// Отримати всі товари з деталями категорії (v2)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ToyWithCategoryDto>), 200)]
    public async Task<ActionResult<List<ToyWithCategoryDto>>> GetAllWithDetails()
    {
        var toys = await _db.Toys
            .Include(t => t.Category)
            .Select(t => new ToyWithCategoryDto
            {
                Id = t.Id,
                Name = t.Name,
                Price = t.Price,
                Category = new CategoryDto
                {
                    Id = t.Category.Id,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }
            })
            .ToListAsync();

        return Ok(toys);
    }

    /// <summary>
    /// Отримати товар з деталями (v2)
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ToyWithCategoryDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ToyWithCategoryDto>> GetByIdWithDetails(int id)
    {
        var toy = await _db.Toys
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (toy == null)
            return NotFound(new { message = "Товар не найден" });

        return Ok(new ToyWithCategoryDto
        {
            Id = toy.Id,
            Name = toy.Name,
            Price = toy.Price,
            Category = toy.Category != null ? new CategoryDto
            {
                Id = toy.Category.Id,
                Name = toy.Category.Name,
                Description = toy.Category.Description
            } : null
        });
    }

    /// <summary>
    /// Отримати товари за ціновим діапазоном (v2)
    /// </summary>
    [HttpGet("price-range")]
    [ProducesResponseType(typeof(List<ToyWithCategoryDto>), 200)]
    public async Task<ActionResult<List<ToyWithCategoryDto>>> GetByPriceRange([FromQuery] decimal minPrice = 0, [FromQuery] decimal maxPrice = decimal.MaxValue)
    {
        var toys = await _db.Toys
            .Include(t => t.Category)
            .Where(t => t.Price >= minPrice && t.Price <= maxPrice)
            .Select(t => new ToyWithCategoryDto
            {
                Id = t.Id,
                Name = t.Name,
                Price = t.Price,
                Category = new CategoryDto
                {
                    Id = t.Category.Id,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }
            })
            .ToListAsync();

        return Ok(toys);
    }
}

/// <summary>
/// DTO для товара з категорією (v2 специфічний)
/// </summary>
public class ToyWithCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public CategoryDto Category { get; set; }
}
