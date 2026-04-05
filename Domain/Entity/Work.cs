using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Domain.Entity
{
    [Index(nameof(ClientId))]
    public class Work
    {
        public int Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;

        // Later add project description data
        //public string ProjectDesc { get; set; } = string.Empty;

        public int ClientId { get; set; }
        public int Cost { get; set; }
        public bool IsCompleted { get; set; } = false;
        public bool IsPaid { get; set; } = false;

        public User User { get; set; }
    }
}
