using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Domain.Entities.Business.Chats;
using SocialMedia.Core.Domain.Entities.Business.Posts;
using SocialMedia.Core.Domain.Entities.Business.Profiles;
using System;

namespace SocialMedia.Core.Context;
public class AppdbContext : IdentityDbContext<User, Role, Guid>
{

    public AppdbContext(DbContextOptions<AppdbContext> option) : base(option)
    {

    }

    public DbSet<Post> Posts { set; get; }
    public DbSet<Share> Shares { set; get; }
    public DbSet<Image> Images { set; get; }
    public DbSet<Video> Videos { set; get; }
    public DbSet<Block> Blocks { set; get; }
    public DbSet<Story> Stories { set; get; }
    public DbSet<StoryView> StoryViews { set; get; }
    public DbSet<StoryReaction> StoryReactions { set; get; }
    public DbSet<Follow> Follows { set; get; }
    public DbSet<Comment> Comments { set; get; }
    public DbSet<UserProfile> Profiles { set; get; }
    public DbSet<User> Users { set; get; }
    public DbSet<Reels> Reels { set; get; }
    public DbSet<PostLikes> PostLike { set; get; }
    public DbSet<Report> Reports { set; get; }
    public DbSet<CommentLikes> CommentLikes { set; get; }
    public DbSet<Chat> Chats { get; set; }

    public DbSet<Message> Messages { get; set; }

    public DbSet<ChatParticipant> ChatParticipants { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppdbContext).Assembly);
    }
}