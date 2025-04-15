using MemberSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberSystem.Business.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}
