using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
  // DbContext represents a session with Database and can be used to query and save instances of entitites.
  public class DataContext : IdentityDbContext<AppUser>
  {
    // We have to use some options from DbContextOptions BUT also from DbContext, hence 'base' addition.
    // W/o the base, that will cause problems to migration.
    public DataContext(DbContextOptions options) : base(options)
    {

    }

    // This will be a table name.
    public DbSet<Value> Values { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<UserActivity> UserActivities { get; set; }
    public DbSet<Photo> Photos {get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserFollowing> Followings { get; set; }
    public DbSet<Spot> Spots { get; set; }
    public DbSet<SpotPhoto> SpotPhotos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<Value>()
          .HasData(
              new Value { Id = 1, Name = "Value 101" },
              new Value { Id = 2, Name = "Value 102" },
              new Value { Id = 3, Name = "Value 103" }
          );

      // Creating primary key that consist of two IDs
      builder.Entity<UserActivity>(x => x.HasKey(ua => 
        new {ua.AppUserId, ua.ActivityId}));

      // Creating many to many relationship
      builder.Entity<UserActivity>()
        .HasOne(u => u.AppUser)
        .WithMany(a => a.UserActivities)
        .HasForeignKey(u => u.AppUserId);

      builder.Entity<UserActivity>()
        .HasOne(a => a.Activity)
        .WithMany(u => u.UserActivities)
        .HasForeignKey(a => a.ActivityId);

      builder.Entity<UserFollowing>(b =>
      {
        b.HasKey(k => new { k.ObserverId, k.TargetId });

        b.HasOne(o => o.Observer)
          .WithMany(f => f.Followings)
          .HasForeignKey(o => o.ObserverId)
          .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(o => o.Target)
          .WithMany(f => f.Followers)
          .HasForeignKey(o => o.TargetId)
          .OnDelete(DeleteBehavior.Restrict);
      });
    }
  }
}
