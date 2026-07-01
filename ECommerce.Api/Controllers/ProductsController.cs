using ECommerce.BIL.DTOS.ProductDtos;
using ECommerce.BIL.Services.ProductService;
using ECommerce.DAL.PaginationFilterDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Crypto.Operators;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;

        }
        [Authorize]
        [HttpGet]
        [EnableRateLimiting("Sliding")]
        public async Task<ActionResult> GetAllAsync([FromQuery] ProductFilterDto productFilterDto)
        {
            return Ok(await _productService.GetAllProductsAsync(productFilterDto));
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            var Product = await _productService.GetProductByIdAsync(id);
            if (Product == null)
            {
                return NotFound();
            }
            return Ok(Product);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteAsync(int Id)
        {

            var Product = await _productService.GetProductByIdAsync(Id);
            if (Product == null)
            {
                { return BadRequest(); }

            }
            await _productService.DeleteProductAsync(Id);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ProductAddDto productAddDto)
        {
           int Id = await _productService.CreateProductAsync(productAddDto);
            return Created(
        $"/api/products/{Id}",
        new { Message = "Created" });
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateAsync(ProductUpdateDto productUpdateDto, int Id)
        {
            if (productUpdateDto.Id != Id)
            {
                return BadRequest();
            }
            await _productService.UpdateProductAsync(productUpdateDto);
            return NoContent();
        }
    }
}
