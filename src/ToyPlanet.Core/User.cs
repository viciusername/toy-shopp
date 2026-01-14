using System.ComponentModel.DataAnnotations;
using System;

namespace ToyPlanet.Core;

public class User
{
    public int Id { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string PasswordHash { get; set; } = null!;
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
