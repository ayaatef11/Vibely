using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace SocialMedia.Core.Configuration.Business.Profiles;
public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasOne(x => x.Sender)
                     .WithMany(x=>x.Senders)
                     .HasForeignKey(x => x.SenderId)
                     .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Recipient)
            .WithMany(x=>x.Receivers)
            .HasForeignKey(x => x.RecipientId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
