using Microsoft.EntityFrameworkCore;
using ClientManagement.Domain.Entity;

namespace ClientManagement.Infrastructure.Persistence.PostgresDB
{
    public class ClientManagementDbContext : DbContext
    {
        public ClientManagementDbContext(DbContextOptions<ClientManagementDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Work> Works { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserDetail>()
                .HasOne(ud => ud.User)
                .WithOne(u => u.UserDetail)
                .HasForeignKey<UserDetail>(ud => ud.Id);

            modelBuilder.Entity<Work>()
                .HasOne(wo => wo.User)
                .WithOne(u => u.Work)
                .HasForeignKey<Work>(u => u.ClientId);
        }
    }
}
