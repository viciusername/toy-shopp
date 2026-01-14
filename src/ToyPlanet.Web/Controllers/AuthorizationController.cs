using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace ToyPlanet.Web.Controllers;

public class AuthorizationController : Controller
{
    [HttpGet("~/connect/authorize")]
    [AllowAnonymous]
    public async Task<IActionResult> Authorize()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        // TODO: Перевірити, чи користувач аутентифікований
        // Якщо ні - редирект на логін
        if (!User.Identity?.IsAuthenticated ?? true)
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                    Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
            });
        }

        // Створити claims principal
        var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "user"));
        identity.AddClaim(new Claim(OpenIddictConstants.Claims.Name, User.Identity.Name ?? "User"));

        var principal = new ClaimsPrincipal(identity);

        // Встановити scopes
        principal.SetScopes(new[]
        {
            OpenIddictConstants.Scopes.OpenId,
            OpenIddictConstants.Scopes.Profile,
            OpenIddictConstants.Scopes.Email
        }.Intersect(request.GetScopes()));

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpPost("~/connect/token")]
    [AllowAnonymous]
    [Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
        {
            // Отримати ClaimsPrincipal з токену
            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // TODO: Перевірити, чи користувач все ще існує в БД

            return SignIn(result.Principal!, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        else if (request.IsClientCredentialsGrantType())
        {
            var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, request.ClientId ?? "client"));

            var principal = new ClaimsPrincipal(identity);
            principal.SetScopes(request.GetScopes());

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        throw new InvalidOperationException("The specified grant type is not supported.");
    }

    [HttpGet("~/connect/userinfo")]
    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Userinfo()
    {
        var claims = new Dictionary<string, object>
        {
            [OpenIddictConstants.Claims.Subject] = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "user",
            [OpenIddictConstants.Claims.Name] = User.Identity?.Name ?? "User",
            [OpenIddictConstants.Claims.Email] = User.FindFirstValue(ClaimTypes.Email) ?? "user@example.com"
        };

        return Ok(claims);
    }
}
