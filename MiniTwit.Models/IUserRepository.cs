using System.Collections.Generic;
using System.Threading.Tasks;
using MiniTwit.Entities;

namespace MiniTwit.Models
{
    public interface IUserRepository
    {
        Task<User> ReadAsync(int userId);
        Task<User> ReadAsyncByUsername(string username);
        Task<IEnumerable<User>> ReadManyAsync();
        Task<(Response response, int userId)> CreateAsync(User user);
        Task<Response> UpdateAsync(User user);
        Task<Response> DeleteAsync(int userId, bool force = false);
        Task<Response> AddFollowerAsync(int followerId, int followeeId);
        Task<IEnumerable<User>> GetFollowsAsync(int userId);
        Task<IEnumerable<User>> GetFollowersAsync(int userId);
        Task<Response> RemoveFollowerAsync(int followerId, int followeeId);
        Task<bool> IsUserFollowing(int followerId, int followeeId);
    }
}