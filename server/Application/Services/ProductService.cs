using Application.Interfaces;
using Application.Interfaces.Infrastructure.Postgres;
using Application.Models.Dto.Request;
using Application.Models.Dto.Responses;
using Core.Domain.Entities;

namespace Application.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<Product?> CreateProduct(CreateProductDto createProductDto)
    {
        var product = createProductDto.ToProduct();
        return await repository.CreateProduct(product);
    }

    public async Task<ProductDto> GetProductById(int id)
    {
        return await repository.GetProductById(id);
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await repository.GetAllProducts();
    }
}