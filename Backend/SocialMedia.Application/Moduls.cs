using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Application.Abstractions.PostAbstractions;
using SocialMedia.Application.Implementations;
using SocialMedia.Application.UnitOfWorks;

namespace SocialMedia.Application;
public static class Moduls
{
    public static void AddApplicationService(this IServiceCollection service)
    {
        service.AddTransient<IUnitOfWork, UnitOfWork>();
        service.AddSignalR();

        service.AddScoped<IMailService, MailService>();
        service.AddScoped<IUsersService, UsersService>();
        service.AddScoped<IPostService, PostService>();
        service.AddScoped<IPostLikeService, PostLikeService>();
        service.AddScoped<ISavePostService, SavePostService>();
        service.AddScoped<ISharePostService, ShareService>();
        service.AddScoped<ICommentService, CommentService>();
        service.AddScoped<IProfileService, ProfileService>();
        service.AddScoped<IBlockService, BlockService>();
        service.AddScoped<ICommentLikeService, CommentLikeService>();
        service.AddScoped<IAuthenticationService, AuthenticationService>();
        service.AddScoped<IFollowerService, FollowerService>();
        service.AddScoped<IStoryService, StoryService>();
    }
}

