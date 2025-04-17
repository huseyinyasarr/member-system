using System.Collections.Generic;
using System.Threading.Tasks;
using MemberSystem.Domain.Entities;

namespace MemberSystem.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // Bu property, ilgili DbSet'e ulaşmak için tanımlandı.
        object Users { get; }

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();

        // Sadece User entity için kullanılacak metot
        Task<User> GetUserByPhonePasswordAsync(string phoneNumber, string password);
    }
}
