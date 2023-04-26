using Core.DTOs;
using Core.Helpers;
using Core.Interfaces;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApiContext _context;

        public OrderService(ApiContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrders(int offset, int limit)
        {
            try
            {
                //Add date filter

                var orders = await _context.Orders
                    .OrderByDescending(o => o.Placed <= DateTime.Now)
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync();

                return orders;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Order> GetOrderById(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .FirstOrDefaultAsync(o => o.Id == id);

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Order>> GetOrdersWithIncludes()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Customer)
                    .ToListAsync();

                return orders;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Order>> GetOrdersWithIncludesOrderedByPlaced()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.Placed)
                    .ToListAsync();

                return orders;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GetOrderDto<PaginatedResponse<Order>>> GetOrdersByPage(List<Order>? orders, int pageIndex, int pageSize)
        {
            try
            {
                if (orders == null)
                {
                    throw new NullReferenceException();
                }

                var page = new PaginatedResponse<Order>(orders, pageIndex, pageSize);
                var totalPages = Math.Ceiling((double)orders.Count() / pageSize);

                var orderByPage = new GetOrderDto<PaginatedResponse<Order>>
                {
                    Page = page,
                    TotalPages = totalPages
                };

                return orderByPage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Order> CreateOrder(OrderDto orderDto)
        {
            try
            {
                var order = new Order
                {
                    Placed = DateTime.Now,
                    CustomerId = orderDto.CustomerId,
                    OrderItems = orderDto.OrderItemsDtos as IEnumerable<OrderItem>
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
