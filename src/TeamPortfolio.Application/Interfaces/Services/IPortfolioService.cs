using TeamPortfolio.Application.DTOs;

namespace TeamPortfolio.Application.Interfaces.Services;

public interface IPortfolioService
{
    Task<IEnumerable<PortfolioItemDto>> GetAllAsync();   // همه (published + draft) — برای admin
    Task<IEnumerable<PortfolioItemDto>> GetPublishedAsync();
    Task<PortfolioItemDto?> GetBySlugAsync(string slug);
    Task<IEnumerable<PortfolioItemDto>> FilterByTagAsync(string tag);
    Task<PagedResult<PortfolioItemDto>> GetPagedAsync(int page, int pageSize);
    Task<IEnumerable<PortfolioItemDto>> GetLatestAsync(int count);
    Task<PortfolioItemDto> CreateAsync(PortfolioItemDto dto);
    Task<PortfolioItemDto> UpdateAsync(int id, PortfolioItemDto dto);
    Task DeleteAsync(int id);
}
