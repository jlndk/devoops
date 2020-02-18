using MiniTwit.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniTwit.Models
{
    public interface IUserRepository
    {
        Task<(Response response, int userId)> CreateAsync(User user);
        Task<IEnumerable<User>> ReadAsync();
        Task<User> ReadAsync(int userId);
        Task<Response> UpdateAsync(User user);
        Task<Response> DeleteAsync(int userId, bool force = false);
    }
}
    