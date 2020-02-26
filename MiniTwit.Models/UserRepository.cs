using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniTwit.Entities;
using static MiniTwit.Models.Response;

namespace MiniTwit.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly IMiniTwitContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IMiniTwitContext context, ILogger<UserRepository> logger)
        {
            _logger = logger;
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
            var query =
                from u in _context.Users
                orderby u.UserName
                select u;
            return await query.ToListAsync();
        }

        public async Task<User> ReadAsync(int userId)
        {
            var users =
                from u in _context.Users
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

            entity.UserName = user.UserName;
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

        private async Task<bool> UserExists(int userId, string emailAddress)
        {
            return await _context.Users.AnyAsync(u => u.Id != userId && u.Email == emailAddress);
        }

        public async Task AddFollowerAsync(int followerId, int followeeId)
        {
            if (followeeId == followerId)
            {
                _logger.LogInformation($"{followeeId} tried to follow themself");
                return;
            }

            if (_context.Follows.Any(f => f.FolloweeId == followeeId && f.FollowerId == followerId))
            {
                _logger.LogInformation($"{followerId} tried to follow {followeeId} but was already following");
                return;
            }
            
            _context.Follows.Add(new Follow
            {
                FollowerId = followerId,
                FolloweeId = followeeId
            });

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFollowerAsync(int followerId, int followeeId)
        {
            if (followeeId == followerId)
            {
                _logger.LogInformation($"{followeeId} tried to unfollow themself");
                return;
            }

            if (!_context.Follows.Any(f => f.FolloweeId == followeeId && f.FollowerId == followerId))
            {
                _logger.LogInformation($"{followerId} tried to unfollow {followeeId} but was not following");
                return;
            }

            var follow = await _context.Follows.FirstAsync(f => f.FolloweeId == followeeId && f.FollowerId == followerId);
            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();
        } 
        
        public async Task<User> ReadAsyncByUsername(string username)
        {
            var users =
                from u in _context.Users
                where u.UserName == username
                select u;
            return await users.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetFollows(int userId)
        {
            var users =
                from f in _context.Follows
                where f.FollowerId == userId
                join u in _context.Users on f.FolloweeId equals u.Id
                select u;
            return await users.ToListAsync();
        }
        
        public async Task<IEnumerable<User>> GetFollowedBy(int userId)
        {
            var users =
                from f in _context.Follows
                where f.FolloweeId == userId
                join u in _context.Users on f.FolloweeId equals u.Id
                select u;
            return await users.ToListAsync();
        }

       
    }
}