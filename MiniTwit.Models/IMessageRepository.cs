using MiniTwit.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniTwit.Models
{
    public interface IMessageRepository
    {
        Task<(Response response, int messageId)> CreateAsync(Message message);
        Task<IEnumerable<Message>> ReadAsync();
        Task<List<Message>> ReadAllMessagesFromUserAsync(int userId);
        Task<Message> ReadAsync(int messageId);
        Task<Response> UpdateAsync(Message message);
        Task<Response> DeleteAsync(int messageId, bool force = false);
    }
}
    