using System.Collections.Generic;

namespace ToyPlanet.Core;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string? Description { get; set; }
    
    // Навігаційна властивість для зв'язку з товарами
    public virtual List<Toy> Toys { get; set; } = new List<Toy>();

    public Category()
    {
    }

    public Category(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }
}