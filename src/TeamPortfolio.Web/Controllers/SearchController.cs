using Microsoft.AspNetCore.Mvc;
using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Interfaces.Services;

namespace TeamPortfolio.Web.Controllers;

public class SearchController : Controller
{
    private readonly ISearchService? _searchService;
    private readonly ILogger<SearchController> _logger;

    public SearchController(ILogger<SearchController> logger, ISearchService? searchService = null)
    {
        _logger = logger;
        _searchService = searchService;
    }

    public async Task<IActionResult> Index(string? q)
    {
        ViewData["Title"] = string.IsNullOrWhiteSpace(q) ? "Search" : $"Search: {q}";
        ViewData["BreadcrumbItems"] = new List<(string, string?)> { ("Search", null) };

        if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
        {
            ViewBag.Query = q;
            ViewBag.Result = null;
            ViewBag.TooShort = !string.IsNullOrWhiteSpace(q);
            return View();
        }

        SearchResultDto? result = null;
        if (_searchService is not null)
        {
            try { result = await _searchService.SearchAsync(q.Trim()); }
            catch (Exception ex) { _logger.LogWarning(ex, "Search failed for query {Q}", q); }
        }

        ViewBag.Query = q.Trim();
        ViewBag.Result = result;
        return View();
    }
}
