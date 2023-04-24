using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public PositionController(IPositionService positionService, 
            ITokenService tokenService,
            IUserService userService)
        {
            _positionService = positionService;
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetByCategoryId(int categoryId)
        {
            try
            {
                var positions = await _positionService
                    .GetPositionsByCategoryId(categoryId);

                if (positions == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Positions of this category id doesn't exist!"
                    });
                }

                return Ok(positions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PositionDto positionDto)
        {
            try
            {
                if (positionDto == null)
                {
                    return BadRequest("Invalid request");
                }

                var principle = _tokenService.GetPrincipleFromToken(positionDto.UserAccessToken);
                var user = await _userService.GetUserByFirstAndLastName(principle.Identity.Name);

                positionDto.CreatedByUserId = user.Id;

                await _positionService.CreatePosition(positionDto);

                return Ok(new { Message = "Position has been created!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _positionService.DeletePosition(id);

                return Ok(new { Message = "Position has been deleted!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromBody] PositionDto positionDto)
        {
            try
            {
                if (positionDto == null)
                {
                    return BadRequest("Invalid request");
                }

                await _positionService.UpdatePosition(positionDto);

                return Ok(new { Message = "Position has been updated!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
