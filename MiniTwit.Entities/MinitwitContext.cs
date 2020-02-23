using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Utils;

namespace MiniTwit.Entities
{
    public class MiniTwitContext : IdentityDbContext<User, IdentityRole<int>, int>, IMiniTwitContext
    {
        public MiniTwitContext(DbContextOptions<MiniTwitContext> options)
            : base(options)
        {
        }

        // Dont delete this, it is used by migrations.
        // ReSharper disable once UnusedMember.Global
        public MiniTwitContext() : base()
        {
        }

        public DbSet<Message> Messages { get; set; }
        
        public DbSet<Follows> Follows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            var connectionString = Misc.IsDevelopment()
                ? @"Host=localhost;Database=MiniTwit;Username=postgres;Password=test"
                : @"Host=database;Database=MiniTwit;Username=postgres;Password=test";

            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityColumns();
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Follows>()
                .HasKey(f => new { f.FollowerId, f.FolloweeId});
            
            modelBuilder.Entity<Follows>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Follows)
                .HasForeignKey(f => f.FollowerId);

            modelBuilder.Entity<Follows>()
                .HasOne(f => f.Followee)
                .WithMany(u => u.FollowedBy)
                .HasForeignKey(f => f.FolloweeId);

        }
    }
}