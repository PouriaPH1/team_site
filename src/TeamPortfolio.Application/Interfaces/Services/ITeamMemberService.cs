using TeamPortfolio.Application.DTOs;

namespace TeamPortfolio.Application.Interfaces.Services;

public interface ITeamMemberService
{
    Task<IEnumerable<TeamMemberDto>> GetAllActiveAsync();
    Task<TeamMemberDto?> GetByIdAsync(int id);
    Task<TeamMemberDto?> GetBySlugAsync(string slug);
    Task<IEnumerable<TeamMemberDto>> SearchAsync(string query);
    Task<TeamMemberDto> CreateAsync(TeamMemberDto dto);
    Task<TeamMemberDto> UpdateAsync(int id, TeamMemberDto dto);
    Task DeleteAsync(int id);
    Task AddSkillAsync(int memberId, string skillName, int proficiencyLevel);
}
