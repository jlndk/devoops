using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniTwit.Entities;

namespace MiniTwit.Models
{
    public interface IMessageRepository
    {
        Task<(Response response, int messageId)> CreateAsync(Message message);
        Task<IEnumerable<Message>> ReadAsync(bool includeFlagged = false);
        Task<IEnumerable<Message>> ReadCountAsync(int count = 100);
        Task<IEnumerable<Message>> ReadCountBeforeTimeAsync(int count, DateTime beforeDateTime);
        Task<List<Message>> ReadAllMessagesFromUserAsync(int userId);
        Task<Message> ReadAsync(int messageId);
        Task<Response> UpdateAsync(Message message);
        Task<Response> DeleteAsync(int messageId, bool force = false);
        Task<IEnumerable<Message>> ReadAllMessagesFromFollowedAsync(int followeeId);
        Task<IEnumerable<Message>> ReadCountFromUserBeforeTimeAsync(int count, int userId, DateTime beforeDateTime);
        Task<List<Message>> ReadCountFromUserAsync(int userId, int count = 100);
    }
}