using System.ComponentModel.DataAnnotations;

namespace TeamPortfolio.Web.ViewModels.Account;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "ایمیل الزامی است")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل نامعتبر است")]
    public string Email { get; set; } = "";
}
