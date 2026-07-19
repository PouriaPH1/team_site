using TeamPortfolio.Domain.Common;

namespace TeamPortfolio.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public ICollection<BlogPost> BlogPosts { get; set; } = [];
}
