using M33tingClub.Domain.MeetingNotifications;
using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Tags;
using M33tingClub.Domain.Users;
using M33tingClub.Domain.Users.Followers;
using Microsoft.EntityFrameworkCore;

namespace M33tingClub.Infrastructure.DataAccess;

public class M33tingClubDbContext : DbContext
{
    public DbSet<Meeting> Meetings { get; set; }
    
    public DbSet<Tag> Tags { get; set; }
    
    public DbSet<User> Users { get; set; }

    public DbSet<Followership> Followership { get; set; }
    
    public DbSet<MeetingNotification> MeetingNotifications { get; set; }

    public M33tingClubDbContext(DbContextOptions<M33tingClubDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = GetType().Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}