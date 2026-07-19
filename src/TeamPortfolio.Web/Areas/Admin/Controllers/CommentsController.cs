using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamPortfolio.Application.Interfaces.Services;

namespace TeamPortfolio.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class CommentsController : Controller
{
    private readonly ICommentService? _service;
    private readonly ILogger<CommentsController> _logger;

    public CommentsController(ILogger<CommentsController> logger, ICommentService? service = null)
    { _logger = logger; _service = service; }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Comments";
        var comments = Enumerable.Empty<TeamPortfolio.Application.DTOs.CommentDto>();
        if (_service is not null)
            try { comments = await _service.GetPendingAsync(); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to load pending comments"); }
        return View(comments);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        if (_service is not null)
            try { await _service.ApproveAsync(id); } catch (Exception ex) { _logger.LogWarning(ex, "Approve failed"); }
        TempData["Success"] = "Comment approved.";
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (_service is not null)
            try { await _service.DeleteAsync(id); } catch (Exception ex) { _logger.LogWarning(ex, "Delete failed"); }
        TempData["Success"] = "Comment deleted.";
        return RedirectToAction("Index");
    }
}
