using TeamPortfolio.Domain.Common;
using TeamPortfolio.Domain.Enums;

namespace TeamPortfolio.Domain.Entities;

public class Comment : BaseEntity
{
    public string CommenterName { get; set; } = "";
    public string CommenterEmail { get; set; } = "";
    public string Body { get; set; } = "";
    public CommentStatus Status { get; set; } = CommentStatus.Pending;
    public int PostId { get; set; }
    public BlogPost Post { get; set; } = null!;
}
