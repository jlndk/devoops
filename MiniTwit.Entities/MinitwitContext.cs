using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MiniTwit.Entities
{
    public class MiniTwitContext : IdentityDbContext<User, IdentityRole<int>, int>, IMiniTwitContext
    {
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
