using TeamPortfolio.Domain.Common;
using TeamPortfolio.Domain.Enums;

namespace TeamPortfolio.Domain.Entities;

public class BlogPost : BaseEntity
{
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Body { get; set; } = "";
    public string? CoverImagePath { get; set; }
    public BlogPostStatus Status { get; set; } = BlogPostStatus.Draft;
    public int ViewCount { get; set; } = 0;
    public DateTime? PublishDate { get; set; }
    public int AuthorId { get; set; }
    public TeamMember Author { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<BlogPostTag> BlogPostTags { get; set; } = [];
}
