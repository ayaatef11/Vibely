using Microsoft.AspNetCore.Identity;
using SocialMedia.Core.Domain.Entities.Business.Posts;

namespace SocialMedia.Infrastructure.Domain.Entities.Security;
public class User : IdentityUser<Guid>
{
    public string FullName { set; get; }
    public string Location { set; get; }
    public UserProfile Profile { set; get; }
    public Guid ProfileId { set; get; }

    public ICollection<Block> BlockedUsers { set; get; } = new List<Block>();
    public ICollection<Block> BlockedByUsers { set; get; } = new List<Block>();
    public ICollection<Report> ReportedUsers { set; get; } = new List<Report>();
    public ICollection<Report> ReportedByUsers { set; get; } = new List<Report>();
    public ICollection<Role> Roles { set; get; } = new List<Role>();
    public ICollection<Reels> Reals { set; get; } = new List<Reels>();

}