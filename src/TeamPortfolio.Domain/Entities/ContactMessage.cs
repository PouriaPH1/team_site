using TeamPortfolio.Domain.Common;

namespace TeamPortfolio.Domain.Entities;

public class ContactMessage : BaseEntity
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Body { get; set; } = "";
    public bool IsRead { get; set; } = false;
}
