using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Interfaces.Services;
using TeamPortfolio.Web.Models;
using TeamPortfolio.Web.ViewModels.Home;

namespace TeamPortfolio.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPortfolioService? _portfolioService;
    private readonly IBlogService? _blogService;
    private readonly ITeamMemberService? _teamMemberService;
    private readonly ICacheService? _cacheService;

    private const string CacheKey = "home_page_data";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    public HomeController(
        ILogger<HomeController> logger,
        IPortfolioService? portfolioService = null,
        IBlogService? blogService = null,
        ITeamMemberService? teamMemberService = null,
        ICacheService? cacheService = null)
    {
        _logger = logger;
        _portfolioService = portfolioService;
        _blogService = blogService;
        _teamMemberService = teamMemberService;
        _cacheService = cacheService;
    }

    public async Task<IActionResult> Index()
    {
        // Try cache first
        if (_cacheService is not null)
        {
            try
            {
                var cached = await _cacheService.GetAsync<HomeViewModel>(CacheKey);
                if (cached is not null)
                    return View(cached);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache read failed for key {Key}", CacheKey);
            }
        }

        var viewModel = await BuildViewModelAsync();

        // Store in cache
        if (_cacheService is not null)
        {
            try
            {
                await _cacheService.SetAsync(CacheKey, viewModel, CacheDuration);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache write failed for key {Key}", CacheKey);
            }
        }

        return View(viewModel);
    }

    private async Task<HomeViewModel> BuildViewModelAsync()
    {
        IEnumerable<PortfolioItemDto> projects = [];
        IEnumerable<BlogPostDto> posts = [];
        IEnumerable<TeamMemberDto> members = [];

        if (_portfolioService is not null)
        {
            try { projects = await _portfolioService.GetLatestAsync(6); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to fetch latest projects"); }
        }

        if (_blogService is not null)
        {
            try { posts = await _blogService.GetLatestAsync(3); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to fetch latest blog posts"); }
        }

        if (_teamMemberService is not null)
        {
            try { members = await _teamMemberService.GetAllActiveAsync(); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to fetch team members"); }
        }

        // Derive unique technologies from portfolio items
        var technologies = projects
            .SelectMany(p => p.Technologies.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(t => t)
            .ToList();

        // Fallback technology list if no portfolio data yet
        if (technologies.Count == 0)
        {
            technologies =
            [
                "ASP.NET Core", "C#", "Entity Framework Core",
                "PostgreSQL", "Redis", "Docker",
                "TypeScript", "React", "Tailwind CSS",
                "Azure", "GitHub Actions", "REST APIs"
            ];
        }

        return new HomeViewModel
        {
            Hero = new HeroSectionViewModel
            {
                TeamName = "DevTeam",
                Tagline = "Building modern software solutions that scale",
                PrimaryCtaText = "View Our Work",
                PrimaryCtaUrl = "/Portfolio",
                SecondaryCtaText = "Meet the Team",
                SecondaryCtaUrl = "/Team"
            },
            LatestProjects = projects,
            LatestPosts = posts,
            Technologies = technologies,
            Statistics = new StatisticsViewModel
            {
                TotalProjects = projects.Count(),
                TotalMembers = members.Count(),
                TotalTechnologies = technologies.Count,
                YearsOfExperience = DateTime.UtcNow.Year - 2018
            }
        };
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
