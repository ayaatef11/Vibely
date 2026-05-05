using SocialMedia.Application.DTOs.Responses;
using SocialMedia.Core.Domain.DTOs.Requests.Comment;

namespace SocialMedia.Application.Abstractions;
public interface ICommentService  
{
    public ValueTask<CommentResponse> AddComment(AddCommentDTO comment);
    public ValueTask<CommentResponse?> EditComment(EditCommentDTO comment);
    public ValueTask<int> DeleteComment(Guid id);
    ValueTask<IEnumerable<CommentResponse>> GetComments(Guid id);
}
