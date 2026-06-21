using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.ProductDtos;
using ECommerce.DAL.IUnitOfWork;
using ECommerce.DAL.Models;
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
            Product.IsDeleted = true;
            await _unitOfWork.ProductsRepo.SoftDelete();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ICollection<ProductReadDto>> GetAllProductsAsync()
        {

            var products = await _unitOfWork.ProductsRepo.GetAllAsync();
            var ProductReadDto = products.Select(P=> new ProductReadDto()
            {
                Id = P.Id,
               Price = P.Price,
                Description = P.Description,
                 ImgUrl =  P.ImageUrl,
                  Name = P.Name,
                  InStock = P.InStock,
                  QuantityInStock= P.QuntityInStock,
                  RowVersion = P.RowVersion,

            }).ToList();
            return ProductReadDto;
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
                RowVersion = product.RowVersion
            };
            if (product.QuntityInStock < 10)
            {
                ProductModel.QuantityInStock  = product.QuntityInStock;
            }
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

