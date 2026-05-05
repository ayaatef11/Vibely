using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SocialMedia.Infrastructure.Persistence.Configuration.Business;
public class PostLikesConfigurations : IEntityTypeConfiguration<PostLikes>
{
    public void Configure(EntityTypeBuilder<PostLikes> builder)
    {
        builder.ToTable("PostLikes").HasKey(x => x.Id);

        // relationship  one to many between post and react
        builder.HasOne(x => x.Post).
            WithMany(x => x.Reacts).
            HasForeignKey(x => x.PostId).
            OnDelete(DeleteBehavior.Restrict).
            IsRequired(true);

        // relationship  one to many between User and react
        builder.HasOne(x => x.Profile).
            WithMany(x => x.Reacts).
            HasForeignKey(x => x.ProfileId).
            OnDelete(DeleteBehavior.Restrict).
            IsRequired(true);
    }
}
