using MemberSystem.Domain.Interfaces;
using MemberSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemberSystem.Domain.Entities;

namespace MemberSystem.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly UserSystemDbContext _context;

        public Repository(UserSystemDbContext context)
        {
            _context = context;
        }

        // Generic DbSet olarak Users property’si
        public object Users => _context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Yalnızca User tipi için implemente edilecek metod
        public async Task<User> GetUserByPhonePasswordAsync(string phoneNumber, string password)
        {
            // Bu metod yalnızca T tipi User olduğunda anlamlıdır.
            if (typeof(T) != typeof(User))
                throw new InvalidOperationException("GetUserByPhonePasswordAsync metodu yalnızca User entity tipi için kullanılabilir.");

            // _context.Set<User>() ile sorgulama yapıyoruz.
            return await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.Password == password);
        }
    }
}
