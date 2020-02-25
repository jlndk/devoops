using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MiniTwit.Entities
{
    public interface IMiniTwitContext
    {
        DbSet<Message> Messages { get; set; }
        DbSet<User> Users { get; set; }
        public DbSet<Follow> Follows { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}