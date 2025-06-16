namespace Dsw2025Ej15.Domain.Entities;

public class Product: EntityBase
{
    public Product()
    {
        
    }
    public Product(string sku, string name, decimal price)
    {
        Sku = sku;
        Name = name;
        CurrentUnitPrice = price;
        Id = Guid.NewGuid();
        IsActive = true;
    }
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public decimal CurrentUnitPrice { get; set; }
    public bool  IsActive { get; set; }
}
