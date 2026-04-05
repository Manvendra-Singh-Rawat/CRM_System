using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Domain.Entity
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; private set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash {  get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public UserDetail UserDetail { get; set; }
        public Work Work { get; set; }
    }
}
