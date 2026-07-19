using TeamPortfolio.Domain.Common;

namespace TeamPortfolio.Domain.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; } = "";
    public int ProficiencyLevel { get; set; }  // 1–100
    public int MemberId { get; set; }
    public TeamMember Member { get; set; } = null!;
}
