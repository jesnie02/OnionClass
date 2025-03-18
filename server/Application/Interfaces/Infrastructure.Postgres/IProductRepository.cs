using Application.Models.Dto.Responses;
using Core.Domain.Entities;

namespace Application.Interfaces.Infrastructure.Postgres;

public interface IProductRepository
{
    Task<Product> CreateProduct(Product product);
    Task<ProductDto> GetProductById(int id);
    Task<List<Product>> GetAllProducts();
    User? GetUserOrNull(string email);
    User AddUser(User user);
    Devicelog AddMetric(Devicelog eventDtoData);
    List<Devicelog> GetAllMetrics();
    void ClearMetrics();
}