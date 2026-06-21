using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.CartDtos;
using ECommerce.DAL.IUnitOfWork;
using ECommerce.DAL.Models;

namespace ECommerce.BIL.Services.CartService
{
    public class CartService : ICartService
    { 
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddItemToCart(string userId, AddToCartDto dto)
        {
            var customer = await _unitOfWork.CustomersRepo.GetByUserIdAsync(userId);

            if (customer == null)
                throw new KeyNotFoundException("CustomerNotFound");


            var cart = customer.cart;

            if (cart == null)
                throw new KeyNotFoundException("CartNotFound");


            var product = await _unitOfWork.ProductsRepo.GetByIdAsync(dto.ProductId);

            if (product == null)
                throw new KeyNotFoundException("ProductNotFound");


            cart.cartItems.Add(new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Quantity = dto.Quaintity
            });


            await _unitOfWork.SaveChangesAsync();
        }


        public async Task ClearCart(string userId)
        {
            var cusomer = await _unitOfWork.CustomersRepo.GetByUserIdAsync(userId);
            if(cusomer == null)
            {
                throw new KeyNotFoundException("CustomerNotFound");
            }
            var Cart = cusomer.cart;
            if (Cart == null)
            {

                throw new KeyNotFoundException("CartNotFound");
            }
            Cart.cartItems?.Clear();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ICollection<CartReadDto>> GetAllCartAsync()
        {
            var Carts = await _unitOfWork.CartsRepo.GetAllAsync();


            var CartsDto = Carts.Select(x => new CartReadDto
            {
                CustomerName = x.Customer.UserName ?? "",
                CustomerId = x.Customer.Id,

                Items = x.cartItems.Select(i => new CartItemReadDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name ?? "",
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList()
            }).ToList();

            return CartsDto;
        }

        public async Task<CartReadDto?> GetCartByCustomerIdAsync(int Id)
        {
            var Customer = await _unitOfWork.CustomersRepo.GetByIdAsync(Id);
            if (Customer?.cart == null)
            {
                return null;
            }

            var cart = await _unitOfWork.CartsRepo.GetByIdAsync(Customer.cart.Id);

            if (cart != null && Customer != null)
            {
               var CustomerName = cart.Customer?.UserName;
                var CartModel = new CartReadDto()
                {
                    CustomerName = CustomerName,
                    CustomerId =Id,
                    Items = cart.cartItems.Select(x => new CartItemReadDto
                    {
                        ProductId = x.ProductId,
                        ProductName = x.Product?.Name ?? "",
                        Quantity = x.Quantity,
                        Price = x.Product?.Price ?? 0
                    }).ToList()
                };
                return CartModel;
            }
            return null;
        }

        public async Task<CartReadDto?> GetCartByIdAsync(int Id) {
            var cart = await _unitOfWork.CartsRepo.GetByIdAsync(Id);
            if (cart == null)
            {
                throw new KeyNotFoundException("CartNotFound");
            }
            var customerName = cart.Customer?.UserName;

            var CartModel = new CartReadDto()
            {
                CustomerId = cart.CustomerID,
                CustomerName = customerName,
                Items = cart.cartItems.Select(x => new CartItemReadDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.Product?.Name ?? "",
                    Quantity = x.Quantity,
                    Price = x.Product?.Price ?? 0
                }).ToList()
            };
            return CartModel;
        }

        public async Task RemoveFromCart(string userId, CartItemDto dto)
        {
            var customer = await _unitOfWork.CustomersRepo.GetByUserIdAsync(userId);

            if (customer?.cart == null)
                throw new KeyNotFoundException("CartNotFound");


            var item = customer.cart.cartItems
                .FirstOrDefault(x => x.ProductId == dto.ProductId);


            if (item == null)
                throw new KeyNotFoundException("ItemNotFound");


            customer.cart.cartItems.Remove(item);


            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateQuantity(string userId, UpdateCartItemDto dto)
        {
            var customer = await _unitOfWork.CustomersRepo.GetByUserIdAsync(userId);

            if (customer == null || customer.cart == null)
            {
                throw new KeyNotFoundException("CartNotFound");
            }


            var cartItem = customer.cart.cartItems?
                .FirstOrDefault(x => x.ProductId == dto.ProductId);


            if (cartItem == null)
            {
                throw new KeyNotFoundException("CartItemNotFound");
            }


            if (dto.Quantity <= 0)
            {
                customer.cart.cartItems.Remove(cartItem);

                await _unitOfWork.SaveChangesAsync();
                return;
            }


            cartItem.Quantity = dto.Quantity;

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<CartReadDto?> GetMyCartAsync(string userId)
        {
            var customer = await _unitOfWork.CustomersRepo.GetByUserIdAsync(userId);

            if (customer == null)
            {
                throw new KeyNotFoundException("CustomerNotFound");
            }


            var cart = customer.cart;

            if (cart == null)
            {
                throw new KeyNotFoundException("CartNotFound");
            }


            return new CartReadDto
            {
                CustomerId = customer.Id,
                CustomerName = customer.UserName,
                Items = cart.cartItems.Select(x => new CartItemReadDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.Product?.Name ?? "",
                    Quantity = x.Quantity,
                    Price = x.Product?.Price ?? 0
                }).ToList()
            };
        }

    }

    }
