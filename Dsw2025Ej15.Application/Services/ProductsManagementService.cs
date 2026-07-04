using Dsw2025Ej15.Application.Dtos;
using Dsw2025Ej15.Application.Exceptions;
using Dsw2025Ej15.Domain;
using Dsw2025Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dsw2025Ej15.Application.Services;

public class ProductsManagementService
{
    // Cambiamos IRepository por IPersistence
    private readonly IPersistence _repository;

    public ProductsManagementService(IPersistence repository)
    {
        _repository = repository;
    }

    public async Task<Product?> GetProductById(Guid id)
    {
        return await _repository.GetById<Product>(id);
    }

    public async Task<List<T>?> GetProducts<T>() where T : EntityBase
    {
        return await _repository.GetAll<T>();
    }

    public async Task<ProductModel.Response> AddProduct(ProductModel.Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Sku) ||
            string.IsNullOrWhiteSpace(request.Name) ||
            request.Price < 0)
        {
            throw new ArgumentException("Valores para el producto no válidos");
        }

        var exist = await _repository.First<Product>(p => p.Sku == request.Sku);
        if (exist != null) throw new DuplicatedEntityException($"Ya existe un producto con el Sku {request.Sku}");

        var product = new Product(request.Sku, request.Name, request.Price);
        await _repository.Add(product);
        return new ProductModel.Response(product.Id);
    }
}