using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TeamPortfolio.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager,Author,Member")]
public class ProfileController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "My Profile";
        return View();
    }
}
