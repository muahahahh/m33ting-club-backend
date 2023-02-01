using M33tingClub.Domain.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace M33tingClub.Infrastructure.Tags;

public class TagTypeConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tag", "app");

        builder.HasKey(m => m.Name);
        
        builder.Property(t => t.Name)
            .HasColumnName("name")
            .HasConversion(tName => tName.Value, tName => TagName.Create(tName));
        
        builder.Property("_isOfficial")
            .HasColumnName("is_official");
    }
}