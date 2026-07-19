using System.ComponentModel.DataAnnotations;
using TeamPortfolio.Application.DTOs;

namespace TeamPortfolio.Web.ViewModels.Blog;

public class BlogPostViewModel
{
    public BlogPostDto Post { get; set; } = new();
    public IEnumerable<CommentDto> Comments { get; set; } = [];
    public IEnumerable<BlogPostDto> RelatedPosts { get; set; } = [];
    public SubmitCommentViewModel CommentForm { get; set; } = new();
}

public class SubmitCommentViewModel
{
    [Required(ErrorMessage = "Name is required")]
    public string CommenterName { get; set; } = "";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string CommenterEmail { get; set; } = "";

    [Required(ErrorMessage = "Comment body is required")]
    [MinLength(3)]
    public string Body { get; set; } = "";

    public int PostId { get; set; }
}
