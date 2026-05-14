using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Abstractions;
using SocialMedia.Application.Helpers;

namespace SocialMedia.Application.Implementations;
public class PostLikeService(AppdbContext _context,INotificationsService _notificationService) : IPostLikeService
{
    public async ValueTask DisLikeAsync(DisLikeRequest request)
    {
        var like = await _context.PostLike.FirstOrDefaultAsync(x => x.PostId == request.PostId && x.ProfileId == request.ProfileId);
        if (like == null)
            throw new NotFoundException( "LikeNotFound");

        var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.PostId);
        if (post == null)
            throw new NotFoundException("PostNotFound");

        post.ReactsCount--;
        _context.PostLike.Remove(like);
        var dislikeOperation = await _context.SaveChangesAsync();

        if( dislikeOperation <= 0) throw new BadRequestException("Invalid");
    }

    public async ValueTask LikeAsync(LikeRequest request)
    {
        var profile =await _context.Profiles.FirstOrDefaultAsync(u => u.Id == request.ProfileId);
        if (profile == null)
            throw new NotFoundException("Profile not found");

        var react = new PostLikes()
        {
            PostId = request.PostId,
            ReactionType = request.React,
            ProfileId = request.ProfileId
        };
       
        var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.PostId);
        if (post == null)
            throw new NotFoundException("Post Not Found");

        post.ReactsCount++;
        await _context.PostLike.AddAsync(react);
        var likeOperation = await _context.SaveChangesAsync();
        var notificationRequest = new NotificationRequest()
        {
            RecipientId = post.ProfileId,
            SenderId = request.ProfileId,
            Type = NotificationType.Like,
            Message = $"{profile.FullName} liked your post",
            ReferenceId = post.Id
        };
        await _notificationService.SendNotificationAsync(notificationRequest);
        if ( likeOperation <= 0) throw new BadRequestException("Invalid Add Like");
    }
}