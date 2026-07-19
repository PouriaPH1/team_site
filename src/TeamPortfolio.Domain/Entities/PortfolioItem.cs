using TeamPortfolio.Domain.Common;

namespace TeamPortfolio.Domain.Entities;

public class PortfolioItem : BaseEntity
{
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Description { get; set; } = "";
    public string Technologies { get; set; } = "";   // comma-separated
    public DateTime StartDate { get; set; }
    public string? GitHubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string CoverImagePath { get; set; } = "";
    public bool IsPublished { get; set; } = false;
    public ICollection<PortfolioItemMember> Members { get; set; } = [];
    public ICollection<PortfolioImage> Images { get; set; } = [];
}
