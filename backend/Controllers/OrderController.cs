using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Core.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(IOrderService orderService, 
            ICustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var orders = await _orderService
                    .GetOrdersWithIncludesOrderedByPlaced();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]OrderDto orderDto)
        {
            try
            {
                if (orderDto == null)
                {
                    return BadRequest();
                }

                var order = await _orderService.CreateOrder(orderDto);

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet("{pageIndex:int}/{pageSize:int}")]
        public async Task<IActionResult> Get(int pageIndex, int pageSize)
        {
            try
            {
                var orders = await _orderService.GetOrdersWithIncludesOrderedByPlaced();

                var response = await _orderService.GetOrdersByPage(orders, pageIndex, pageSize);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet("by-state")]
        public async Task<IActionResult> GetByState()
        {
            try
            {
                var orders = await _orderService.GetOrdersWithIncludes();

                var groupedResult = orders.GroupBy(o => o.Customer.State)
                    .ToList()
                    .Select(grp => new
                    {
                        State = grp.Key,
                        Total = grp.Sum(x => x.Total)
                    }).OrderByDescending(res => res.Total)
                .ToList();

                return Ok(groupedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }     
        }

        [HttpGet("by-customer/{number}")]
        public async Task<IActionResult> GetByCustomer(int number)
        {
            try
            {
                var orders = await _orderService.GetOrdersWithIncludes();

                var groupedResult = orders.GroupBy(o => o.Customer.Id)
                    .ToList()
                    .Select(grp => new
                    {
                        Name = _customerService.GetCustomerById(grp.Key).Result.Name,
                        Total = grp.Sum(x => x.Total)
                    }).OrderByDescending(res => res.Total)
                    .Take(number)
                    .ToList();

                return Ok(groupedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }         
        }

        [HttpGet("get-order/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderById(id);

                if (order == null)
                {
                    return BadRequest();
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
