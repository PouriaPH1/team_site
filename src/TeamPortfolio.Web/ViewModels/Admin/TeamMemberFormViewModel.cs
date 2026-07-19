using System.ComponentModel.DataAnnotations;

namespace TeamPortfolio.Web.ViewModels.Admin;

public class TeamMemberFormViewModel
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = "";

    [Required]
    public string LastName { get; set; } = "";

    [Required]
    public string Role { get; set; } = "";

    public string? Biography { get; set; }
    public string? ProfilePhotoPath { get; set; }
    public bool IsActive { get; set; } = true;
    public Microsoft.AspNetCore.Http.IFormFile? ProfilePhoto { get; set; }
}
