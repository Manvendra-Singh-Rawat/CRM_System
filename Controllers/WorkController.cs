using ClientManagement.Application.DataTemplate;
using ClientManagement.Application.DTO;
using ClientManagement.Application.Features.Command;
using ClientManagement.Application.Features.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkController(ISender _sender) : Controller
    {
        [Authorize]
        [HttpPost("creatework")]
        public async Task<ActionResult<ApiResponse<int>>> CreateWork(CreateWorkCommand work)
        {
            Result<int> result = await _sender.Send(work);
            if (result.IsSuccess)
                return Created("/work", ApiResponse<int>.SuccessResponse(result.Data, result.Message));
            else
                return ApiResponse<int>.FailureResponse(result.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("update-details")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateWorkDetails(UpdateWorkDetailsCommand work)
        {
            Result<string> result = await _sender.Send(work);
            if(result.IsSuccess)
                return Ok(ApiResponse<string>.SuccessResponse(result.Data, result.Message));
            else
                return ApiResponse<string>.FailureResponse(result.Message);
        }

        [Authorize]
        [HttpGet("get-your-work")]
        public async Task<ActionResult<ApiResponse<List<GetWorkDTO>>>> GetWorkDetails()
        {
            Result<List<GetWorkDTO>> result = await _sender.Send(new GetWorkQuery());

            if (result.IsSuccess)
                return Ok(ApiResponse<List<GetWorkDTO>>.SuccessResponse(result.Data, result.Message));
            else
                return ApiResponse<List<GetWorkDTO>>.FailureResponse(result.Message);
        }
    }
}
