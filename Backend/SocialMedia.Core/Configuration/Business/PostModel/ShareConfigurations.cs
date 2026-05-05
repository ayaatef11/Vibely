using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SocialMedia.Infrastructure.Persistence.Configuration.Business;
public class ShareConfigurations : IEntityTypeConfiguration<Share>
{
    public void Configure(EntityTypeBuilder<Share> builder)
    {
        builder.ToTable("Share").HasKey(x => x.Id);

        builder.HasOne(x => x.Post)
            .WithMany(x => x.Shares)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(true);

        builder.HasOne(x => x.Profile)
           .WithMany(x => x.Shares)
           .HasForeignKey(x => x.ProfileId)
           .OnDelete(DeleteBehavior.Restrict)
           .IsRequired(true);
    }
}