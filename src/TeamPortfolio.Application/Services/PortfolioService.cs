using Slugify;
using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Application.Interfaces.Services;
using TeamPortfolio.Domain.Entities;

namespace TeamPortfolio.Application.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IPortfolioRepository _repository;
    private readonly SlugHelper _slugHelper;

    public PortfolioService(IPortfolioRepository repository)
    {
        _repository = repository;
        _slugHelper = new SlugHelper();
    }

    public async Task<IEnumerable<PortfolioItemDto>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<PortfolioItemDto>> GetPublishedAsync()
    {
        var items = await _repository.GetPublishedAsync();
        return items.Select(MapToDto);
    }

    public async Task<PortfolioItemDto?> GetBySlugAsync(string slug)
    {
        var item = await _repository.GetBySlugAsync(slug);
        return item == null ? null : MapToDto(item);
    }

    public async Task<IEnumerable<PortfolioItemDto>> FilterByTagAsync(string tag)
    {
        var items = await _repository.FilterByTagAsync(tag);
        return items.Select(MapToDto);
    }

    public async Task<PagedResult<PortfolioItemDto>> GetPagedAsync(int page, int pageSize)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(page, pageSize);
        return new PagedResult<PortfolioItemDto>
        {
            Items = items.Select(MapToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<IEnumerable<PortfolioItemDto>> GetLatestAsync(int count)
    {
        var items = await _repository.GetLatestAsync(count);
        return items.Select(MapToDto);
    }

    public async Task<PortfolioItemDto> CreateAsync(PortfolioItemDto dto)
    {
        // Auto-generate slug from title
        var slug = _slugHelper.GenerateSlug(dto.Title);
        // Ensure uniqueness by appending timestamp if needed
        if (string.IsNullOrWhiteSpace(slug))
            slug = $"project-{DateTime.UtcNow.Ticks}";

        var item = new PortfolioItem
        {
            Title = dto.Title,
            Slug = slug,
            Description = dto.Description,
            Technologies = dto.Technologies,
            StartDate = dto.StartDate,
            GitHubUrl = dto.GitHubUrl,
            DemoUrl = dto.DemoUrl,
            CoverImagePath = dto.CoverImagePath,
            IsPublished = true,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _repository.AddAsync(item);
        return MapToDto(created);
    }

    public async Task<PortfolioItemDto> UpdateAsync(int id, PortfolioItemDto dto)
    {
        var item = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"PortfolioItem with id {id} not found.");
        item.Title = dto.Title;
        // اگر slug خالیه (پروژه قدیمی)، دوباره generate کن
        if (string.IsNullOrWhiteSpace(item.Slug))
            item.Slug = _slugHelper.GenerateSlug(dto.Title);
        item.Description = dto.Description;
        item.Technologies = dto.Technologies;
        item.StartDate = dto.StartDate;
        item.GitHubUrl = dto.GitHubUrl;
        item.DemoUrl = dto.DemoUrl;
        item.CoverImagePath = dto.CoverImagePath;
        item.IsPublished = true;
        item.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(item);
        return MapToDto(item);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"PortfolioItem with id {id} not found.");
        await _repository.DeleteAsync(item);
    }

    private static PortfolioItemDto MapToDto(PortfolioItem p) => new()
    {
        Id = p.Id,
        Title = p.Title,
        Slug = p.Slug,
        Description = p.Description,
        Technologies = p.Technologies,
        StartDate = p.StartDate,
        GitHubUrl = p.GitHubUrl,
        DemoUrl = p.DemoUrl,
        CoverImagePath = p.CoverImagePath,
        IsPublished = p.IsPublished
    };
}
