using MemberSystem.Business.Interfaces;
using MemberSystem.Domain.Entities;
using MemberSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberSystem.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _memberRepository;

        public UserService(IRepository<User> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }

        public async Task AddUserAsync(User member)
        {
            await _memberRepository.AddAsync(member);
            await _memberRepository.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User member)
        {
            _memberRepository.Update(member);
            await _memberRepository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member != null)
            {
                _memberRepository.Delete(member);
                await _memberRepository.SaveChangesAsync();
            }
        }
    }
}
