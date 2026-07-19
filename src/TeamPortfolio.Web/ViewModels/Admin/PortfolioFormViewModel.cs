using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TeamPortfolio.Web.ViewModels.Admin;

public class PortfolioFormViewModel
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = "";

    [Required]
    public string Description { get; set; } = "";

    [Required]
    public string Technologies { get; set; } = "";

    public DateTime StartDate { get; set; } = DateTime.Today;
    public string? GitHubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string? CoverImagePath { get; set; }
    public IFormFile? CoverImage { get; set; }
}
