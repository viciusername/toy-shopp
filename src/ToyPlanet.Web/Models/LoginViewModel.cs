using System.ComponentModel.DataAnnotations;

namespace ToyPlanet.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Ім'я користувача обов'язкове")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обов'язковий")]
    public string Password { get; set; } = string.Empty;
}
