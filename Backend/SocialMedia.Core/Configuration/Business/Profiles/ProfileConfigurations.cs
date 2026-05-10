using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Domain.Entities.Business.Profiles;
using SocialMedia.Infrastructure.Domain.Entities.Security;

namespace SocialMedia.Infrastructure.Persistence.Configuration.Business.Profiles;
public class ProfileConfigurations : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("Profile").HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithOne(x => x.Profile)
            .HasForeignKey<User>(x => x.ProfileId);
    }
}

