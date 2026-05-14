using AutoMapper;

namespace SocialMedia.API.Mapper;
public class PostMapper : Profile
{
    public PostMapper()
    {
        CreateMap<Post, PostResponse>();
        CreateMap<Post, PostResponseWithComments>();
        CreateMap<Comment, CommentResponse>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Profile.UserName));
        ;
        CreateMap<Follow, FollowResponse>();
        CreateMap<User, UserResponse>();
        CreateMap<UserProfile, ProfileResponse>();
        CreateMap<User, UserResponseWithStories>()
            .ForMember(dest => dest.Stories, opt => opt.MapFrom(src => src.Profile.Stories));
        CreateMap<AddMessageRequest, Message>();
        CreateMap<Message, MessageResponse>();
        CreateMap<Chat, ChatResponse>();
        CreateMap<ChatParticipant, ChatParticipantResponse>();
        CreateMap<Reels, ReelsResponse>();
        CreateMap<Story, StoryResponse>();
        CreateMap<AddStoryCommentRequest, StoryComment>();
        CreateMap<StoryComment, StoryCommentResponse>();
        CreateMap<Notification, NotificationResponse>();
    }
}

