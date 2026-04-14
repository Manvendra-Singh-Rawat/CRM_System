using ClientManagement.Application.DTO;
using ClientManagement.Application.Features.Command;
using ClientManagement.Application.Features.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkController(ISender _sender) : Controller
    {
        [Authorize]
        [HttpPost("creatework")]
        public async Task<ActionResult<string>> CreateWork(CreateWorkCommand work)
        {
            string result = await _sender.Send(work);
            return Created();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("update-details")]
        public async Task<ActionResult> UpdateWorkDetails(UpdateWorkDetailsCommand work)
        {
            var result = await _sender.Send(work);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("get-your-work")]
        public async Task<ActionResult<List<GetWorkDTO>>> GetWorkDetails()
        {
            var result = await _sender.Send(new GetWorkQuery());
            if (result == null)
                return NotFound("No work by you");

            return Ok(result);
        }
    }
}
