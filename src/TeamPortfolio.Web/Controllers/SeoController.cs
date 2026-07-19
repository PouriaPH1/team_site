using Microsoft.AspNetCore.Mvc;
using TeamPortfolio.Application.Interfaces.Services;

namespace TeamPortfolio.Web.Controllers;

public class SeoController : Controller
{
    private readonly IPortfolioService? _portfolioService;
    private readonly IBlogService? _blogService;
    private readonly ILogger<SeoController> _logger;

    public SeoController(ILogger<SeoController> logger, IPortfolioService? portfolioService = null, IBlogService? blogService = null)
    { _logger = logger; _portfolioService = portfolioService; _blogService = blogService; }

    [Route("sitemap.xml")]
    public async Task<IActionResult> Sitemap()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var urls = new List<string>
        {
            baseUrl + "/",
            baseUrl + "/About",
            baseUrl + "/Team",
            baseUrl + "/Portfolio",
            baseUrl + "/Blog",
            baseUrl + "/Contact"
        };

        if (_portfolioService is not null)
            try { foreach (var p in await _portfolioService.GetPublishedAsync()) urls.Add(baseUrl + "/Portfolio/" + p.Slug); }
            catch (Exception ex) { _logger.LogWarning(ex, "Sitemap portfolio failed"); }

        if (_blogService is not null)
            try { var r = await _blogService.GetPublishedAsync(1, 500); foreach (var b in r.Items) urls.Add(baseUrl + "/Blog/" + b.Slug); }
            catch (Exception ex) { _logger.LogWarning(ex, "Sitemap blog failed"); }

        var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n";
        xml += string.Join("\n", urls.Select(u => $"  <url><loc>{u}</loc></url>"));
        xml += "\n</urlset>";
        return Content(xml, "application/xml");
    }

    [Route("robots.txt")]
    public IActionResult Robots()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var content = $"User-agent: *\nAllow: /\nDisallow: /Admin/\nDisallow: /Account/\n\nSitemap: {baseUrl}/sitemap.xml\n";
        return Content(content, "text/plain");
    }
}
