using Core.Domain.Entities;

namespace Application.Models.Dto.Responses;

public class ProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    
    public static ProductDto FromEntity(Product product)
    {
        return new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };

    }
}