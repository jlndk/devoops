using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Entities;
using static MiniTwit.Models.Response;

namespace MiniTwit.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly IMinitwitContext _context;

        public UserRepository(IMinitwitContext context)
        {
            _context = context;
        }

        public async Task<(Response response, int userId)> CreateAsync(User user)
        {
            if (await UserExists(0, user.Email))
            {
                return (Conflict, 0);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (Created, user.Id);
        }

        public async Task<IEnumerable<User>> ReadAsync()
        {
            var query = from u in _context.Users
                        orderby u.Username
                        select u;

            return await query.ToListAsync();
        }

        public async Task<User> ReadAsync(int userId)
        {
            var users = from u in _context.Users
                        where u.Id == userId
                        select u;

            return await users.FirstOrDefaultAsync();
        }

        public async Task<Response> UpdateAsync(User user)
        {
            var entity = await _context.Users.FindAsync(user.Id);

            if (entity == null)
            {
                return NotFound;
            }

            if (await UserExists(user.Id, user.Email))
            {
                return Conflict;
            }

            entity.Username = user.Username;
            entity.Email = user.Email;

            await _context.SaveChangesAsync();

            return Updated;
        }

        public async Task<Response> DeleteAsync(int userId, bool force = false)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (entity == null)
            {
                return NotFound;
            }

            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();

            return Deleted;
        }

        private async Task<bool> UserExists(int userId, string emailAddress) => await _context.Users.AnyAsync(u => u.Id != userId && u.Email == emailAddress);
    }
}
