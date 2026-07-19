using Microsoft.EntityFrameworkCore;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Domain.Entities;
using TeamPortfolio.Infrastructure.Data;

namespace TeamPortfolio.Infrastructure.Repositories;

public class TeamMemberRepository : BaseRepository<TeamMember>, ITeamMemberRepository
{
    public TeamMemberRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TeamMember>> GetAllActiveAsync()
        => await _dbSet
            .Where(m => m.IsActive)
            .Include(m => m.Skills)
            .OrderBy(m => m.FirstName)
            .ToListAsync();

    public async Task<TeamMember?> GetBySlugAsync(string slug)
        => await _dbSet
            .Include(m => m.Skills)
            .Include(m => m.WorkExperiences)
            .Include(m => m.Educations)
            .Include(m => m.BlogPosts)
            .Include(m => m.PortfolioItemMembers)
                .ThenInclude(pim => pim.PortfolioItem)
            .FirstOrDefaultAsync(m => m.Slug == slug);

    public async Task<IEnumerable<TeamMember>> SearchAsync(string query)
    {
        var lowerQuery = query.ToLower();
        return await _dbSet
            .Where(m => m.IsActive &&
                (m.FirstName.ToLower().Contains(lowerQuery) ||
                 m.LastName.ToLower().Contains(lowerQuery) ||
                 m.Role.ToLower().Contains(lowerQuery)))
            .Include(m => m.Skills)
            .ToListAsync();
    }
}
