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
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using ECommerce.BIL.Services.CacheService;

namespace ECommerce.BIL.Services.ProductService
{
    public class ProductSerivce : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        public ProductSerivce(IUnitOfWork unitOfWork, ICacheService cache)
        {
            _cache = cache;            
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
            await _cache.RefreshVersionAsync("products");
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
            await _cache.RefreshVersionAsync("products");
            await _cache.RemoveAsync($"product{Id}");
        }

        public async Task<PagedResult<ProductReadDto>> GetAllProductsAsync(ProductFilterDto filter)
        {
            string ProductVerion = await _cache.GetVersionAsync("products");
            string CacheKey =
$"products_{ProductVerion}_{filter.PageNumber}_{filter.PageSize}_{filter.Search}_{filter.MinPrice}_{filter.MaxPrice}_{filter.MaxQuantity}_{filter.MinQuantity}_{filter.InStock}_{filter.Sortby}_{filter.IsDescending}";

            var CachedProducts = await _cache.GetAsync<PagedResult<ProductReadDto>>(CacheKey);
            if (CachedProducts != null)
                return CachedProducts;
            
            var Products = await _unitOfWork.ProductsRepo.GetProductsAsync(filter);
             var Result = new PagedResult<ProductReadDto>
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
            await _cache.SetAsync(CacheKey, Result,5);
            return Result;
        }


        public async Task<ProductReadDto> GetProductByIdAsync(int Id)
        {
            string CacheKey = $"product{Id}";

            var CacheProduct = await _cache.GetAsync<ProductReadDto>(CacheKey);
            if (CacheProduct != null)
            {
                return CacheProduct;
            }
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
            await _cache.SetAsync(CacheKey, ProductModel, 10);
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
            await _cache.RemoveAsync($"product{productUpdateDto.Id}");
            await _cache.RefreshVersionAsync("products");
        }
                
        }
    }

