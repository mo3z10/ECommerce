using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Resource;
using ECommerce.BIL.DTOS.OrderDtos;
using ECommerce.DAL.IUnitOfWork;
using ECommerce.DAL.Models;
using ECommerce.DAL.PaginationFilterDtos;

namespace ECommerce.BIL.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public async Task Checkout(string CustomerId)
        {
            var OrderAdd = new OrderAddDto() { 
            UserId = CustomerId,
            };
            var Customer = await _unitOfWork.CustomersRepo.GetByUserIdAsync(CustomerId);
            if (Customer == null) {
                throw new Exception("CustomerNotFound");
            }

            await _unitOfWork.BeginTransaction();
            try
            {

                await CreateOrderAsync(OrderAdd);
                await _unitOfWork.CartsRepo.ClearCartAsync(Customer.cart.Id);
                await _unitOfWork.Commit();
               
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;

            }

        }

        public async Task CreateOrderAsync(OrderAddDto OrderAddDto)
        {
            var Customer = await _unitOfWork.CustomersRepo.GetByUserIdAsync(OrderAddDto.UserId);
            if (Customer == null)
            {
                throw new KeyNotFoundException("CustomerNotFound");
            }
            var cart = Customer.cart;
            if (Customer.cart == null || !Customer.cart.cartItems.Any())
            {
                throw new Exception("Cart is empty");
            }

            var Order = new Order()
            {
                CustomerId = Customer.Id,
                OrderDate = DateTime.Now,
                OrderStatus = OrderStatus.Pending,
                TotalPrice = 0,
                TotalQuintiy = 0
            };
            foreach (var item in cart.cartItems)
            {
 
                if (item.Product.QuntityInStock < item.Quantity)
                {
                    throw new Exception(
                        $"Not enough stock for {item.Product.Name} Only {item.Product.QuntityInStock}"
                    );
                }
                var OrderItem = new OrderItem()
                {
                    ProductId = item.ProductId,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                };
                Order.OrderItems.Add(OrderItem);
                Order.TotalQuintiy += OrderItem.Quantity;
                Order.TotalPrice += (OrderItem.Quantity * OrderItem.Price);
                item.Product.QuntityInStock -= item.Quantity;
            }
            await _unitOfWork.OrdersRepo.CreateAsync(Order);
            await _unitOfWork.SaveChangesAsync();

                
            }

        public async Task DeleteOrderAsync(int OrderId)
        {
            var Order = await _unitOfWork.OrdersRepo.GetByIdAsync(OrderId);
            if (Order == null) {
                throw new KeyNotFoundException("OrderNotFound");
            }
            await _unitOfWork.OrdersRepo.DeleteAsync(Order);
        }
        public async Task<PagedResult<OrderReadDto>> GetAllOrdersAsync(OrderFilterDto orderFilterDto)
        {
            var Orders = await _unitOfWork.OrdersRepo.GetPagedAllAsync(orderFilterDto);
            return new PagedResult<OrderReadDto>
            {
                Items = Orders.Items.Select(o => new OrderReadDto()
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.UserName,
                    totalPrice = o.TotalPrice,
                    totalQuantity = o.TotalQuintiy,
                    OrderStatus = o.OrderStatus.ToString(),
                    Items = o.OrderItems.Select(i => new ReadOrderItemDto()
                    {
                        ItemId = i.ProductId,

                        ItemName = i.Product.Name,

                        ItemUnitPrice = i.Price,

                        ItemQuantity = i.Quantity,

                        ItemTotalPrice = i.Price * i.Quantity

                    }).ToList()


                }).ToList(),
                PageNumber = Orders.PageNumber,
                PageSize = Orders.PageSize,
                TotalCount = Orders.TotalCount

            };

        }

        public async Task<PagedResult<OrderReadDto>> GetCustomerOrderAsync(string CustomerId,OrderFilterDto orderFilterDto)
        {
            var Customer = await _unitOfWork.CustomersRepo.GetByUserIdAsync(CustomerId);
            if (Customer == null)
            
                {
                throw new KeyNotFoundException("CustomerNotFound");
                }
            var CustomerOrders = await _unitOfWork.OrdersRepo.GetCustomerOrders(orderFilterDto, Customer.Id);
            if (CustomerOrders == null)
            {
                throw new Exception("NoOrders");
            }

            var CustomerPagedOrders = CustomerOrders.Items.Select(o => new OrderReadDto()
            {
                CustomerId = o.CustomerId,
                CustomerName = o.Customer.UserName,
                Id = o.Id,
                totalPrice = o.TotalPrice,
                totalQuantity = o.TotalQuintiy,
                OrderStatus = o.OrderStatus.ToString(),
                Items = o.OrderItems.Select(i => new ReadOrderItemDto()
                {
                    ItemId = i.ProductId,

                    ItemName = i.Product.Name,

                    ItemUnitPrice = i.Price,

                    ItemQuantity = i.Quantity,

                    ItemTotalPrice = i.Price * i.Quantity

                }).ToList()
            }).ToList();

            return new PagedResult<OrderReadDto>
            {
                Items = CustomerPagedOrders,
                TotalCount = CustomerPagedOrders.Count(),
                PageNumber =CustomerOrders.PageNumber,
                PageSize = CustomerOrders.PageSize,
            };
        }

        public async Task<OrderReadDto> GetOrderByIdAsync(int OrderId)
        {
            var order = await _unitOfWork.OrdersRepo.GetByIdAsync(OrderId);
            if (order == null)
            {
                throw new KeyNotFoundException("OrderNotfound");
            }
            return new OrderReadDto()
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.UserName,
                totalPrice = order.TotalPrice,
                totalQuantity = order.TotalQuintiy,
                OrderStatus = order.OrderStatus.ToString(),
                Items = order.OrderItems.Select(i => new ReadOrderItemDto()
                {
                    ItemId = i.Id,
                    ItemName = i.Product.Name,
                    ItemQuantity = i.Quantity,
                    ItemTotalPrice = i.Price * i.Quantity,
                    ItemUnitPrice = i.Price,
                }).ToList()

            };
        }

        public async Task UpdateOrderStatusAsync(OrderUpdateStatusDto orderUpdateStatusDto)
        {
            var order = await _unitOfWork.OrdersRepo.GetByIdAsync(orderUpdateStatusDto.OrderId);
            if (order == null)
            {
                throw new KeyNotFoundException("OrderNotFound");

            }
            order.OrderStatus = orderUpdateStatusDto.Status;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
