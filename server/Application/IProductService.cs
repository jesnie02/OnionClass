using Application.Models.Dto.Request;
using Application.Models.Dto.Responses;
using Core.Domain.Entities;

namespace Application;

public interface IProductService
{
    Task<Product?> CreateProduct(CreateProductDto createProductDto);
    Task<ProductDto> GetProductById(int id);
    Task<List<Product>> GetAllProducts();
}