using System;
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
                var isDevelopment = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development", StringComparison.InvariantCultureIgnoreCase);
                var connectionString = isDevelopment ? 
                    @"Host=localhost;Database=MiniTwit;Username=postgres;Password=test" 
                    : @"Host=database;Database=MiniTwit;Username=postgres;Password=test";

                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityColumns();
            base.OnModelCreating(modelBuilder);
        }
    }
}
