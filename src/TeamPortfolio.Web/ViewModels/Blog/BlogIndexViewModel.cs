using TeamPortfolio.Application.DTOs;

namespace TeamPortfolio.Web.ViewModels.Blog;

public class BlogIndexViewModel
{
    public IEnumerable<BlogPostDto> Posts { get; set; } = [];
    public int Page { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
    public int? ActiveCategoryId { get; set; }
    public string? ActiveCategoryName { get; set; }
}
