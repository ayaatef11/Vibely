using AutoMapper;
using SocialMedia.Core.Domain.Entities.Business.Profiles;
using SocialMedia.Application.DTOs.Responses.Posts;
using SocialMedia.Application.DTOs.Responses;
using SocialMedia.Application.DTOs.Responses.Users;
using SocialMedia.Core.Domain.Entities.Business.Chats;
using SocialMedia.Application.DTOs.Responses.Chats;
using SocialMedia.Application.DTOs.Requests.Chats;
namespace SocialMedia.API.Mapper;
public class PostMapper : Profile
{
    public PostMapper()
    {
        CreateMap<Post, PostResponse>();
        CreateMap<Post, PostResponseWithComments>();
        CreateMap<Comment, CommentResponse>()
            .ForMember(dest=>dest.ProfileImage,opt=>opt.MapFrom(src=>src.Profile.ProfileImage))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Profile.UserName));
        ;
        CreateMap<Follow, FollowResponse>();
        CreateMap<User, UserResponse>();
        CreateMap<UserProfile, ProfileResponse>();
        CreateMap<User, UserResponseWithStories>()
            .ForMember(dest => dest.Stories, opt => opt.MapFrom(src => src.Profile.Stories))
            .ForMember(dest=>dest.ProfileImage,opt=>opt.MapFrom(src=>src.Profile.ProfileImage));
        CreateMap<AddMessageRequest, Message>();
        CreateMap<Message, MessageResponse>();
        CreateMap<Chat, ChatResponse>();
        CreateMap<ChatParticipant, ChatParticipantResponse>();
    }
}

