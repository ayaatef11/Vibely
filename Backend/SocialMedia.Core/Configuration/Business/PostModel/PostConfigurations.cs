using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SocialMedia.Infrastructure.Persistence.Configuration.Business.PostModel;
public class PostConfigurations : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Post").HasKey(x => x.Id);

        builder.HasOne(x => x.Profile).
            WithMany(x => x.Posts).
            HasForeignKey(x => x.ProfileId).
            OnDelete(DeleteBehavior.Restrict).
            IsRequired(true);
    }
}
