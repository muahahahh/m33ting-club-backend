using M33tingClub.Domain.Users;
using M33tingClub.Domain.Users.Followers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace M33tingClub.Infrastructure.Users.Followers;

public class FollowershipTypeConfiguration : IEntityTypeConfiguration<Followership>
{
    public void Configure(EntityTypeBuilder<Followership> builder)
    {
        builder.ToTable("followership", "app");
        
        builder.HasKey(f => new { f.FollowerId, f.FollowingId });
        
        builder.Property(f => f.FollowerId)
            .HasColumnName("follower_id")
            .HasConversion(fId => fId.Value, fId => UserId.FromGuid(fId));
        
        builder.Property(f => f.FollowingId)
            .HasColumnName("following_id")
            .HasConversion(fId => fId.Value, fId => UserId.FromGuid(fId));
        
        builder.Property("_createdAt")
            .HasColumnName("created_at");
    }
}