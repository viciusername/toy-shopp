using System.ComponentModel.DataAnnotations;

namespace ToyPlanet.Web.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ім'я користувача обов'язкове")]
    [StringLength(50, ErrorMessage = "Ім'я користувача не може перевищувати 50 символів")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Повне ім'я обов'язкове")]
    [StringLength(500, ErrorMessage = "Повне ім'я не може перевищувати 500 символів")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обов'язковий")]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "Пароль має бути від 8 до 16 символів")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).+$",
        ErrorMessage = "Пароль має містити велику літеру, цифру та знак.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Підтвердження пароля обов'язкове")]
    [Compare("Password", ErrorMessage = "Паролі не співпадають")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Телефон обов'язковий")]
    [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Телефон у форматі +380XXXXXXXXX")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обов'язковий")]
    [EmailAddress(ErrorMessage = "Невірний формат email")]
    public string Email { get; set; } = string.Empty;
}
