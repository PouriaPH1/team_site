using System.ComponentModel.DataAnnotations;

namespace TeamPortfolio.Web.ViewModels.Contact;

public class ContactViewModel
{
    [Required(ErrorMessage = "Full name is required")]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Subject is required")]
    public string Subject { get; set; } = "";

    [Required(ErrorMessage = "Message is required")]
    public string Body { get; set; } = "";
}
