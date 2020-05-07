using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Entities;
using static MiniTwit.Models.Response;

namespace MiniTwit.Models
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMiniTwitContext _context;

        public MessageRepository(IMiniTwitContext context)
        {
            _context = context;
        }

        public async Task<(Response response, int messageId)> CreateAsync(Message message)
        {
            if (message.Author == null && message.AuthorId != 0)
            {
                message.Author = _context.Users.FirstOrDefault(u => u.Id == message.AuthorId);
            }
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return (Created, message.Id);
        }

        public async Task<IEnumerable<Message>> ReadManyAsync(int count, bool includeFlagged = false)
        {
            var query = 
                from m in _context.Messages
                where includeFlagged || m.Flagged <= 0
                orderby m.PubDate descending
                join user in _context.Users on m.AuthorId equals user.Id
                select new Message
                {
                    Author = user,
                    AuthorId = user.Id,
                    Flagged = m.Flagged,
                    Id = m.Id,
                    PubDate = m.PubDate,
                    Text = m.Text
                };
            return await query.Take(count).ToListAsync();
        }

        public async Task<IEnumerable<Message>> ReadManyWithinTimeAsync(int count, DateTime? dateOlderThan = null, DateTime? dateNewerThan = null)
        {
            return await _context.Messages
                .Where(m => m.Flagged <= 0)
                .Where(m => dateOlderThan == null || m.PubDate < dateOlderThan)
                .Where(m => dateNewerThan == null || m.PubDate > dateNewerThan)
                .OrderByDescending(m => m.PubDate)
                .Take(count)
                .Join(_context.Users, m => m.AuthorId, u => u.Id, (m, u) =>
                new Message {
                    Author = u,
                    AuthorId = u.Id,
                    Flagged = m.Flagged,
                    Id = m.Id,
                    PubDate = m.PubDate,
                    Text = m.Text
                })
                .ToListAsync();
        }

        public async Task<Message> ReadAsync(int messageId)
        {
            var messages = 
                from m in _context.Messages
                where m.Id == messageId && m.Flagged <= 0
                join user in _context.Users on m.AuthorId equals user.Id
                select new Message
                {
                    Author = user,
                    AuthorId = user.Id,
                    Flagged = m.Flagged,
                    Id = m.Id,
                    PubDate = m.PubDate,
                    Text = m.Text
                };
            return await messages.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Message>> ReadManyFromUserWithinTimeAsync(
            int userId,
            int count,
            DateTime? dateOlderThan = null,
            DateTime? dateNewerThan = null
        )
        {
            var messages = 
                from m in _context.Messages
                where m.AuthorId == userId && m.Flagged <= 0
                && (dateOlderThan == null || m.PubDate < dateOlderThan)
                && (dateNewerThan == null || m.PubDate > dateNewerThan)
                join user in _context.Users on m.AuthorId equals user.Id
                orderby m.PubDate descending 
                select new Message
                {
                    Author = user,
                    AuthorId = user.Id,
                    Flagged = m.Flagged,
                    Id = m.Id,
                    PubDate = m.PubDate,
                    Text = m.Text
                };
            return await messages.Take(count).ToListAsync();
        }

        public async Task<IEnumerable<Message>> ReadManyFromUserAsync(int userId, int count)
        {
            var messages = 
                from m in _context.Messages
                where m.AuthorId == userId && m.Flagged <= 0
                orderby m.PubDate descending
                join user in _context.Users on m.AuthorId equals user.Id
                select new Message
                {
                    Author = user,
                    AuthorId = user.Id,
                    Flagged = m.Flagged,
                    Id = m.Id,
                    PubDate = m.PubDate,
                    Text = m.Text
                };
            return await messages.Take(count).ToListAsync();
        }

        public async Task<IEnumerable<Message>> ReadAllMessagesFromUserAsync(int userId)
        {
            var messages = 
                from m in _context.Messages
                where m.AuthorId == userId && m.Flagged <= 0
                orderby m.PubDate descending
                join user in _context.Users on m.AuthorId equals user.Id
                select new Message
                {
                    Author = user,
                    AuthorId = user.Id,
                    Flagged = m.Flagged,
                    Id = m.Id,
                    PubDate = m.PubDate,
                    Text = m.Text
                };
            return await messages.ToListAsync();
        }

        public async Task<Response> UpdateAsync(Message message)
        {
            var entity = await _context.Messages.FindAsync(message.Id);
            if (entity == null)
            {
                return NotFound;
            }
            entity.Text = message.Text;
            entity.Flagged = message.Flagged;
            await _context.SaveChangesAsync();
            return Updated;
        }

        public async Task<Response> DeleteAsync(int messageId, bool force = false)
        {
            var entity = await _context.Messages.FirstOrDefaultAsync(u => u.Id == messageId);
            if (entity == null)
            {
                return NotFound;
            }

            _context.Messages.Remove(entity);
            await _context.SaveChangesAsync();
            return Deleted;
        }

        private async Task<IEnumerable<Follow>> GetFollowedAsync(int followerId)
        {
            var query =
                from f in _context.Follows
                where f.FollowerId == followerId
                select f;
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Message>> ReadMessagesFromFollowedWithinTimeAsync(
            int followerId,
            DateTime? dateOlderThan = null,
            DateTime? dateNewerThan = null)
        {
            var followedList = (await GetFollowedAsync(followerId)).Select(f => f.FolloweeId);
            var messages =
                from m in _context.Messages
                join user in _context.Users on m.AuthorId equals user.Id
                where followedList.Contains(user.Id) || user.Id == followerId
                where dateOlderThan == null || m.PubDate >= dateOlderThan
                where dateNewerThan == null || m.PubDate <= dateNewerThan
                orderby m.PubDate descending
                select new Message
                {
                    Author = user,
                    AuthorId = user.Id,
                    Flagged = m.Flagged,
                    Id = m.Id,
                    PubDate = m.PubDate,
                    Text = m.Text
                };
            return await messages.ToListAsync();
        }
    }
}
