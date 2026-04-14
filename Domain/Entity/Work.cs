using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Domain.Entity
{
    [Index(nameof(ClientId))]
    public class Work
    {
        public int Id { get; set; }
        public required string ProjectName { get; set; }
        public string ProjectDesc { get; set; } = string.Empty;

        public required int ClientId { get; set; }
        public double Cost { get; set; } = 0;
        public bool IsCompleted { get; set; } = false;
        public bool IsPaid { get; set; } = false;

        public User User { get; set; }
    }
}
