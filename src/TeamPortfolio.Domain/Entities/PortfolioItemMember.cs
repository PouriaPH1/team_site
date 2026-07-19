namespace TeamPortfolio.Domain.Entities;

public class PortfolioItemMember
{
    public int PortfolioItemId { get; set; }
    public PortfolioItem PortfolioItem { get; set; } = null!;
    public int TeamMemberId { get; set; }
    public TeamMember TeamMember { get; set; } = null!;
}
