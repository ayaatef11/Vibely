namespace SocialMedia.Application.Abstractions;
public interface ICommentService  
{
    public ValueTask<CommentResponse> AddComment(AddCommentRequest comment);
    public ValueTask<CommentResponse?> EditComment(EditCommentRequest comment);
    public ValueTask DeleteComment(Guid id);
    ValueTask<IEnumerable<CommentResponse>> GetComments(Guid id);
}
