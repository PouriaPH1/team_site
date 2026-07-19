using TeamPortfolio.Domain.Common;

namespace TeamPortfolio.Domain.Entities;

public class TeamMember : BaseEntity
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string FullName => $"{FirstName} {LastName}";
    public string Role { get; set; } = "";
    public string Slug { get; set; } = "";
    public string? Biography { get; set; }
    public string? ProfilePhotoPath { get; set; }
    public string? BannerPhotoPath { get; set; }
    public string? ResumeFilePath { get; set; }
    public bool IsActive { get; set; } = true;

    // Social Links
    public string? GitHubUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? TelegramUrl { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    // ASP.NET Identity FK
    public string? ApplicationUserId { get; set; }

    // Navigation
    public ICollection<Skill> Skills { get; set; } = [];
    public ICollection<WorkExperience> WorkExperiences { get; set; } = [];
    public ICollection<Education> Educations { get; set; } = [];
    public ICollection<BlogPost> BlogPosts { get; set; } = [];
    public ICollection<PortfolioItemMember> PortfolioItemMembers { get; set; } = [];
}
