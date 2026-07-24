using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Interfaces.Services;
using TeamPortfolio.Web.ViewModels.Admin;

namespace TeamPortfolio.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager,Author")]
public class BlogController : Controller
{
    private readonly IBlogService? _service;
    private readonly IFileUploadService? _fileUpload;
    private readonly ICacheService? _cache;
    private readonly ILogger<BlogController> _logger;

    public BlogController(ILogger<BlogController> logger,
        IBlogService? service = null,
        IFileUploadService? fileUpload = null,
        ICacheService? cache = null)
    {
        _logger = logger;
        _service = service;
        _fileUpload = fileUpload;
        _cache = cache;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Blog Posts";
        IEnumerable<BlogPostDto> posts = [];
        if (_service is not null)
            try
            {
                var r = await _service.GetPublishedAsync(1, 50);
                posts = r.Items;
            }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to load blog posts"); }
        return View(posts);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Write Post";
        return View(new BlogPostFormViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BlogPostFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new BlogPostDto
        {
            Title = model.Title,
            Body = model.Body,
            Status = "Published",
            CategoryName = model.CategoryName
        };

        if (model.CoverImage is not null && _fileUpload is not null)
        {
            if (!_fileUpload.IsValidImageFile(model.CoverImage))
            {
                ModelState.AddModelError("CoverImage", "Only JPEG, PNG or WebP up to 5 MB");
                return View(model);
            }
            var r = await _fileUpload.UploadImageAsync(model.CoverImage, "blog");
            if (r.Success) dto.CoverImagePath = r.FilePath;
        }

        if (_service is not null)
            try
            {
                var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                await _service.CreateAsync(dto, applicationUserId);
                if (_cache is not null)
                    try { await _cache.RemoveAsync("home_page_data"); } catch { }
                TempData["Success"] = "Post created.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to create blog post");
                ModelState.AddModelError("", ex.Message.Contains("No TeamMember")
                    ? ex.Message
                    : "Failed to save post. Please try again.");
                return View(model);
            }

        TempData["Success"] = "Post created.";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Edit Post";
        BlogPostDto? post = null;
        if (_service is not null)
            try
            {
                var r = await _service.GetPublishedAsync(1, 200);
                post = r.Items.FirstOrDefault(x => x.Id == id);
            }
            catch { }
        if (post is null) return NotFound();

        return View(new BlogPostFormViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            CoverImagePath = post.CoverImagePath,
            CategoryName = post.CategoryName
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BlogPostFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new BlogPostDto
        {
            Id = id,
            Title = model.Title,
            Body = model.Body,
            Status = "Published",
            CoverImagePath = model.CoverImagePath,
            CategoryName = model.CategoryName
        };

        if (model.CoverImage is not null && _fileUpload is not null)
        {
            if (!_fileUpload.IsValidImageFile(model.CoverImage))
            {
                ModelState.AddModelError("CoverImage", "Only JPEG, PNG or WebP up to 5 MB");
                return View(model);
            }
            var r = await _fileUpload.UploadImageAsync(model.CoverImage, "blog");
            if (r.Success) dto.CoverImagePath = r.FilePath;
        }

        if (_service is not null)
            try
            {
                var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                var isAdmin = User.IsInRole("Admin") || User.IsInRole("Manager");
                await _service.UpdateAsync(id, dto, applicationUserId, isAdmin);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to update blog post {Id}", id);
                ModelState.AddModelError("", "Failed to update post. Please try again.");
                return View(model);
            }

        if (_cache is not null)
            try { await _cache.RemoveAsync("home_page_data"); } catch { }

        TempData["Success"] = "Post updated.";
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var isAdmin = User.IsInRole("Admin") || User.IsInRole("Manager");
        if (_service is not null)
            try { await _service.DeleteAsync(id, User.Identity?.Name ?? "", isAdmin); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to delete blog post {Id}", id); }

        if (_cache is not null)
            try { await _cache.RemoveAsync("home_page_data"); } catch { }

        TempData["Success"] = "Post deleted.";
        return RedirectToAction("Index");
    }
}
