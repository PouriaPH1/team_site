using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TeamPortfolio.Web.ViewModels.Admin;

public class BlogPostFormViewModel
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = "";

    [Required]
    public string Body { get; set; } = "";

    public string? CoverImagePath { get; set; }
    public IFormFile? CoverImage { get; set; }
    public string CategoryName { get; set; } = "";
}
