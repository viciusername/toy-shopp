using Microsoft.AspNetCore.Mvc;
using ToyPlanet.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ToyPlanet.Web.Controllers;

/// <summary>
/// API контролер для замовлень
/// </summary>
[ApiController]
[Route("api/v1/orders")]
public class OrdersApiController : ControllerBase
{
    private readonly ToyPlanet.Data.ToyPlanetDbContext _db;

    public OrdersApiController(ToyPlanet.Data.ToyPlanetDbContext db) => _db = db;

    /// <summary>
    /// Отримати всі замовлення
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<OrderDto>), 200)]
    public async Task<ActionResult<List<OrderDto>>> GetAll()
    {
        var orders = await _db.Orders
            .Include(o => o.Items)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                CreatedAt = o.CreatedAt,
                UserId = o.UserId,
                UserEmail = o.UserEmail,
                Total = o.Total,
                ItemCount = o.Items.Count
            })
            .ToListAsync();

        return Ok(orders);
    }

    /// <summary>
    /// Отримати замовлення по ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<OrderDto>> GetById(Guid id)
    {
        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound(new { message = "Замовлення не найдено" });

        return Ok(new OrderDto
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            UserId = order.UserId,
            UserEmail = order.UserEmail,
            Total = order.Total,
            ItemCount = order.Items.Count
        });
    }

    /// <summary>
    /// Отримати замовлення користувача
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(List<OrderDto>), 200)]
    public async Task<ActionResult<List<OrderDto>>> GetByUser(int userId)
    {
        var orders = await _db.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                CreatedAt = o.CreatedAt,
                UserId = o.UserId,
                UserEmail = o.UserEmail,
                Total = o.Total,
                ItemCount = o.Items.Count
            })
            .ToListAsync();

        return Ok(orders);
    }
}

/// <summary>
/// API контролер для категорій
/// </summary>
[ApiController]
[Route("api/v1/categories")]
public class CategoriesApiController : ControllerBase
{
    private readonly ToyPlanet.Data.ToyPlanetDbContext _db;

    public CategoriesApiController(ToyPlanet.Data.ToyPlanetDbContext db) => _db = db;

    /// <summary>
    /// Отримати всі категорії
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryDto>), 200)]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    {
        var categories = await _db.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            })
            .ToListAsync();

        return Ok(categories);
    }

    /// <summary>
    /// Отримати категорію по ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CategoryDto>> GetById(Guid id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category == null)
            return NotFound(new { message = "Категорія не найдена" });

        return Ok(new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        });
    }

    /// <summary>
    /// Створити нову категорію
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CategoryDto categoryDto)
    {
        if (string.IsNullOrWhiteSpace(categoryDto.Name))
            return BadRequest(new { message = "Назва категорії не може бути порожною" });

        var category = new ToyPlanet.Core.Category(categoryDto.Name, categoryDto.Description);
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        categoryDto.Id = category.Id;
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, categoryDto);
    }
}

/// <summary>
/// API контролер для користувачів
/// </summary>
[ApiController]
[Route("api/v1/users")]
public class UsersApiController : ControllerBase
{
    private readonly ToyPlanet.Data.ToyPlanetDbContext _db;

    public UsersApiController(ToyPlanet.Data.ToyPlanetDbContext db) => _db = db;

    /// <summary>
    /// Отримати всіх користувачів
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserDto>), 200)]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        var users = await _db.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        return Ok(users);
    }

    /// <summary>
    /// Отримати користувача по ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { message = "Користувач не найден" });

        return Ok(new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            CreatedAt = user.CreatedAt
        });
    }

    /// <summary>
    /// Пошук користувача по email
    /// </summary>
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<UserDto>> GetByEmail(string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return NotFound(new { message = "Користувач з таким email не найден" });

        return Ok(new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            CreatedAt = user.CreatedAt
        });
    }
}
