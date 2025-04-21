using MemberSystem.Business.Interfaces;
using MemberSystem.Domain.Entities;
using MemberSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberSystem.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateUserAsync(string phoneNumber, string password)
        {
            return await _userRepository.GetByPhoneNumberAsync(phoneNumber);
            // Şifre kontrolü burada YAPILMALI (örneğin, hash karşılaştırması)
            // Şu anda sadece kullanıcı varlığını kontrol ediyor.
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                _userRepository.Delete(user);
                await _userRepository.SaveChangesAsync();
            }
        }

        public async Task<User> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _userRepository.GetByPhoneNumberAsync(phoneNumber);
        }
    }
}