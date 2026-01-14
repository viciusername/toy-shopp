using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ToyPlanet.Web.Models;
using System.Security.Claims;

namespace ToyPlanet.Web.Controllers;

public class AccountController : Controller
{
    [AllowAnonymous]
    [HttpPost]
    public IActionResult LoginGoogle(string returnUrl = "/")
    {
        return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "Google");
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        
        // Тимчасова авторизація для тестування (без перевірки пароля)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, vm.Username),
            new Claim(ClaimTypes.NameIdentifier, vm.Username),
            new Claim("UserId", "1")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        
        // Тимчасово просто авторизуємо користувача
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, vm.Username),
            new Claim(ClaimTypes.Email, vm.Email),
            new Claim(ClaimTypes.NameIdentifier, vm.Username),
            new Claim(ClaimTypes.MobilePhone, vm.Phone),
            new Claim("UserId", "1")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Registration(RegisterViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        // TODO: створити користувача через Identity/OAuth2
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Profile()
    {
        // TODO: показати дані користувача з claims
        return View();
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
