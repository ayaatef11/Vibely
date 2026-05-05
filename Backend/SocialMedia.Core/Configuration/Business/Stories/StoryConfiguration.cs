using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Infrastructure.Domain.Entities.Business.Stories;

namespace SocialMedia.Infrastructure.Persistence.Configuration.Business.Stories;
public class StoryConfiguration : IEntityTypeConfiguration<Story>
{
    public void Configure(EntityTypeBuilder<Story> builder)
    {
        builder.ToTable("Story").HasKey(x => x.Id);

        builder.HasOne(x => x.Profile)
            .WithMany(x => x.Stories)
            .HasForeignKey(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}

