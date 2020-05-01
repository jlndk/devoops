using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniTwit.Entities;

namespace MiniTwit.Models
{
    public interface IMessageRepository
    {
        Task<Message> ReadAsync(int messageId);
        Task<IEnumerable<Message>> ReadManyAsync(int count, bool includeFlagged = false);
        Task<IEnumerable<Message>> ReadManyWithinTimeAsync(int count, DateTime? dateOlderThan = null, DateTime? dateNewerThan = null);
        Task<IEnumerable<Message>> ReadAllMessagesFromUserAsync(int userId);
        Task<IEnumerable<Message>> ReadMessagesFromFollowedWithinTimeAsync(int followerId, DateTime? dateOlderThan = null, DateTime? dateNewerThan = null);
        Task<IEnumerable<Message>> ReadManyFromUserWithinTimeAsync(int userId, int count, DateTime? dateOlderThan = null, DateTime? dateNewerThan = null);
        Task<IEnumerable<Message>> ReadManyFromUserAsync(int userId, int count);
        Task<(Response response, int messageId)> CreateAsync(Message message);
        Task<Response> UpdateAsync(Message message);
        Task<Response> DeleteAsync(int messageId, bool force = false);
    }
}