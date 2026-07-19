using TeamPortfolio.Application.DTOs;

namespace TeamPortfolio.Application.Interfaces.Services;

public interface IBlogService
{
    Task<PagedResult<BlogPostDto>> GetPublishedAsync(int page, int pageSize, int? categoryId = null);
    Task<BlogPostDto?> GetBySlugAsync(string slug);
    Task IncrementViewCountAsync(int postId);
    Task<IEnumerable<BlogPostDto>> GetRelatedAsync(int postId, int count = 3);
    Task<IEnumerable<BlogPostDto>> GetLatestAsync(int count = 3);
    Task<BlogPostDto> CreateAsync(BlogPostDto dto, string authorId);
    Task<BlogPostDto> UpdateAsync(int id, BlogPostDto dto, string userId, bool isAdmin);
    Task DeleteAsync(int id, string userId, bool isAdmin);
    Task PublishAsync(int id);
    Task UnpublishAsync(int id);
}
