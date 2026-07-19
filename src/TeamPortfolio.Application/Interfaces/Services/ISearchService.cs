using TeamPortfolio.Application.DTOs;

namespace TeamPortfolio.Application.Interfaces.Services;

public interface ISearchService
{
    Task<SearchResultDto> SearchAsync(string query);
}
