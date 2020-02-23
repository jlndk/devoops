﻿using System.Collections.Generic;
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

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return (Created, message.Id);
        }

        public async Task<IEnumerable<Message>> ReadAsync()
        {
            var query = from m in _context.Messages
                        where m.Flagged <= 0
                        orderby m.PubDate
                        join user in _context.Users on m.AuthorId equals user.Id
                        select new Message()
                        {
                            Author = user,
                            AuthorId = user.Id,
                            Flagged = m.Flagged,
                            Id = m.Id,
                            PubDate = m.PubDate,
                            Text = m.Text
                        };


                        return await query.ToListAsync();
        }

        public async Task<IEnumerable<Message>> ReadCountAsync(int count)
        {
            var query = from m in _context.Messages
                where m.Flagged <= 0
                orderby m.PubDate
                join user in _context.Users on m.AuthorId equals user.Id
                select new Message()
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

        public async Task<Message> ReadAsync(int messageId)
        {
            var messages = from m in _context.Messages
                        where m.Id == messageId
                        join user in _context.Users on m.AuthorId equals user.Id
                        select new Message()
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

        public async Task<List<Message>> ReadAllMessagesFromUserAsync(int userId)
        {
            var messages = from m in _context.Messages
                           where m.AuthorId == userId && m.Flagged <= 0
                           orderby m.PubDate
                           join user in _context.Users on m.AuthorId equals user.Id
                           select new Message()
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

    }
}
