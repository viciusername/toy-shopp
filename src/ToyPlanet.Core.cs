
using System;

namespace ToyPlanet.Core;

public class Toy
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    // Зовнішній ключ до категорії
    public Guid? CategoryId { get; set; }
    
    // Навігаційна властивість для зв'язку з категорією
    public virtual Category Category { get; set; }
    
    // Стара властивість для зворотної сумісності (опціонально)
    public string CategoryName { get; set; }
}
