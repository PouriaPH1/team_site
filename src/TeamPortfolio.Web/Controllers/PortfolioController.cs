using Microsoft.AspNetCore.Mvc;
using TeamPortfolio.Application.Interfaces.Services;
using TeamPortfolio.Web.ViewModels.Portfolio;

namespace TeamPortfolio.Web.Controllers;

public class PortfolioController : Controller
{
    private readonly IPortfolioService? _portfolioService;
    private readonly ILogger<PortfolioController> _logger;
    private const int PageSize = 9;

    public PortfolioController(ILogger<PortfolioController> logger, IPortfolioService? portfolioService = null)
    {
        _logger = logger;
        _portfolioService = portfolioService;
    }

    public async Task<IActionResult> Index(int page = 1, string? tag = null)
    {
        ViewData["Title"] = "Portfolio";
        ViewData["BreadcrumbItems"] = new List<(string, string?)> { ("Portfolio", null) };

        var vm = new PortfolioIndexViewModel { Page = page, ActiveTag = tag };

        if (_portfolioService is not null)
        {
            try
            {
                IEnumerable<TeamPortfolio.Application.DTOs.PortfolioItemDto> allItems;

                if (!string.IsNullOrWhiteSpace(tag))
                    allItems = await _portfolioService.FilterByTagAsync(tag);
                else
                {
                    var paged = await _portfolioService.GetPagedAsync(page, PageSize);
                    vm.Items = paged.Items;
                    vm.TotalCount = paged.TotalCount;
                    vm.TotalPages = paged.TotalPages;
                    vm.HasPreviousPage = paged.HasPreviousPage;
                    vm.HasNextPage = paged.HasNextPage;

                    // Collect all tags from all published items for filter bar
                    var published = await _portfolioService.GetPublishedAsync();
                    vm.AllTags = published
                        .SelectMany(p => p.Technologies.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderBy(t => t)
                        .ToList();
                    return View(vm);
                }

                // Tag filter path — paginate in memory
                var list = allItems.ToList();
                vm.TotalCount = list.Count;
                vm.TotalPages = (int)Math.Ceiling(list.Count / (double)PageSize);
                if (vm.TotalPages < 1) vm.TotalPages = 1;
                vm.HasPreviousPage = page > 1;
                vm.HasNextPage = page < vm.TotalPages;
                vm.Items = list.Skip((page - 1) * PageSize).Take(PageSize);

                // All tags for filter bar
                var allPublished = await _portfolioService.GetPublishedAsync();
                vm.AllTags = allPublished
                    .SelectMany(p => p.Technologies.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(t => t)
                    .ToList();
            }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to load portfolio items"); }
        }

        return View(vm);
    }

    [Route("Portfolio/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        TeamPortfolio.Application.DTOs.PortfolioItemDto? item = null;
        if (_portfolioService is not null)
        {
            try { item = await _portfolioService.GetBySlugAsync(slug); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to load portfolio item {Slug}", slug); }
        }

        if (item is null) return NotFound();

        ViewData["Title"] = item.Title;
        ViewData["BreadcrumbItems"] = new List<(string, string?)>
        {
            ("Portfolio", "/Portfolio"),
            (item.Title, null)
        };
        return View(new PortfolioDetailViewModel { Item = item });
    }
}
