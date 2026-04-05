using ClientManagement.Application.Interfaces;

namespace ClientManagement.Application.Service
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public int clientId => int.Parse(httpContextAccessor.HttpContext?
            .User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
    }
}
