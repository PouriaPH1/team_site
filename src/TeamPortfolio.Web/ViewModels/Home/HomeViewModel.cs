using TeamPortfolio.Application.DTOs;

namespace TeamPortfolio.Web.ViewModels.Home;

public class HomeViewModel
{
    public HeroSectionViewModel Hero { get; set; } = new();
    public IEnumerable<PortfolioItemDto> LatestProjects { get; set; } = [];
    public IEnumerable<BlogPostDto> LatestPosts { get; set; } = [];
    public StatisticsViewModel Statistics { get; set; } = new();
    public IEnumerable<string> Technologies { get; set; } = [];
}

public class HeroSectionViewModel
{
    public string TeamName { get; set; } = "DevTeam";
    public string Tagline { get; set; } = "Building modern software solutions that scale";
    public string PrimaryCtaText { get; set; } = "View Our Work";
    public string PrimaryCtaUrl { get; set; } = "/Portfolio";
    public string SecondaryCtaText { get; set; } = "Meet the Team";
    public string SecondaryCtaUrl { get; set; } = "/Team";
}

public class StatisticsViewModel
{
    public int TotalProjects { get; set; }
    public int TotalMembers { get; set; }
    public int TotalTechnologies { get; set; }
    public int YearsOfExperience { get; set; }
}
