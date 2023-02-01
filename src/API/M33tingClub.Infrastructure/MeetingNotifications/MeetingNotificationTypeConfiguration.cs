using M33tingClub.Domain.MeetingNotifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace M33tingClub.Infrastructure.MeetingNotifications;

public class MeetingNotificationTypeConfiguration : IEntityTypeConfiguration<MeetingNotification>
{
    public void Configure(EntityTypeBuilder<MeetingNotification> builder)
    {
        builder.ToTable("meeting_notification", "app");
        
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("id");
        
        builder.Property<MeetingNotificationType>("_type")
            .HasColumnName("type")
            .HasConversion(type => type.Name,
                typeName => MeetingNotificationType.FromName(typeName));
        
        builder.Property("_meetingId")
            .HasColumnName("meeting_id");
        
        builder.Property("_performerId")
            .HasColumnName("performer_id");
        
        builder.Property("_receiverId")
            .HasColumnName("receiver_id");

        builder.Property("_wasSeen")
            .HasColumnName("was_seen");

        builder.Property("_occuredOn")
            .HasColumnName("occured_on");
    }
}