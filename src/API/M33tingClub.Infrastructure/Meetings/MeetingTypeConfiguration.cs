using M33tingClub.Domain.Meetings;
using M33tingClub.Domain.Tags;
using M33tingClub.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;

namespace M33tingClub.Infrastructure.Meetings;

public class MeetingTypeConfiguration : IEntityTypeConfiguration<Meeting>
{
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
        builder.ToTable("meeting", "app");

        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Id)
            .HasColumnName("id")
            .HasConversion(mId => mId.Value, mId => MeetingId.FromGuid(mId));
        
        builder.Property("_name")
            .HasColumnName("name");
        
        builder.Property("_description")
            .HasColumnName("description");
        
        builder.Property("_participantsLimit")
            .HasColumnName("participants_limit");

        builder.Property("_isPublic")
            .HasColumnName("is_public");
        
        builder.Property("_confidentialInfo")
            .HasColumnName("confidential_info");

        builder.OwnsOne<TimeRange>(
            "_timeRange", timeRange =>
            {
                timeRange.Property(x => x.Start)
                         .HasColumnName("start_date");

                timeRange.Property(x => x.End)
                         .HasColumnName("end_date");
            });

        builder.Property("_imageId")
               .HasColumnName("image_id");

        builder.Property<MeetingStatus>("_status")
            .HasColumnName("status")
            .HasConversion(meetingStatus => meetingStatus.Name,
                meetingStatusName => MeetingStatus.FromName(meetingStatusName));

        builder.OwnsOne<Location>("_location", location =>
        {
            location.Property(x => x.Name)
                .HasColumnName("location_name");

            location.Property(x => x.Description)
                .HasColumnName("location_description");
            
            location.Property(x => x.Coordinates)
                .HasColumnName("location_coordinates")
                .HasConversion(x => new NpgsqlPoint(x.Longitude, x.Latitude), x => Coordinates.From(x.X, x.Y));
        });
        
        builder.OwnsMany<TagName>("_tagNames", tagName =>
        {
            tagName.ToTable("meeting_tag", "app");

            tagName.HasKey(x => x.Value);
            
            tagName.Property<MeetingId>("MeetingId")
                .HasConversion(mId => mId.Value, mId => MeetingId.FromGuid(mId))
                .HasColumnName("meeting_id");
            
            tagName.Property(x => x.Value).HasColumnName("tag_name");
        });

        builder.OwnsMany<Participant>("_participants", participant =>
        {
            participant.ToTable("participant", "app");
            
            participant.HasKey("MeetingId", "UserId");
            
            participant.Property<MeetingId>("MeetingId")
                .HasConversion(mId => mId.Value, mId => MeetingId.FromGuid(mId))
                .HasColumnName("meeting_id");

            participant.Property(x => x.UserId)
                .HasConversion(uId => uId.Value, uId => UserId.FromGuid(uId))
                .HasColumnName("user_id");
            
            participant.Property(x => x.MeetingRole)
                .HasConversion(meetingRole => meetingRole.Name, 
                    meetingRoleName => MeetingRole.FromName(meetingRoleName))
                .HasColumnName("meeting_role");
            
            participant.Property(x => x.JoinedDate).HasColumnName("joined_date");
        });

        builder.OwnsMany<MeetingApplication>(
            "_applications",
            application =>
            {
                application.ToTable("application", "app");

                application.HasKey("MeetingId", "UserId");

                application.Property<MeetingId>("MeetingId")
                           .HasConversion(mId => mId.Value, mId => MeetingId.FromGuid(mId))
                           .HasColumnName("meeting_id");

                application.Property(x => x.UserId)
                           .HasConversion(uId => uId.Value, uId => UserId.FromGuid(uId))
                           .HasColumnName("user_id");

                application.Property(x => x.Status)
                           .HasConversion(
                               status => status.Name,
                               statusName => MeetingApplicationStatus.FromName(statusName))
                           .HasColumnName("status");
            });
    }
}
