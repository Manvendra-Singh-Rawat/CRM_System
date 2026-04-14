using ClientManagement.Application.DataTemplate;
using ClientManagement.Domain.Entity;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Application.Features.Command
{
    public class RegisterUserCommandHandler(ClientManagementDbContext dbContext) : IRequestHandler<RegisterUserCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            bool isUserExists = await dbContext.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
            if (isUserExists)
                return Result<int>.Failure("Email already exists");

            var hasher = new PasswordHasher<User>();

            User newUser = new() { Email = request.Email };
            newUser.PasswordHash = hasher.HashPassword(newUser, request.Password);

            UserDetail userDetail = new() { User = newUser };

            await dbContext.Users.AddAsync(newUser, cancellationToken);
            await dbContext.UserDetails.AddAsync(userDetail, cancellationToken);

            await dbContext.SaveChangesAsync();

            return Result<int>.Success(newUser.Id, "User created successfully");
        }
    }
}
