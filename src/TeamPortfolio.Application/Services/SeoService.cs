using TeamPortfolio.Application.Interfaces.Services;
using System.Text.RegularExpressions;

namespace TeamPortfolio.Application.Services;

public class SeoService : ISeoService
{
    private const string SiteTitle = "TeamPortfolio";

    public string GenerateTitle(string pageTitle)
    {
        if (string.IsNullOrWhiteSpace(pageTitle)) return SiteTitle;
        return pageTitle.Length > 60
            ? $"{pageTitle[..57]}... | {SiteTitle}"
            : $"{pageTitle} | {SiteTitle}";
    }

    public string GenerateMetaDescription(string content, int maxLength = 160)
    {
        if (string.IsNullOrWhiteSpace(content)) return "Professional software development team specializing in modern web solutions.";
        // Strip HTML tags
        var stripped = Regex.Replace(content, "<[^>]+>", " ");
        stripped = Regex.Replace(stripped, @"\s+", " ").Trim();
        if (stripped.Length <= maxLength) return stripped;
        var truncated = stripped[..maxLength];
        var lastSpace = truncated.LastIndexOf(' ');
        return lastSpace > 0 ? truncated[..lastSpace] + "..." : truncated + "...";
    }

    public Dictionary<string, string> GenerateOpenGraphTags(string title, string description, string? imageUrl)
    {
        var tags = new Dictionary<string, string>
        {
            ["og:type"] = "website",
            ["og:site_name"] = SiteTitle,
            ["og:title"] = title,
            ["og:description"] = description
        };
        if (!string.IsNullOrWhiteSpace(imageUrl))
            tags["og:image"] = imageUrl;
        return tags;
    }
}
