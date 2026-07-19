using TeamPortfolio.Domain.Common;

namespace TeamPortfolio.Domain.Entities;

public class Education : BaseEntity
{
    public string InstitutionName { get; set; } = "";
    public string Degree { get; set; } = "";
    public string FieldOfStudy { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int MemberId { get; set; }
    public TeamMember Member { get; set; } = null!;
}
