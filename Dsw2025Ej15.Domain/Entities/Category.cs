namespace Dsw2025Ej15.Domain.Entities;

public class Category: EntityBase
{
    public string? Name { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Product> Products { get; } = new HashSet<Product>();
}
