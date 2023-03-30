using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _customerService.GetCustomers();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }  
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerById(id);

                if (customer == null)
                {
                    return BadRequest();
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }          
        }
    }
}
