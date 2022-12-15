using backend.Data;
using backend.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Controllers.Helpers;


namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApiContext _context;

        public OrderController(ApiContext context)
        {
            _context = context;
        }

        //GET api/orders/pageNumber/pageSize
        [HttpGet("{pageIndex:int}/{pageSize:int}")]
        public IActionResult Get(int pageIndex, int pageSize)
        {
            var data = _context.Orders.Include(o => o.Customer)
                .OrderByDescending(c => c.Placed);

            var page = new PaginatedResponse<Order>(data, pageIndex, pageSize);

            var totalCount = data.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);

            var response = new
            {
                Page = page,
                TotalPages = totalPages
            };

            return Ok(response);
        }

        [HttpGet("ByState")]
        public IActionResult ByState()
        {
            var orders = _context.Orders.Include(_o => _o.Customer).ToList();

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

        [HttpGet("ByCustomer/{number}")]
        public IActionResult ByCustomer(int number)
        {
            var orders = _context.Orders.Include(_o => _o.Customer).ToList();

            var groupedResult = orders.GroupBy(o => o.Customer.Id)
                .ToList()
                .Select(grp => new
                {
                    Name = _context.Customers.Find(grp.Key).Name,
                    Total = grp.Sum(x => x.Total)
                }).OrderByDescending(res => res.Total)
                .Take(number)
                .ToList();

            return Ok(groupedResult);
        }

        [HttpGet("GetOrder", Name = "GetOrder")]
        public IActionResult GetOrder(int id)
        {
            var order = _context.Orders.Include(_o => _o.Customer)
                .First(o => o.Id == id);

            return Ok(order);
        }
    }
}
