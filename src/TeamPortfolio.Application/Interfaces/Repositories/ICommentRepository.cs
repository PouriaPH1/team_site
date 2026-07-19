using TeamPortfolio.Domain.Entities;

namespace TeamPortfolio.Application.Interfaces.Repositories;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task<IEnumerable<Comment>> GetApprovedForPostAsync(int postId);
    Task<IEnumerable<Comment>> GetPendingAsync();
}
