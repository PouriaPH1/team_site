using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TeamPortfolio.Web.Controllers;

public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger) { _logger = logger; }

    [Route("Error/{statusCode}")]
    public IActionResult Index(int statusCode)
    {
        ViewData["HideBreadcrumb"] = true;
        return statusCode switch
        {
            404 => View("NotFound"),
            403 => View("Forbidden"),
            _ => View("ServerError")
        };
    }

    [Route("Error/500")]
    public IActionResult ServerError()
    {
        var exFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exFeature?.Error is not null)
            _logger.LogError(exFeature.Error, "Unhandled exception at {Path}", exFeature.Path);
        ViewData["HideBreadcrumb"] = true;
        return View("ServerError");
    }
}
