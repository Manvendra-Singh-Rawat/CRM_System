using ClientManagement.Application.DataTemplate;
using ClientManagement.Domain.Entity;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClientManagement.Application.Features.Command
{
    public class LoginUserCommandHandler(ClientManagementDbContext dbContext, IConfiguration config) : IRequestHandler<LoginUserCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var client = await dbContext.Users.AsNoTracking().Where(y => y.IsActive == true).FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
            if(client == null)
                return Result<string>.Failure("Account does not exist or deactivated");

            if (new PasswordHasher<User>().VerifyHashedPassword(client, client.PasswordHash, request.Password) == PasswordVerificationResult.Success)
                return Result<string>.Success(CreateToken(client), "Login successful");
            else
                return Result<string>.Failure("Incorrect email or password");
        }

        private string CreateToken(User client)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
                new Claim(ClaimTypes.Email, client.Email),
                new Claim(ClaimTypes.Role, client.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("JwtSettings:Key")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: config.GetValue<string>("JwtSettings:Issuer"),
                audience: config.GetValue<string>("JwtSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.Date.AddDays(7.0),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
