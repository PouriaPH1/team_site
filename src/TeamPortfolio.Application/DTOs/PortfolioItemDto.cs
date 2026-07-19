namespace TeamPortfolio.Application.DTOs;

public class PortfolioItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Description { get; set; } = "";
    public string Technologies { get; set; } = "";
    public DateTime StartDate { get; set; }
    public string? GitHubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string CoverImagePath { get; set; } = "";
    public bool IsPublished { get; set; }
}
