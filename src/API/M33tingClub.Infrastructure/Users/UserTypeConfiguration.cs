using M33tingClub.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace M33tingClub.Infrastructure.Users;

public class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user", "app");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .HasConversion(uId => uId.Value, uId => UserId.FromGuid(uId));

        builder.Property(u => u.FirebaseId)
            .HasColumnName("firebase_id");

        builder.Property("_name")
            .HasColumnName("name");
        
        builder.Property("_birthday")
            .HasColumnName("birthday");

        builder.Property("_imageId")
            .HasColumnName("image_id");
        
        builder.Property("_phoneNumber")
            .HasColumnName("phone_number");
        
        builder.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted");
        
        builder.Property<UserGender>("_gender")
            .HasColumnName("gender")
            .HasConversion(gender => gender.Name,
                genderName => UserGender.FromName(genderName));
    }
}