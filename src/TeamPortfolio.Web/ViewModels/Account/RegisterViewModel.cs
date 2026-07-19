using System.ComponentModel.DataAnnotations;

namespace TeamPortfolio.Web.ViewModels.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "ایمیل الزامی است")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل نامعتبر است")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [MinLength(8, ErrorMessage = "رمز عبور باید حداقل 8 کاراکتر باشد")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن مطابقت ندارند")]
    public string ConfirmPassword { get; set; } = "";
}
