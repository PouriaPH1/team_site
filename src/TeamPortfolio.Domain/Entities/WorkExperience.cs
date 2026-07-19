using TeamPortfolio.Domain.Common;

namespace TeamPortfolio.Domain.Entities;

public class WorkExperience : BaseEntity
{
    public string CompanyName { get; set; } = "";
    public string Role { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public int MemberId { get; set; }
    public TeamMember Member { get; set; } = null!;
}
