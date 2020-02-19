using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MiniTwit.Entities
{
    public class MiniTwitContext : IdentityDbContext<User, IdentityRole<int>, int>, IMiniTwitContext
    {
        public DbSet<Message> Messages { get; set; }

        public MiniTwitContext(DbContextOptions<MiniTwitContext> options)
            : base(options)
        {
        }

        public MiniTwitContext() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                var connectionString = @"Host=127.0.0.1;Database=MiniTwit;Username=postgres;Password=test";

                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
