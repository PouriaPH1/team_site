using TeamPortfolio.Domain.Common;

namespace TeamPortfolio.Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = "";
    public ICollection<BlogPostTag> BlogPostTags { get; set; } = [];
}
