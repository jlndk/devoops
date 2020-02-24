using System.Collections.Generic;
using System.Threading.Tasks;
using MiniTwit.Entities;

namespace MiniTwit.Models
{
    public interface IMessageRepository
    {
        Task<(Response response, int messageId)> CreateAsync(Message message);
        Task<IEnumerable<Message>> ReadAsync();
        Task<IEnumerable<Message>> ReadCountAsync(int count);
        Task<List<Message>> ReadAllMessagesFromUserAsync(int userId);
        Task<Message> ReadAsync(int messageId);
        Task<Response> UpdateAsync(Message message);
        Task<Response> DeleteAsync(int messageId, bool force = false);
        Task<IEnumerable<Message>> ReadAllMessagesFromFollowedAsync(int parse);
    }
}