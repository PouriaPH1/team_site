using TeamPortfolio.Domain.Common;

namespace TeamPortfolio.Domain.Entities;

public class PortfolioImage : BaseEntity
{
    public string ImagePath { get; set; } = "";
    public int SortOrder { get; set; }
    public int PortfolioItemId { get; set; }
    public PortfolioItem PortfolioItem { get; set; } = null!;
}
