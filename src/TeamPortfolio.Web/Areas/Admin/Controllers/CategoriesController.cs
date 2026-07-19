using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Domain.Entities;

namespace TeamPortfolio.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class CategoriesController : Controller
{
    private readonly ICategoryRepository? _repo;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ILogger<CategoriesController> logger, ICategoryRepository? repo = null)
    { _logger = logger; _repo = repo; }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Categories";
        IEnumerable<Category> cats = [];
        if (_repo is not null) try { cats = await _repo.GetAllAsync(); } catch (Exception ex) { _logger.LogWarning(ex, ""); }
        return View(cats);
    }

    public IActionResult Create() { ViewData["Title"] = "Add Category"; return View(); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name)) { TempData["Error"] = "Name is required."; return View(); }
        if (_repo is not null) try { await _repo.AddAsync(new Category { Name = name.Trim(), Description = description?.Trim() }); } catch (Exception ex) { _logger.LogWarning(ex, ""); }
        TempData["Success"] = "Category created.";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Edit Category";
        Category? cat = null;
        if (_repo is not null) try { cat = await _repo.GetByIdAsync(id); } catch { }
        if (cat is null) return NotFound();
        return View(cat);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name)) { TempData["Error"] = "Name required."; return RedirectToAction("Edit", new { id }); }
        if (_repo is not null)
        {
            try
            {
                var cat = await _repo.GetByIdAsync(id);
                if (cat is not null) { cat.Name = name.Trim(); cat.Description = description?.Trim(); await _repo.UpdateAsync(cat); }
            }
            catch (Exception ex) { _logger.LogWarning(ex, ""); }
        }
        TempData["Success"] = "Category updated.";
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (_repo is not null)
        {
            try
            {
                var count = await _repo.GetPostCountByCategoryAsync(id);
                if (count > 0) { TempData["Error"] = $"Cannot delete: {count} blog post(s) use this category."; return RedirectToAction("Index"); }
                var cat = await _repo.GetByIdAsync(id);
                if (cat is not null) await _repo.DeleteAsync(cat);
            }
            catch (Exception ex) { _logger.LogWarning(ex, ""); }
        }
        TempData["Success"] = "Category deleted.";
        return RedirectToAction("Index");
    }
}
