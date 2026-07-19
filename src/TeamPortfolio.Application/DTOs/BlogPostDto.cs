namespace TeamPortfolio.Application.DTOs;

public class BlogPostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Body { get; set; } = "";
    public string? CoverImagePath { get; set; }
    public string Status { get; set; } = "";
    public int ViewCount { get; set; }
    public DateTime? PublishDate { get; set; }
    public string AuthorName { get; set; } = "";
    public string CategoryName { get; set; } = "";
}
