using Microsoft.EntityFrameworkCore;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Domain.Entities;
using TeamPortfolio.Infrastructure.Data;

namespace TeamPortfolio.Infrastructure.Repositories;

public class PortfolioRepository : BaseRepository<PortfolioItem>, IPortfolioRepository
{
    public PortfolioRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PortfolioItem>> GetPublishedAsync()
        => await _dbSet
            .Where(p => p.IsPublished)
            .Include(p => p.Images)
            .Include(p => p.Members)
                .ThenInclude(m => m.TeamMember)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

    public async Task<PortfolioItem?> GetBySlugAsync(string slug)
        => await _dbSet
            .Include(p => p.Images)
            .Include(p => p.Members)
                .ThenInclude(m => m.TeamMember)
            .FirstOrDefaultAsync(p => p.Slug == slug);

    public async Task<IEnumerable<PortfolioItem>> FilterByTagAsync(string tag)
    {
        var lowerTag = tag.ToLower();
        return await _dbSet
            .Where(p => p.IsPublished && p.Technologies.ToLower().Contains(lowerTag))
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<PortfolioItem>> GetLatestAsync(int count)
        => await _dbSet
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .Include(p => p.Images)
            .ToListAsync();

    public async Task<(IEnumerable<PortfolioItem> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = _dbSet
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.CreatedAt);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.Images)
            .ToListAsync();

        return (items, totalCount);
    }
}
