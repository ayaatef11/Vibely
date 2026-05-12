using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Infrastructure.Domain.Entities.Business.Profiles;

namespace SocialMedia.Infrastructure.Persistence.Configuration.Business;
public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        builder.ToTable("Follower").HasKey(x => x.Id);

        builder.HasOne(x => x.Follower)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.FollowerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.Following)
          .WithMany(x => x.Following)
          .HasForeignKey(x => x.FollowingId)
          .OnDelete(DeleteBehavior.Restrict)
          .IsRequired(false);
        builder.Property(x => x.FollowState)
            .HasConversion<string>();
    }
}
