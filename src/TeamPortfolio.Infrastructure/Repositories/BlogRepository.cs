using Microsoft.EntityFrameworkCore;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Domain.Entities;
using TeamPortfolio.Domain.Enums;
using TeamPortfolio.Infrastructure.Data;

namespace TeamPortfolio.Infrastructure.Repositories;

public class BlogRepository : BaseRepository<BlogPost>, IBlogRepository
{
    public BlogRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BlogPost>> GetPublishedAsync()
        => await _dbSet
            .Where(b => b.Status == BlogPostStatus.Published)
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Include(b => b.BlogPostTags)
                .ThenInclude(bt => bt.Tag)
            .OrderByDescending(b => b.PublishDate)
            .ToListAsync();

    public async Task<BlogPost?> GetBySlugAsync(string slug)
        => await _dbSet
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Include(b => b.BlogPostTags)
                .ThenInclude(bt => bt.Tag)
            .Include(b => b.Comments.Where(c => c.Status == CommentStatus.Approved))
            .FirstOrDefaultAsync(b => b.Slug == slug);

    public async Task<IEnumerable<BlogPost>> GetRelatedAsync(int postId, int count)
    {
        var post = await _dbSet
            .Include(b => b.BlogPostTags)
            .FirstOrDefaultAsync(b => b.Id == postId);

        if (post == null) return [];

        var tagIds = post.BlogPostTags.Select(bt => bt.TagId).ToList();

        return await _dbSet
            .Where(b => b.Id != postId &&
                   b.Status == BlogPostStatus.Published &&
                   (b.BlogPostTags.Any(bt => tagIds.Contains(bt.TagId)) ||
                    b.CategoryId == post.CategoryId))
            .Include(b => b.Author)
            .Include(b => b.Category)
            .OrderByDescending(b => b.PublishDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<BlogPost>> GetLatestAsync(int count)
        => await _dbSet
            .Where(b => b.Status == BlogPostStatus.Published)
            .Include(b => b.Author)
            .Include(b => b.Category)
            .OrderByDescending(b => b.PublishDate)
            .Take(count)
            .ToListAsync();

    public async Task<(IEnumerable<BlogPost> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, int? categoryId = null)
    {
        var query = _dbSet
            .Where(b => b.Status == BlogPostStatus.Published);

        if (categoryId.HasValue)
            query = query.Where(b => b.CategoryId == categoryId.Value);

        query = query.OrderByDescending(b => b.PublishDate);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(b => b.Author)
            .Include(b => b.Category)
            .ToListAsync();

        return (items, totalCount);
    }
}
