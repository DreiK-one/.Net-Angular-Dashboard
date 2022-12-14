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

            var responcse = new
            {
                Page = page,
                TotalPages = totalPages
            };

            return Ok();
        }
    }
}
