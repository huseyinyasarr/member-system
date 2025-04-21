using MemberSystem.Domain.Entities;
using MemberSystem.Domain.Interfaces;
using MemberSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MemberSystem.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly UserSystemDbContext _dbContext;

        public UserRepository(UserSystemDbContext dbContext) : base(dbContext) // Fix for CS1503
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}