using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Application.Interfaces.Services;

namespace TeamPortfolio.Application.Services;

public class SearchService : ISearchService
{
    private readonly ITeamMemberRepository _memberRepo;
    private readonly IPortfolioRepository _portfolioRepo;
    private readonly IBlogRepository _blogRepo;

    public SearchService(
        ITeamMemberRepository memberRepo,
        IPortfolioRepository portfolioRepo,
        IBlogRepository blogRepo)
    {
        _memberRepo = memberRepo;
        _portfolioRepo = portfolioRepo;
        _blogRepo = blogRepo;
    }

    public async Task<SearchResultDto> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2)
            return new SearchResultDto { Query = query };

        var lowerQuery = query.Trim().ToLower();

        var members = await _memberRepo.SearchAsync(lowerQuery);
        var memberDtos = members.Select(m => new TeamMemberDto
        {
            Id = m.Id,
            FirstName = m.FirstName,
            LastName = m.LastName,
            FullName = m.FullName,
            Role = m.Role,
            Slug = m.Slug,
            ProfilePhotoPath = m.ProfilePhotoPath,
            IsActive = m.IsActive
        });

        var allPortfolio = await _portfolioRepo.GetPublishedAsync();
        var projectDtos = allPortfolio
            .Where(p => p.Title.ToLower().Contains(lowerQuery) ||
                        p.Description.ToLower().Contains(lowerQuery))
            .Select(p => new PortfolioItemDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                Description = p.Description,
                Technologies = p.Technologies,
                CoverImagePath = p.CoverImagePath,
                IsPublished = p.IsPublished
            });

        var allPosts = await _blogRepo.GetPublishedAsync();
        var articleDtos = allPosts
            .Where(b => b.Title.ToLower().Contains(lowerQuery) ||
                        b.Body.ToLower().Contains(lowerQuery))
            .Select(b => new BlogPostDto
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
            });

        return new SearchResultDto
        {
            Query = query,
            Members = memberDtos,
            Projects = projectDtos,
            Articles = articleDtos
        };
    }
}
