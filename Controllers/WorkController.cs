using ClientManagement.Application.Features.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkController(ISender _sender) : Controller
    {
        [HttpPost("creatework")]
        public Task CreateWork(CreateWorkCommand work)
        {
            _sender.Send(work);
            return Task.CompletedTask;
        }

        /*[HttpDelete]
        public Task DeleteWork(Work work)
        {
            return Task.CompletedTask;
        }

        [HttpPost]
        public Task UpdateWork(Work work)
        {
            return Task.CompletedTask;
        }*/
    }
}
