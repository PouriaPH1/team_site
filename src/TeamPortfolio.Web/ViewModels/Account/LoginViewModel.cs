using System.ComponentModel.DataAnnotations;

namespace TeamPortfolio.Web.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "ایمیل الزامی است")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل نامعتبر است")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; }
}
