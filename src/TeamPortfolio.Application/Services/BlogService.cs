using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Application.Interfaces.Services;
using TeamPortfolio.Domain.Entities;
using TeamPortfolio.Domain.Enums;

namespace TeamPortfolio.Application.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;

    public BlogService(
        IBlogRepository repository,
        ICategoryRepository categoryRepository,
        ITeamMemberRepository teamMemberRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _teamMemberRepository = teamMemberRepository;
    }

    public async Task<PagedResult<BlogPostDto>> GetPublishedAsync(int page, int pageSize, int? categoryId = null)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(page, pageSize, categoryId);
        return new PagedResult<BlogPostDto>
        {
            Items = items.Select(MapToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<BlogPostDto?> GetBySlugAsync(string slug)
    {
        var post = await _repository.GetBySlugAsync(slug);
        return post == null ? null : MapToDto(post);
    }

    public async Task IncrementViewCountAsync(int postId)
    {
        var post = await _repository.GetByIdAsync(postId);
        if (post != null)
        {
            post.ViewCount++;
            post.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(post);
        }
    }

    public async Task<IEnumerable<BlogPostDto>> GetRelatedAsync(int postId, int count = 3)
    {
        var posts = await _repository.GetRelatedAsync(postId, count);
        return posts.Select(MapToDto);
    }

    public async Task<IEnumerable<BlogPostDto>> GetLatestAsync(int count = 3)
    {
        var posts = await _repository.GetLatestAsync(count);
        return posts.Select(MapToDto);
    }

    public async Task<BlogPostDto> CreateAsync(BlogPostDto dto, string applicationUserId)
    {
        // Resolve AuthorId from ApplicationUserId (ASP.NET Identity user ID)
        var allMembers = await _teamMemberRepository.GetAllAsync();
        var author = allMembers.FirstOrDefault(m => m.ApplicationUserId == applicationUserId);
        if (author == null)
            throw new InvalidOperationException(
                $"No TeamMember is linked to user '{applicationUserId}'. " +
                "Please link the user to a TeamMember record first.");

        // Resolve or create Category
        int categoryId;
        var categoryName = string.IsNullOrWhiteSpace(dto.CategoryName) ? "General" : dto.CategoryName.Trim();
        var allCategories = await _categoryRepository.GetAllAsync();
        var category = allCategories.FirstOrDefault(c =>
            string.Equals(c.Name, categoryName, StringComparison.OrdinalIgnoreCase));
        if (category == null)
        {
            category = await _categoryRepository.AddAsync(new Category
            {
                Name = categoryName,
                CreatedAt = DateTime.UtcNow
            });
        }
        categoryId = category.Id;

        var slugHelper = new Slugify.SlugHelper();
        var slug = slugHelper.GenerateSlug(dto.Title);
        if (string.IsNullOrWhiteSpace(slug))
            slug = $"post-{DateTime.UtcNow.Ticks}";

        // Ensure slug is unique
        var existingSlugs = (await _repository.GetAllAsync()).Select(p => p.Slug).ToHashSet();
        if (existingSlugs.Contains(slug))
            slug = $"{slug}-{DateTime.UtcNow.Ticks}";

        var post = new BlogPost
        {
            Title = dto.Title,
            Slug = slug,
            Body = dto.Body,
            CoverImagePath = dto.CoverImagePath,
            AuthorId = author.Id,
            CategoryId = categoryId,
            Status = BlogPostStatus.Published,
            PublishDate = DateTime.UtcNow,
            ViewCount = 0,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _repository.AddAsync(post);
        return MapToDto(created);
    }

    public async Task<BlogPostDto> UpdateAsync(int id, BlogPostDto dto, string userId, bool isAdmin)
    {
        var post = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"BlogPost {id} not found.");
        post.Title = dto.Title;
        post.Body = dto.Body;
        post.CoverImagePath = dto.CoverImagePath;
        post.Status = BlogPostStatus.Published;
        post.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(post);
        return MapToDto(post);
    }

    public async Task DeleteAsync(int id, string userId, bool isAdmin)
    {
        var post = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"BlogPost {id} not found.");
        await _repository.DeleteAsync(post);
    }

    public async Task PublishAsync(int id)
    {
        var post = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"BlogPost {id} not found.");
        post.Status = BlogPostStatus.Published;
        post.PublishDate = DateTime.UtcNow;
        post.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(post);
    }

    public async Task UnpublishAsync(int id)
    {
        var post = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"BlogPost {id} not found.");
        post.Status = BlogPostStatus.Draft;
        post.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(post);
    }

    private static BlogPostDto MapToDto(BlogPost b) => new()
    {
        Id = b.Id,
        Title = b.Title,
        Slug = b.Slug,
        Body = b.Body,
        CoverImagePath = b.CoverImagePath,
        Status = b.Status.ToString(),
        ViewCount = b.ViewCount,
        PublishDate = b.PublishDate,
        AuthorName = b.Author?.FullName ?? "",
        CategoryName = b.Category?.Name ?? ""
    };
}
