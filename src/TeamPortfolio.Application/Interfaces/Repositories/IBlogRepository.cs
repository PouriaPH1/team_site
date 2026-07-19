using TeamPortfolio.Domain.Entities;

namespace TeamPortfolio.Application.Interfaces.Repositories;

public interface IBlogRepository : IBaseRepository<BlogPost>
{
    Task<IEnumerable<BlogPost>> GetPublishedAsync();
    Task<BlogPost?> GetBySlugAsync(string slug);
    Task<IEnumerable<BlogPost>> GetRelatedAsync(int postId, int count);
    Task<IEnumerable<BlogPost>> GetLatestAsync(int count);
    Task<(IEnumerable<BlogPost> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, int? categoryId = null);
}
