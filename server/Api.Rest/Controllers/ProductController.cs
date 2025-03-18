using Application;
using Application.Models.Dto.Request;
using Application.Models.Dto.Responses;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Rest.Controllers;


    
    [ApiController]
    [Route("/[controller]")]
    [Authorize]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDto createProductDto)
        {
            var createdProduct = await productService.CreateProduct(createProductDto);
            if (createdProduct == null) 
                return BadRequest();
            
            return CreatedAtAction(nameof(CreateProduct), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpGet]
        [Route("")]
        public async Task<List<Product>> GetAllProduct()
        {
            return await productService.GetAllProducts();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            try
            {
                var product = await productService.GetProductById(id);
                return Ok(product);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
