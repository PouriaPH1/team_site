using Microsoft.AspNetCore.Mvc;
using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Services;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Web.ViewModels.Contact;

namespace TeamPortfolio.Web.Controllers;

public class ContactController : Controller
{
    private readonly ILogger<ContactController> _logger;
    private readonly IContactMessageRepository? _contactRepo;

    public ContactController(ILogger<ContactController> logger, IContactMessageRepository? contactRepo = null)
    {
        _logger = logger;
        _contactRepo = contactRepo;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Contact";
        ViewData["BreadcrumbItems"] = new List<(string, string?)> { ("Contact", null) };
        return View(new ContactViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(ContactViewModel model)
    {
        ViewData["Title"] = "Contact";
        ViewData["BreadcrumbItems"] = new List<(string, string?)> { ("Contact", null) };

        if (!ModelState.IsValid)
            return View("Index", model);

        if (_contactRepo is not null)
        {
            try
            {
                var service = new ContactService(_contactRepo);
                await service.SubmitContactAsync(new ContactMessageDto
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Subject = model.Subject,
                    Body = model.Body
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to submit contact message");
            }
        }

        TempData["ContactSuccess"] = "Thank you! Your message has been sent. We'll get back to you soon.";
        return RedirectToAction("Index");
    }
}
