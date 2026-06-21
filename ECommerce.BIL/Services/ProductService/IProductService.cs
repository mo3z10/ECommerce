using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.ProductDtos;

namespace ECommerce.BIL.Services.ProductService
{
    public interface IProductService
    {
        Task<ICollection<ProductReadDto>> GetAllProductsAsync();
        Task<ProductReadDto> GetProductByIdAsync(int Id);
        Task <int> CreateProductAsync (ProductAddDto productAddDto);
        Task DeleteProductAsync(int Id);
        Task UpdateProductAsync(ProductUpdateDto productUpdateDto);
 
    }
}
