using System;
using System.ComponentModel.DataAnnotations;

namespace ToyPlanet.Web.Controllers;

// ============= DTOs для API (Data Transfer Objects) =============

/// <summary>
/// DTO для Toy (игрушка/пони)
/// </summary>
public class ToyDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Guid? CategoryId { get; set; }
    public string CategoryName { get; set; }
}

/// <summary>
/// DTO для Category (категория)
/// </summary>
public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

/// <summary>
/// DTO для User (користувач)
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO для Order (замовлення)
/// </summary>
public class OrderDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? UserId { get; set; }
    public string UserEmail { get; set; }
    public decimal Total { get; set; }
    public int ItemCount { get; set; }
}

/// <summary>
/// DTO для OrderItem (товар в замовленні)
/// </summary>
public class OrderItemDto
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public int ToyId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
