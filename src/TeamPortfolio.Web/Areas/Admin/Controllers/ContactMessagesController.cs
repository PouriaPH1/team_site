using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamPortfolio.Application.Interfaces.Repositories;

namespace TeamPortfolio.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class ContactMessagesController : Controller
{
    private readonly IContactMessageRepository? _repo;
    private readonly ILogger<ContactMessagesController> _logger;

    public ContactMessagesController(ILogger<ContactMessagesController> logger, IContactMessageRepository? repo = null)
    { _logger = logger; _repo = repo; }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Contact Messages";
        IEnumerable<TeamPortfolio.Domain.Entities.ContactMessage> messages = [];
        if (_repo is not null)
            try { messages = (await _repo.GetAllAsync()).OrderByDescending(m => m.CreatedAt); }
            catch (Exception ex) { _logger.LogWarning(ex, ""); }
        return View(messages);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkRead(int id)
    {
        if (_repo is not null)
            try { var msg = await _repo.GetByIdAsync(id); if (msg is not null) { msg.IsRead = true; await _repo.UpdateAsync(msg); } }
            catch (Exception ex) { _logger.LogWarning(ex, ""); }
        return RedirectToAction("Index");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (_repo is not null)
            try { var msg = await _repo.GetByIdAsync(id); if (msg is not null) await _repo.DeleteAsync(msg); }
            catch (Exception ex) { _logger.LogWarning(ex, ""); }
        TempData["Success"] = "Message deleted.";
        return RedirectToAction("Index");
    }
}
