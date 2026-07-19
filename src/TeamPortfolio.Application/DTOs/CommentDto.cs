namespace TeamPortfolio.Application.DTOs;

public class CommentDto
{
    public int Id { get; set; }
    public string CommenterName { get; set; } = "";
    public string CommenterEmail { get; set; } = "";
    public string Body { get; set; } = "";
    public string Status { get; set; } = "";
    public int PostId { get; set; }
    public DateTime CreatedAt { get; set; }
}
