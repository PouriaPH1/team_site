namespace TeamPortfolio.Application.Interfaces.Services;

public interface ISeoService
{
    string GenerateTitle(string pageTitle);
    string GenerateMetaDescription(string content, int maxLength = 160);
    Dictionary<string, string> GenerateOpenGraphTags(string title, string description, string? imageUrl);
}
