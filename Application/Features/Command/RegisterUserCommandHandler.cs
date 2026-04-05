using ClientManagement.Domain.Entity;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Application.Features.Command
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private ClientManagementDbContext dbContext;

        public RegisterUserCommandHandler(ClientManagementDbContext context) => dbContext = context;

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var exists = dbContext.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
            if(exists.Result) return -1;

            User client = new User { Email = request.Email };
            var hasher = new PasswordHasher<User>();
            client.PasswordHash = hasher.HashPassword(client, request.Password);
            dbContext.Users.Add(client);
            await dbContext.SaveChangesAsync();

            var newTask = Task.Run(async () => await CreateUserDetails(client.Id));
            newTask.Wait();
            return client.Id;
        }

        private async Task CreateUserDetails(int id)
        {
            UserDetail userDetail = new UserDetail { Id = id };
            dbContext.UserDetails.Add(userDetail);
            await dbContext.SaveChangesAsync();
        }
    }
}
