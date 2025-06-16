namespace Dsw2025Ej15.Application.Dtos;

public record ProductModel
{
    public record Request(string Sku, string Name, decimal Price);

    public record Response(Guid Id);
}
