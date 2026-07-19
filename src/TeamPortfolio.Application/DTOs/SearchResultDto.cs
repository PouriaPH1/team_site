namespace TeamPortfolio.Application.DTOs;

public class SearchResultDto
{
    public string Query { get; set; } = "";
    public IEnumerable<TeamMemberDto> Members { get; set; } = [];
    public IEnumerable<PortfolioItemDto> Projects { get; set; } = [];
    public IEnumerable<BlogPostDto> Articles { get; set; } = [];
    public int TotalCount => Members.Count() + Projects.Count() + Articles.Count();
}
