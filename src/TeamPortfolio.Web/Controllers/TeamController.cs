using Microsoft.AspNetCore.Mvc;
using TeamPortfolio.Application.Interfaces.Services;
using TeamPortfolio.Web.ViewModels.Team;

namespace TeamPortfolio.Web.Controllers;

public class TeamController : Controller
{
    private readonly ITeamMemberService? _teamService;
    private readonly ILogger<TeamController> _logger;

    public TeamController(ILogger<TeamController> logger, ITeamMemberService? teamService = null)
    {
        _logger = logger;
        _teamService = teamService;
    }

    public async Task<IActionResult> Index(string? q)
    {
        var members = Enumerable.Empty<TeamPortfolio.Application.DTOs.TeamMemberDto>();
        if (_teamService is not null)
        {
            try
            {
                members = string.IsNullOrWhiteSpace(q)
                    ? await _teamService.GetAllActiveAsync()
                    : await _teamService.SearchAsync(q);
            }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to fetch team members"); }
        }

        ViewData["Title"] = "Our Team";
        ViewData["BreadcrumbItems"] = new List<(string, string?)> { ("Team", null) };
        return View(new TeamIndexViewModel { Members = members, SearchQuery = q });
    }

    [Route("Team/{slug}")]
    public async Task<IActionResult> Profile(string slug)
    {
        TeamPortfolio.Application.DTOs.TeamMemberDto? member = null;
        if (_teamService is not null)
        {
            try { member = await _teamService.GetBySlugAsync(slug); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to fetch member {Slug}", slug); }
        }

        if (member is null) return NotFound();

        ViewData["Title"] = member.FullName;
        ViewData["BreadcrumbItems"] = new List<(string, string?)>
        {
            ("Team", "/Team"),
            (member.FullName, null)
        };
        return View(new MemberProfileViewModel { Member = member });
    }
}
