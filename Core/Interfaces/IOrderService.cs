using Core.DTOs;
using Core.Helpers;
using Data.Entities;


namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersWithIncludes();
        Task<List<Order>> GetOrdersWithIncludesOrderedByPlaced();
        Task<Order> GetOrderById(int id);
        Task<OrderDto<PaginatedResponse<Order>>> GetOrdersByPage(List<Order>? orders, int pageIndex, int pageSize);
    }
}
