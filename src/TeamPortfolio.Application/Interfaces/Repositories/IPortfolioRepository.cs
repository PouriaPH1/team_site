using TeamPortfolio.Domain.Entities;

namespace TeamPortfolio.Application.Interfaces.Repositories;

public interface IPortfolioRepository : IBaseRepository<PortfolioItem>
{
    Task<IEnumerable<PortfolioItem>> GetPublishedAsync();
    Task<PortfolioItem?> GetBySlugAsync(string slug);
    Task<IEnumerable<PortfolioItem>> FilterByTagAsync(string tag);
    Task<IEnumerable<PortfolioItem>> GetLatestAsync(int count);
    Task<(IEnumerable<PortfolioItem> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
}
