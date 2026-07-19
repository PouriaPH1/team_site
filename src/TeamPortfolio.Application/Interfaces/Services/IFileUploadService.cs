using Microsoft.AspNetCore.Http;

namespace TeamPortfolio.Application.Interfaces.Services;

public interface IFileUploadService
{
    Task<FileUploadResult> UploadImageAsync(IFormFile file, string folder);
    Task DeleteAsync(string filePath);
    bool IsValidImageFile(IFormFile file);
}

public record FileUploadResult(bool Success, string? FilePath, string? ErrorMessage);
