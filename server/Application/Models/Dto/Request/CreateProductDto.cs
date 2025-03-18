using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;

namespace Application.Models.Dto.Request;

public class CreateProductDto
{
    [Required] 
    private string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public Product ToProduct()
    {
        return new Product()
        {
            Name = Name,
            Price = Price
        };
    }
}