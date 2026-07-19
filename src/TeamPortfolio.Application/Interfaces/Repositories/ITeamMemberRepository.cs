using TeamPortfolio.Domain.Entities;

namespace TeamPortfolio.Application.Interfaces.Repositories;

public interface ITeamMemberRepository : IBaseRepository<TeamMember>
{
    Task<IEnumerable<TeamMember>> GetAllActiveAsync();
    Task<TeamMember?> GetBySlugAsync(string slug);
    Task<IEnumerable<TeamMember>> SearchAsync(string query);
}
