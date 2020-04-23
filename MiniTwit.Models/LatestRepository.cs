using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Entities;

namespace MiniTwit.Models
{
    public class LatestRepository : ILatestRepository
    {
        
        private readonly IMiniTwitContext _context;

        public LatestRepository(IMiniTwitContext context)
        {
            _context = context;
        }

        
        public async Task<long> ReadLatestAsync()
        {
            var latest
                = from c in _context.Latest
                orderby c.Date descending 
                select c.Value;
            return await latest.FirstOrDefaultAsync();
        }

        public async Task UpdateLatestAsync(long value)
        {
            var latest = new Latest{Date = DateTime.Now, Value = value};
            await _context.Latest.AddAsync(latest);
            await _context.SaveChangesAsync();
        }
    }
}