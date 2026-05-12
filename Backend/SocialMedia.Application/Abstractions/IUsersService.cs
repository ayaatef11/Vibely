namespace SocialMedia.Application.Abstractions;
public interface IUsersService
{
    Task ReportUser(Guid userId, Guid reporterId);
    Task<List<UserResponse>> SuggestUser(Guid userId);
    List<UserResponse> SearchUser(string keyword);
}

