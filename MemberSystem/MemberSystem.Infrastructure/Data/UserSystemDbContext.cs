using MemberSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberSystem.Infrastructure.Data
{
    public class UserSystemDbContext : DbContext
    {
        public UserSystemDbContext(DbContextOptions<UserSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
