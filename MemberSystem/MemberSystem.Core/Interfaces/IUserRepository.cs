using MemberSystem.Domain.Entities;
using System.Threading.Tasks;

namespace MemberSystem.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User> // Eğer generic bir repository arayüzünüz varsa
    {
        Task<User> GetByPhoneNumberAsync(string phoneNumber);
        // Diğer kullanıcıya özel repository metotları
    }
}