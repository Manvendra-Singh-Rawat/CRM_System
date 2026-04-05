using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Infrastructure.Persistence.InMemoryDB
{
    public class InMemoryDbContext : DbContext
    {
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options)
        {

        }
    }
}
