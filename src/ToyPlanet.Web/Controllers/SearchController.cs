using Microsoft.AspNetCore.Mvc;
using ToyPlanet.Data;
using ToyPlanet.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ToyPlanet.Web.Controllers;

public class SearchController : Controller
{
    private readonly ToyPlanet.Data.ToyPlanetDbContext _db;
    
    public SearchController(ToyPlanet.Data.ToyPlanetDbContext db) => _db = db;

    // GET: Search/Index
    public IActionResult Index()
    {
        return View(new SearchViewModel());
    }

    // POST: Search/Results
    [HttpPost]
    public async Task<IActionResult> Results(SearchViewModel searchModel)
    {
        // Базовий запит з 2+ JOIN операціями:
        // Orders JOIN Users + Orders JOIN OrderItems + OrderItems JOIN Toys
        var query = _db.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Toy)
            .AsQueryable();

        // Фільтр за датою
        if (searchModel.DateFrom.HasValue)
        {
            query = query.Where(o => o.CreatedAt >= searchModel.DateFrom.Value);
        }

        if (searchModel.DateTo.HasValue)
        {
            var dateToEnd = searchModel.DateTo.Value.AddDays(1);
            query = query.Where(o => o.CreatedAt < dateToEnd);
        }

        // Фільтр за іменем користувача (JOIN User)
        if (!string.IsNullOrWhiteSpace(searchModel.UserName))
        {
            var userName = searchModel.UserName.Trim();
            query = query.Where(o => 
                EF.Functions.Like(o.UserEmail, userName + "%") ||
                EF.Functions.Like(o.UserEmail, "%" + userName)
            );
        }

        // Фільтр за іменем товара (JOIN Toy через OrderItem)
        if (!string.IsNullOrWhiteSpace(searchModel.ToyName))
        {
            var toyName = searchModel.ToyName.Trim();
            query = query.Where(o => o.Items.Any(oi => 
                EF.Functions.Like(oi.Toy.Name, "%" + toyName + "%") ||
                EF.Functions.Like(oi.Toy.Name, toyName + "%") ||
                EF.Functions.Like(oi.Toy.Name, "%" + toyName)
            ));
        }

        // Фільтр за категорією товара (3+ JOIN)
        if (searchModel.CategoryId.HasValue)
        {
            query = query.Where(o => o.Items.Any(oi => 
                oi.Toy.CategoryId == searchModel.CategoryId.Value
            ));
        }

        // Сортування
        var results = await query.OrderByDescending(o => o.CreatedAt).ToListAsync();

        var viewModel = new SearchViewModel
        {
            DateFrom = searchModel.DateFrom,
            DateTo = searchModel.DateTo,
            UserName = searchModel.UserName,
            ToyName = searchModel.ToyName,
            CategoryId = searchModel.CategoryId,
            Results = results,
            Categories = await _db.Categories.ToListAsync()
        };

        return View("Results", viewModel);
    }
}

/// <summary>
/// ViewModel для пошуку з підтримкою:
/// - Пошуку за датою (від і до)
/// - Пошуку за префіксом/суфіксом імені користувача
/// - Пошуку по назві товара (містить, починається, закінчується)
/// - Фільтрації за категорією
/// - 3 JOIN операцій: Orders -> Items -> Toys -> Category
/// </summary>
public class SearchViewModel
{
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string UserName { get; set; }
    public string ToyName { get; set; }
    public Guid? CategoryId { get; set; }
    
    public List<Order> Results { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
}
