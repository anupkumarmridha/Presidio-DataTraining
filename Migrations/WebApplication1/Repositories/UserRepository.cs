using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class UserRepository : IRepository<int, User>
    {
        private TodoContext _context;

        public UserRepository(TodoContext context)
        {
            _context = context;
        }
        public async Task<User> Add(User item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<User> Delete(int key)
        {
            var user = await Get(key);
            if (user != null)
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return user;
            }
            throw new Exception("No user with the given ID");
        }

        public async Task<User> Get(int key)
        {
            return (await _context.Users.SingleOrDefaultAsync(u => u.UserId == key)) ?? throw new Exception("No user with the given ID");
        }

        public async Task<IEnumerable<User>> Get()
        {
            return (await _context.Users.ToListAsync());
        }

        public IQueryable<User> Query()
        {
            throw new NotImplementedException();
        }

        public async Task<User> Update(User item)
        {
            var user = await Get(item.UserId);
            if (user != null)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return user;
            }
            throw new Exception("No user with the given ID");
        }
    }
}
