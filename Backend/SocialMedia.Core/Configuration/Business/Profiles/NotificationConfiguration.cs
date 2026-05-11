using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace SocialMedia.Core.Configuration.Business.Profiles;
public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasOne(x => x.Sender)
                     .WithMany()
                     .HasForeignKey(x => x.SenderId)
                     .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Recipient)
            .WithMany()
            .HasForeignKey(x => x.RecipientId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
