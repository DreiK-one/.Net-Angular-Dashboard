using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public CategoryController(ICategoryService categoryService, 
            ITokenService tokenService,
            IUserService userService)
        {
            _categoryService = categoryService;
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpGet("get-categories")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categories = _categoryService.GetAllCategories();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet("get-category/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);

                if (category == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Category with this id doesn't exist!"
                    });
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);

                return Ok(new { Message = "Category has been deleted!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost("create-category")]
        public async Task<IActionResult> Create([FromForm] CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    return BadRequest("Invalid request");
                }

                var principle = _tokenService.GetPrincipleFromToken(categoryDto.UserAccessToken);
                var user = await _userService.GetUserByFirstAndLastName(principle.Identity.Name);

                categoryDto.UserId = user.Id;

                var category = await _categoryService
                    .CreateCategory(categoryDto);

                return Ok(category);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update(int id, IFormFile? file)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
