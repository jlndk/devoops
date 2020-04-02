using System.Collections.Generic;
using System.Threading.Tasks;
using MiniTwit.Entities;

namespace MiniTwit.Models
{
    public interface IUserRepository
    {
        Task<(Response response, int userId)> CreateAsync(User user);
        Task<IEnumerable<User>> ReadAsync();
        Task<User> ReadAsync(int userId);
        Task<Response> UpdateAsync(User user);
        Task<Response> DeleteAsync(int userId, bool force = false);
        Task<Response> AddFollowerAsync(int followerId, int followeeId);
        Task<User> ReadAsyncByUsername(string username);
        Task<IEnumerable<User>> GetFollowsAsync(int userId);
        Task<IEnumerable<User>> GetFollowedByAsync(int userId);
        Task<Response> RemoveFollowerAsync(int followerId, int followeeId);
        Task<bool> IsUserFollowing(int followerId, int followeeId);
    }
}