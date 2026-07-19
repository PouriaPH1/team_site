using TeamPortfolio.Application.DTOs;

namespace TeamPortfolio.Application.Interfaces.Services;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetApprovedForPostAsync(int postId);
    Task<IEnumerable<CommentDto>> GetPendingAsync();
    Task<CommentDto> SubmitAsync(CommentDto dto);
    Task ApproveAsync(int id);
    Task DeleteAsync(int id);
    Task UpdateBodyAsync(int id, string newBody);
}
