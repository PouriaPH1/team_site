using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using TeamPortfolio.Application.Interfaces.Services;

namespace TeamPortfolio.Infrastructure.Services;

public class FileUploadService : IFileUploadService
{
    private static readonly HashSet<string> AllowedMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg", "image/png", "image/webp"
    };

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };

    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
    private readonly string _uploadBasePath;

    public FileUploadService(string uploadBasePath)
    {
        _uploadBasePath = uploadBasePath;
    }

    public bool IsValidImageFile(IFormFile file)
    {
        if (file == null || file.Length == 0) return false;
        if (file.Length > MaxFileSizeBytes) return false;

        var extension = Path.GetExtension(file.FileName);
        if (!AllowedExtensions.Contains(extension)) return false;

        if (!AllowedMimeTypes.Contains(file.ContentType)) return false;

        return true;
    }

    public async Task<FileUploadResult> UploadImageAsync(IFormFile file, string folder)
    {
        if (!IsValidImageFile(file))
            return new FileUploadResult(false, null, "فایل نامعتبر است. فقط JPEG، PNG یا WebP با حداکثر 5 MB مجاز است.");

        try
        {
            var uploadsDir = Path.Combine(_uploadBasePath, folder);
            Directory.CreateDirectory(uploadsDir);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using var stream = file.OpenReadStream();
            using var image = await Image.LoadAsync(stream);
            await image.SaveAsync(filePath);

            var relativePath = $"/uploads/{folder}/{fileName}";
            return new FileUploadResult(true, relativePath, null);
        }
        catch (Exception ex)
        {
            return new FileUploadResult(false, null, $"خطا در آپلود فایل: {ex.Message}");
        }
    }

    public async Task DeleteAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return;

        var fullPath = Path.Combine(_uploadBasePath, filePath.TrimStart('/'));
        if (File.Exists(fullPath))
        {
            await Task.Run(() => File.Delete(fullPath));
        }
    }
}
