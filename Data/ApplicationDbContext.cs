using Micro_social_platform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Micro_social_platform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet <Article> Articles { get; set; }
        public DbSet <Comment> Comments { get; set; }
        public DbSet <Group> Groups { get; set; }
        public DbSet <GroupMessage> GroupMessages { get; set; }
        public DbSet <Profile> Profiles { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<Friend> Friends { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // definire primary key compus
            modelBuilder.Entity<UserGroup>()
            .HasKey(ab => new {
                ab.Id,
                ab.GroupId,
                ab.UserId 
            });
           
            modelBuilder.Entity<UserGroup>()
            .HasOne(ab => ab.Group)
            .WithMany(ab => ab.UserGroups)
            .HasForeignKey(ab => ab.GroupId);
            modelBuilder.Entity<UserGroup>()
            .HasOne(ab => ab.ApplicationUser)
            .WithMany(ab => ab.UserGroups)
            .HasForeignKey(ab => ab.UserId);
            modelBuilder.Entity<Friend>()
            .HasKey(ab => new {
                ab.Id,
                ab.User_sender_id,
                ab.User_receiver_id
            });
            modelBuilder.Entity<UserGroup>()
            .HasOne(ab => ab.Group)
            .WithMany(ab => ab.UserGroups)
            .HasForeignKey(ab => ab.GroupId);

            modelBuilder.Entity<UserGroup>()
            .HasOne(ab => ab.ApplicationUser)
            .WithMany(ab => ab.UserGroups)
            .HasForeignKey(ab => ab.UserId);

            modelBuilder.Entity<Friend>()
            .HasOne(ab => ab.ApplicationUser)
            .WithMany(ab => ab.Friends)
            .HasForeignKey(ab => ab.User_sender_id);

            modelBuilder.Entity<Friend>()
           .HasOne(ab => ab.ApplicationUser)
           .WithMany(ab => ab.Friends)
           .HasForeignKey(ab => ab.User_receiver_id);
        }


    }
}