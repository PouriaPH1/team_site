using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Interfaces.Services;
using TeamPortfolio.Web.ViewModels.Admin;

namespace TeamPortfolio.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class PortfolioController : Controller
{
    private readonly IPortfolioService? _service;
    private readonly IFileUploadService? _fileUpload;
    private readonly ICacheService? _cache;
    private readonly ILogger<PortfolioController> _logger;

    public PortfolioController(ILogger<PortfolioController> logger,
        IPortfolioService? service = null,
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
        ViewData["Title"] = "Portfolio";
        IEnumerable<PortfolioItemDto> items = [];
        if (_service is not null)
            try { items = await _service.GetAllAsync(); }  // همه (published + draft)
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to load portfolio items"); }
        return View(items);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Add Project";
        return View(new PortfolioFormViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PortfolioFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = Map(model);

        if (model.CoverImage is not null && _fileUpload is not null)
        {
            if (!_fileUpload.IsValidImageFile(model.CoverImage))
            {
                ModelState.AddModelError("CoverImage", "Only JPEG, PNG or WebP up to 5 MB");
                return View(model);
            }
            var r = await _fileUpload.UploadImageAsync(model.CoverImage, "portfolio");
            if (r.Success) dto.CoverImagePath = r.FilePath ?? "";
        }

        if (_service is not null)
            try { await _service.CreateAsync(dto); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to create portfolio item"); }

        if (dto.IsPublished && _cache is not null)
            try { await _cache.RemoveAsync("home_page_data"); } catch { }

        TempData["Success"] = "Project created.";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Edit Project";
        PortfolioItemDto? item = null;
        if (_service is not null)
            try { item = (await _service.GetAllAsync()).FirstOrDefault(x => x.Id == id); } catch { }
        if (item is null) return NotFound();

        return View(new PortfolioFormViewModel
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Technologies = item.Technologies,
            StartDate = item.StartDate,
            GitHubUrl = item.GitHubUrl,
            DemoUrl = item.DemoUrl,
            CoverImagePath = item.CoverImagePath
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PortfolioFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = Map(model);

        if (model.CoverImage is not null && _fileUpload is not null)
        {
            if (!_fileUpload.IsValidImageFile(model.CoverImage))
            {
                ModelState.AddModelError("CoverImage", "Only JPEG, PNG or WebP up to 5 MB");
                return View(model);
            }
            var r = await _fileUpload.UploadImageAsync(model.CoverImage, "portfolio");
            if (r.Success) dto.CoverImagePath = r.FilePath ?? "";
        }

        if (_service is not null)
            try { await _service.UpdateAsync(id, dto); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to update portfolio item {Id}", id); }

        if (_cache is not null)
            try { await _cache.RemoveAsync("home_page_data"); } catch { }

        TempData["Success"] = "Project updated.";
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (_service is not null)
            try { await _service.DeleteAsync(id); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to delete portfolio item {Id}", id); }

        if (_cache is not null)
            try { await _cache.RemoveAsync("home_page_data"); } catch { }

        TempData["Success"] = "Project deleted.";
        return RedirectToAction("Index");
    }

    private static PortfolioItemDto Map(PortfolioFormViewModel m) => new()
    {
        Id = m.Id,
        Title = m.Title,
        Description = m.Description,
        Technologies = m.Technologies,
        StartDate = m.StartDate,
        GitHubUrl = m.GitHubUrl,
        DemoUrl = m.DemoUrl,
        CoverImagePath = m.CoverImagePath ?? "",
        IsPublished = true
    };
}
