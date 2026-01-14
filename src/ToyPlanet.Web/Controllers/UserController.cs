using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ToyPlanet.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ToyPlanet.Web.Controllers;

[AllowAnonymous]
public class UserController : Controller
{
    private readonly ToyPlanetDbContext _db;
    public UserController(ToyPlanetDbContext db) => _db = db;

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password, string name)
    {
        if (await _db.Users.AnyAsync(u => u.Email == email))
        {
            ModelState.AddModelError("Email", "Пользователь с таким email уже существует.");
            return View();
        }
        var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
        var user = new User { Email = email, PasswordHash = hash, Name = name };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == hash);
        if (user == null)
        {
            ModelState.AddModelError("Email", "Неверный email или пароль.");
            return View();
        }
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Name ?? user.Email),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("UserId", user.Id.ToString())
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        return RedirectToAction("Profile");
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        var orders = await _db.Orders.Where(o => o.UserEmail == email).ToListAsync();
        // TODO: добавить корзину
        ViewBag.User = user;
        ViewBag.Orders = orders;
        return View();
    }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return RedirectToAction("Login");
            return View(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(string name, string email)
        {
            var currentEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == currentEmail);
            if (user == null) return RedirectToAction("Login");
            // Проверка email на уникальность
            if (email != currentEmail && await _db.Users.AnyAsync(u => u.Email == email))
            {
                ModelState.AddModelError("Email", "Этот e-mail уже используется.");
                return View(user);
            }
            user.Name = name;
            user.Email = email;
            await _db.SaveChangesAsync();
            // Обновить куки, если email изменился
            if (email != currentEmail)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                // Можно реализовать автоматический вход, но лучше попросить пользователя войти снова
                return RedirectToAction("Login");
            }
            return RedirectToAction("Profile");
        }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
