using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.ProductDtos;
using ECommerce.DAL.PaginationFilterDtos;

namespace ECommerce.BIL.Services.ProductService
{
    public interface IProductService
    {
        Task<PagedResult<ProductReadDto>> GetAllProductsAsync(ProductFilterDto filter);
        Task<ProductReadDto> GetProductByIdAsync(int Id);
        Task <int> CreateProductAsync (ProductAddDto productAddDto);
        Task DeleteProductAsync(int Id);
        Task UpdateProductAsync(ProductUpdateDto productUpdateDto);
 
    }
}
