using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.ProductDtos;
using ECommerce.DAL.IUnitOfWork;
using ECommerce.DAL.Models;
using ECommerce.DAL.PaginationFilterDtos;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BIL.Services.ProductService
{
    public class ProductSerivce : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductSerivce(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
            
        }
        public async Task<int> CreateProductAsync(ProductAddDto productAddDto)
        {
            var Product = new Product()
            {
                ImageUrl = productAddDto.ImgUrl,
                Price = productAddDto.Price,
                Name = productAddDto.Name,
                Description = productAddDto.Description,
                QuntityInStock = productAddDto.QuantityInStock,

            };
             await _unitOfWork.ProductsRepo.CreateAsync(Product);
            await _unitOfWork.SaveChangesAsync();
            return Product.Id;
        }

        public async Task DeleteProductAsync(int Id)
        {
            var Product = await _unitOfWork.ProductsRepo.GetByIdAsync(Id);
            if (Product == null)
            {
                throw new KeyNotFoundException("ProductNotFound");
            }
            Product.IsDeleted = true;
            var Cartitems = Product.CartItems;
            foreach(var item in Cartitems)
            {
               await _unitOfWork.CarItemstRepo.DeleteAsync(item);
            }
            await _unitOfWork.ProductsRepo.DeleteAsync(Product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductReadDto>> GetAllProductsAsync(ProductFilterDto filter)
        {
            var Products = await _unitOfWork.ProductsRepo.GetProductsAsync(filter);
            return new PagedResult<ProductReadDto>
            {
                Items = Products.Items.Select(P => new ProductReadDto
                {
                    Id = P.Id,
                    Name = P.Name,
                    Description = P.Description,
                    InStock = P.InStock,
                    ImgUrl = P.ImageUrl,
                    RowVersion = P.RowVersion,
                    Price = P.Price,
                    QuantityInStock = P.QuntityInStock
                }).ToList()
                , PageNumber= Products.PageNumber,
                PageSize = Products.PageSize,
                TotalCount = Products.TotalCount,
            };
        }


        public async Task<ProductReadDto> GetProductByIdAsync(int Id)
        {
            var product = await _unitOfWork.ProductsRepo.GetByIdAsync(Id);
            if (product == null)
            {
                return null;
            }
            var ProductModel = new ProductReadDto()
            {
                Id = product.Id,
                Description = product.Description,
                ImgUrl = product.ImageUrl,
                InStock = product.InStock,
                Price = product.Price,
                Name = product.Name,
                QuantityInStock = product.QuntityInStock,
                RowVersion = product.RowVersion
            };

            return ProductModel;
        }

        public async Task UpdateProductAsync(ProductUpdateDto productUpdateDto)
        {
            var product = await _unitOfWork.ProductsRepo.GetByIdAsync(productUpdateDto.Id);
            if (product == null)
            {
                throw new KeyNotFoundException("not Found");
            }
            product.Description = productUpdateDto.Description;
            product.ImageUrl = productUpdateDto.ImagUrl;
            product.InStock = productUpdateDto.InStock;
            product.QuntityInStock = productUpdateDto.QuantityInStock;
            product.Price = productUpdateDto.Price;
            product.Name = productUpdateDto.Name;
            product.IsDeleted = productUpdateDto.IsDeleted;
            await _unitOfWork.ProductsRepo.SetRowVersion(product,productUpdateDto.RowVersion);
            await _unitOfWork.SaveChangesAsync();
        }
                
        }
    }

