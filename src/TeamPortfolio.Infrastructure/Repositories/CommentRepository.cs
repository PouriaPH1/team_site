using Microsoft.EntityFrameworkCore;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Domain.Entities;
using TeamPortfolio.Domain.Enums;
using TeamPortfolio.Infrastructure.Data;

namespace TeamPortfolio.Infrastructure.Repositories;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Comment>> GetApprovedForPostAsync(int postId)
        => await _dbSet
            .Where(c => c.PostId == postId && c.Status == CommentStatus.Approved)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<Comment>> GetPendingAsync()
        => await _dbSet
            .Where(c => c.Status == CommentStatus.Pending)
            .Include(c => c.Post)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
}
