using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Infrastructure.Domain.Entities.Business.Posts;

namespace SocialMedia.Infrastructure.Persistence.Configuration.Business.PostModel;
public class CommentLikesConfigurations : IEntityTypeConfiguration<CommentLikes>
{
    public void Configure(EntityTypeBuilder<CommentLikes> builder)
    {
        builder.ToTable("CommentLikes").HasKey(x => x.Id);

        //builder.HasOne(x => x.Profile).
        //    WithMany(x => x.CommentLikes).
        //    HasForeignKey(x => x.ProfileId).
        //    OnDelete(DeleteBehavior.Restrict).
        //    IsRequired(true);

   
    }
}