using Microsoft.EntityFrameworkCore;

namespace MiniTwit.Entities
{
    public class MiniTwitContext : DbContext, IMiniTwitContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        public MiniTwitContext(DbContextOptions<MiniTwitContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
