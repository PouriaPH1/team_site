using Microsoft.AspNetCore.Mvc;
namespace TeamPortfolio.Web.Controllers;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "About Us";
        ViewData["BreadcrumbItems"] = new List<(string, string?)> { ("About", null) };
        return View();
    }
}
