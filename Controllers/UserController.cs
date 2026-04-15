using ClientManagement.Application.DataTemplate;
using ClientManagement.Application.Features.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        public ISender _sender;
        public UserController(ISender sender) => _sender = sender;

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<int>>> RegisterUser(RegisterUserCommand user)
        {
            Result<int> registerStatus = await _sender.Send(user);
            if (registerStatus.IsSuccess)
                return Created("/user", ApiResponse<int>.SuccessResponse(registerStatus.Data, registerStatus.Message));
            else
                return ApiResponse<int>.FailureResponse(registerStatus.Message);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> LoginUser(LoginUserCommand loginRequest)
        {
            Result<string> loginStatus = await _sender.Send(loginRequest);

            if (loginStatus.IsSuccess)
                return Created("/user", ApiResponse<string>.SuccessResponse(loginStatus.Data, loginStatus.Message));
            else
                return ApiResponse<string>.FailureResponse(loginStatus.Message);
        }

        [Authorize]
        [HttpPatch("updatedetails")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateDetails(UpdateUserCommand updateUserRequest)
        {
            Result<string> result = await _sender.Send(updateUserRequest);

            if (result.IsSuccess)
                return Ok(ApiResponse<string>.SuccessResponse(result.Data, result.Message));
            else
                return StatusCode(result.StatusCode, ApiResponse<string>.FailureResponse(result.Message));
        }

        [Authorize]
        [HttpGet("checkauth")]
        public IActionResult AuthenticatedOnlyEndPoint()
        {
            return Ok("You are authenticated!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndPoint()
        {
            return Ok("You are authorized!");
        }
    }
}
