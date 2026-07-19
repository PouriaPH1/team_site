using Microsoft.AspNetCore.Mvc;
using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Interfaces.Services;
using TeamPortfolio.Web.ViewModels.Blog;

namespace TeamPortfolio.Web.Controllers;

public class BlogController : Controller
{
    private readonly IBlogService? _blogService;
    private readonly ICommentService? _commentService;
    private readonly ILogger<BlogController> _logger;
    private const int PageSize = 10;

    public BlogController(
        ILogger<BlogController> logger,
        IBlogService? blogService = null,
        ICommentService? commentService = null)
    {
        _logger = logger;
        _blogService = blogService;
        _commentService = commentService;
    }

    public async Task<IActionResult> Index(int page = 1, int? categoryId = null)
    {
        ViewData["Title"] = "Blog";
        ViewData["BreadcrumbItems"] = new List<(string, string?)> { ("Blog", null) };

        var vm = new BlogIndexViewModel { Page = page, ActiveCategoryId = categoryId };

        if (_blogService is not null)
        {
            try
            {
                var result = await _blogService.GetPublishedAsync(page, PageSize, categoryId);
                vm.Posts = result.Items;
                vm.TotalPages = result.TotalPages;
                vm.HasPreviousPage = result.HasPreviousPage;
                vm.HasNextPage = result.HasNextPage;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to load blog posts");
            }
        }

        return View(vm);
    }

    [Route("Blog/{slug}")]
    public async Task<IActionResult> Post(string slug)
    {
        BlogPostDto? post = null;

        if (_blogService is not null)
        {
            try { post = await _blogService.GetBySlugAsync(slug); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to load blog post {Slug}", slug); }
        }

        if (post is null) return NotFound();

        // Increment view count
        if (_blogService is not null)
        {
            try { await _blogService.IncrementViewCountAsync(post.Id); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to increment view count for post {Id}", post.Id); }
        }

        var comments = Enumerable.Empty<CommentDto>();
        if (_commentService is not null)
        {
            try { comments = await _commentService.GetApprovedForPostAsync(post.Id); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to load comments for post {Id}", post.Id); }
        }

        var related = Enumerable.Empty<BlogPostDto>();
        if (_blogService is not null)
        {
            try { related = await _blogService.GetRelatedAsync(post.Id, 3); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to load related posts for {Id}", post.Id); }
        }

        ViewData["Title"] = post.Title;
        ViewData["BreadcrumbItems"] = new List<(string, string?)>
        {
            ("Blog", "/Blog"),
            (post.Title, null)
        };

        return View(new BlogPostViewModel
        {
            Post = post,
            Comments = comments,
            RelatedPosts = related,
            CommentForm = new SubmitCommentViewModel { PostId = post.Id }
        });
    }

    [HttpPost("Blog/{slug}/comment")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitComment(string slug, SubmitCommentViewModel form)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Post", new { slug });
        }

        if (_commentService is not null)
        {
            try
            {
                await _commentService.SubmitAsync(new CommentDto
                {
                    CommenterName = form.CommenterName,
                    CommenterEmail = form.CommenterEmail,
                    Body = form.Body,
                    PostId = form.PostId
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to submit comment for post {Id}", form.PostId);
            }
        }

        TempData["CommentSuccess"] = "Your comment has been submitted and is pending approval.";
        return RedirectToAction("Post", new { slug });
    }
}
