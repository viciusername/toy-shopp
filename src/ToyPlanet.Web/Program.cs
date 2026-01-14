using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using ToyPlanet.Data;
using OpenIddict.Validation.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ToyPlanet API v1",
        Version = "1.0",
        Description = "API для роботи з іграшками (базова версія)"
    });
    
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "ToyPlanet API v2",
        Version = "2.0",
        Description = "API для роботи з іграшками (розширена версія з деталями категорій)"
    });
});
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Добавить DbContext с выбором провайдера
var provider = builder.Configuration["DatabaseProvider"] ?? "Sqlite";
var cs = builder.Configuration.GetConnectionString(provider);

switch (provider)
{
    case "MSSQL":
        builder.Services.AddDbContext<ToyPlanetDbContext>(o => o.UseSqlServer(cs));
        break;
    case "Postgres":
        builder.Services.AddDbContext<ToyPlanetDbContext>(o => o.UseNpgsql(cs));
        break;
    case "Sqlite":
        builder.Services.AddDbContext<ToyPlanetDbContext>(o => o.UseSqlite(cs));
        break;
    case "InMemory":
        builder.Services.AddDbContext<ToyPlanetDbContext>(o => o.UseInMemoryDatabase("ToyPlanetDb"));
        break;
    default:
        throw new Exception($"Unknown DatabaseProvider: {provider}");
}

// Настройка OpenIddict
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<ToyPlanetDbContext>();
    })
    .AddServer(options =>
    {
        options.SetTokenEndpointUris("/connect/token")
            .SetAuthorizationEndpointUris("/connect/authorize")
            .SetUserinfoEndpointUris("/connect/userinfo");

        options.AllowAuthorizationCodeFlow()
            .AllowRefreshTokenFlow()
            .AllowClientCredentialsFlow();

        options.AddEncryptionKey(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            Convert.FromBase64String(builder.Configuration["OAuth2:OpenIddict:EncryptionKey"] ?? "DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));

        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        options.UseAspNetCore()
            .EnableTokenEndpointPassthrough()
            .EnableAuthorizationEndpointPassthrough()
            .EnableUserinfoEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

// Добавить аутентификацию через OAuth2/OpenID Connect
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["OAuth2:Google:ClientId"] ?? "YOUR_GOOGLE_CLIENT_ID";
    options.ClientSecret = builder.Configuration["OAuth2:Google:ClientSecret"] ?? "YOUR_GOOGLE_CLIENT_SECRET";
    options.CallbackPath = "/signin-google";
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://demo.duendesoftware.com/";
    options.ClientId = "interactive.public";
    options.ResponseType = "code";
    options.SaveTokens = true;
});

// Додати hosted service для ініціалізації OpenIddict
builder.Services.AddHostedService<ToyPlanet.Web.OpenIddictWorker>();

var app = builder.Build();

// Ініціалізація бази даних з тестовими даними
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ToyPlanetDbContext>();
    db.Database.EnsureCreated();
    
    // Додаємо товари якщо їх ще немає
    if (!db.Toys.Any())
    {
        db.Toys.AddRange(
            new ToyPlanet.Core.Toy { Id = 1, Name = "Поні 1", Price = 1990 },
            new ToyPlanet.Core.Toy { Id = 2, Name = "Поні 2", Price = 2490 },
            new ToyPlanet.Core.Toy { Id = 3, Name = "Поні 3", Price = 2290 },
            new ToyPlanet.Core.Toy { Id = 4, Name = "Поні 4", Price = 2590 },
            new ToyPlanet.Core.Toy { Id = 5, Name = "Поні 5", Price = 2990 },
            new ToyPlanet.Core.Toy { Id = 6, Name = "Поні 6", Price = 3990 }
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ToyPlanet API v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "ToyPlanet API v2");
        options.RoutePrefix = "swagger";
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { }
