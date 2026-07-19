namespace TeamPortfolio.Application.DTOs;

public class TeamMemberDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Role { get; set; } = "";
    public string Slug { get; set; } = "";
    public string? Biography { get; set; }
    public string? ProfilePhotoPath { get; set; }
    public bool IsActive { get; set; }
}
