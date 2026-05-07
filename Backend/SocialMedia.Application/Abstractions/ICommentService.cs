using SocialMedia.Application.DTOs.Responses;
using SocialMedia.Core.Domain.DTOs.Requests.Comment;

namespace SocialMedia.Application.Abstractions;
public interface ICommentService  
{
    public ValueTask<CommentResponse> AddComment(AddCommentRequest comment);
    public ValueTask<CommentResponse?> EditComment(EditCommentRequest comment);
    public ValueTask<int> DeleteComment(Guid id);
    ValueTask<IEnumerable<CommentResponse>> GetComments(Guid id);
}
