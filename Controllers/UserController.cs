using ClientManagement.Application.DTO;
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
        public async Task<ActionResult<int>> RegisterUser(RegisterUserCommand user)
        {
            int result = await _sender.Send(user);
            return (result < 0) ? StatusCode(500) : (result == 0) ? BadRequest() : Created("/user", result);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser(LoginUserCommand request)
        {
            UserLoginDTO loginStatus = await _sender.Send(request);

            if(!string.Equals(loginStatus.issue, "")) return BadRequest(loginStatus.issue);

            return Ok(loginStatus.token);
        }

        [Authorize]
        [HttpPatch("updatedetails")]
        public async Task<ActionResult> UpdateDetails(UpdateUserCommand request)
        {
            var result = await _sender.Send(request);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("checkauth")]
        public IActionResult AuthenticatedOnlyEndPoint()
        {
            return Ok("You are authenticated! YAY!!!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndPoint()
        {
            return Ok("You are authorized! DAMN BOI GOT SUM STATUS!!");
        }
    }
}
