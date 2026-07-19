using TeamPortfolio.Application.DTOs;

namespace TeamPortfolio.Web.ViewModels.Portfolio;

public class PortfolioIndexViewModel
{
    public IEnumerable<PortfolioItemDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
    public string? ActiveTag { get; set; }
    public IEnumerable<string> AllTags { get; set; } = [];
}
