using TeamPortfolio.Application.DTOs;
namespace TeamPortfolio.Web.ViewModels.Team;
public class TeamIndexViewModel
{
    public IEnumerable<TeamMemberDto> Members { get; set; } = [];
    public string? SearchQuery { get; set; }
}
