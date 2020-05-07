using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Utils;

namespace MiniTwit.Entities
{
    public class MiniTwitContext : IdentityDbContext<User, IdentityRole<int>, int>, IMiniTwitContext
    {
        public MiniTwitContext(DbContextOptions<MiniTwitContext> options) : base(options) { }

        // Dont delete this, it is used by migrations.
        // ReSharper disable once UnusedMember.Global
        public MiniTwitContext() : base()
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Latest> Latest { get; set; }
        
        public DbSet<Follow> Follows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTIONSTRING");

            connectionString ??= Misc.RunsInDocker()
                ? @"Host=database;Database=MiniTwit;Username=postgres;Password=test"
                : @"Host=localhost;Database=MiniTwit;Username=postgres;Password=test";

            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.UseIdentityColumns();
            base.OnModelCreating(builder);

            builder.Entity<Message>().HasIndex(m => m.PubDate);
            builder.Entity<Message>().HasIndex(m => new {m.AuthorId, m.PubDate}); 
            builder.Entity<User>().HasIndex(u => u.UserName);
            builder.Entity<Latest>().HasIndex(m => m.Date);
            builder.Entity<Follow>().HasIndex(f => f.FolloweeId);
            builder.Entity<Follow>()
                .HasKey(f => new { f.FollowerId, f.FolloweeId});
            
            builder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Follows)
                .HasForeignKey(f => f.FollowerId);

            builder.Entity<Follow>()
                .HasOne(f => f.Followee)
                .WithMany(u => u.FollowedBy)
                .HasForeignKey(f => f.FolloweeId);
        }
    }
}
