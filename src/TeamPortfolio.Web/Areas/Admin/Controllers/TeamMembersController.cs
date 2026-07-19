using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Interfaces.Services;
using TeamPortfolio.Web.ViewModels.Admin;

namespace TeamPortfolio.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class TeamMembersController : Controller
{
    private readonly ITeamMemberService? _service;
    private readonly IFileUploadService? _fileUpload;
    private readonly ILogger<TeamMembersController> _logger;

    public TeamMembersController(ILogger<TeamMembersController> logger,
        ITeamMemberService? service = null, IFileUploadService? fileUpload = null)
    {
        _logger = logger;
        _service = service;
        _fileUpload = fileUpload;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Team Members";
        IEnumerable<TeamMemberDto> members = [];
        if (_service is not null)
            try { members = await _service.GetAllActiveAsync(); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to load members"); }
        return View(members);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Add Member";
        return View(new TeamMemberFormViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TeamMemberFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new TeamMemberDto
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Role = model.Role,
            Biography = model.Biography,
            IsActive = model.IsActive
        };

        if (model.ProfilePhoto is not null && _fileUpload is not null)
        {
            if (!_fileUpload.IsValidImageFile(model.ProfilePhoto))
            {
                ModelState.AddModelError("ProfilePhoto", "Only JPEG, PNG or WebP up to 5 MB");
                return View(model);
            }
            var r = await _fileUpload.UploadImageAsync(model.ProfilePhoto, "profiles");
            if (r.Success) dto.ProfilePhotoPath = r.FilePath;
        }

        if (_service is not null)
            try { await _service.CreateAsync(dto); }
            catch (Exception ex) { _logger.LogWarning(ex, "Create failed"); }

        TempData["Success"] = "Team member created.";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Edit Member";
        TeamMemberDto? m = null;
        if (_service is not null)
            try { m = await _service.GetByIdAsync(id); } catch { }
        if (m is null) return NotFound();

        return View(new TeamMemberFormViewModel
        {
            Id = m.Id,
            FirstName = m.FirstName,
            LastName = m.LastName,
            Role = m.Role,
            Biography = m.Biography,
            ProfilePhotoPath = m.ProfilePhotoPath,
            IsActive = m.IsActive
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TeamMemberFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = new TeamMemberDto
        {
            Id = id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Role = model.Role,
            Biography = model.Biography,
            IsActive = model.IsActive,
            ProfilePhotoPath = model.ProfilePhotoPath
        };

        if (model.ProfilePhoto is not null && _fileUpload is not null)
        {
            if (!_fileUpload.IsValidImageFile(model.ProfilePhoto))
            {
                ModelState.AddModelError("ProfilePhoto", "Only JPEG, PNG or WebP up to 5 MB");
                return View(model);
            }
            var r = await _fileUpload.UploadImageAsync(model.ProfilePhoto, "profiles");
            if (r.Success) dto.ProfilePhotoPath = r.FilePath;
        }

        if (_service is not null)
            try { await _service.UpdateAsync(id, dto); }
            catch (Exception ex) { _logger.LogWarning(ex, "Update failed"); }

        TempData["Success"] = "Team member updated.";
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (_service is not null)
            try { await _service.DeleteAsync(id); }
            catch (Exception ex) { _logger.LogWarning(ex, "Delete failed"); }

        TempData["Success"] = "Team member deleted.";
        return RedirectToAction("Index");
    }
}
