using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MemberSystem.Domain.Entities;

namespace MemberSystem.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
       
        object Users { get; }

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
        //Task<User> GetByPhoneNumberAsync(string phoneNumber);

       
    }
}
