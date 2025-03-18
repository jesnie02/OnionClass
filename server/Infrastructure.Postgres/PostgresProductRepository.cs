using Application.Interfaces;
using Application.Interfaces.Infrastructure.Postgres;
using Application.Models.Dto.Responses;
using Core.Domain.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Postgres;

public class PostgresProductRepository(MyDbContext myDbContext) : IProductRepository
{
    public async Task<Product> CreateProduct(Product product)
    {
        myDbContext.Products.Add(product);
        await myDbContext.SaveChangesAsync();
        return product;
    }

    public async Task<ProductDto> GetProductById(int id)
    {
        var list = await myDbContext.Products
            .FirstOrDefaultAsync(p => p.Id == id);
        if (list == null) throw new ArgumentNullException($"No product list found with ID {id}");
        
        return ProductDto.FromEntity(list);
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await myDbContext.Products.ToListAsync();
    }

    public User? GetUserOrNull(string email)
    {
        return myDbContext.Users.FirstOrDefault(u => u.Email == email);
    }

    public User AddUser(User user)
    {
        myDbContext.Users.Add(user);
        myDbContext.SaveChanges();
        return user;
    }

    public Devicelog AddMetric(Devicelog eventDtoData)
    {
        myDbContext.Devicelogs.Add(eventDtoData);
        myDbContext.SaveChanges();
        return eventDtoData;
    }

    public List<Devicelog> GetAllMetrics()
    {
        return myDbContext.Devicelogs.ToList();
    }

    public void ClearMetrics()
    {
        myDbContext.Devicelogs.RemoveRange(myDbContext.Devicelogs);
        myDbContext.SaveChanges();
    }
}