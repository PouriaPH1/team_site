using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamPortfolio.Domain.Entities;
using TeamPortfolio.Infrastructure.Identity;

namespace TeamPortfolio.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<WorkExperience> WorkExperiences => Set<WorkExperience>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<PortfolioItem> PortfolioItems => Set<PortfolioItem>();
    public DbSet<PortfolioImage> PortfolioImages => Set<PortfolioImage>();
    public DbSet<PortfolioItemMember> PortfolioItemMembers => Set<PortfolioItemMember>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<BlogPostTag> BlogPostTags => Set<BlogPostTag>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure many-to-many: PortfolioItem <-> TeamMember
        modelBuilder.Entity<PortfolioItemMember>()
            .HasKey(pim => new { pim.PortfolioItemId, pim.TeamMemberId });

        modelBuilder.Entity<PortfolioItemMember>()
            .HasOne(pim => pim.PortfolioItem)
            .WithMany(p => p.Members)
            .HasForeignKey(pim => pim.PortfolioItemId);

        modelBuilder.Entity<PortfolioItemMember>()
            .HasOne(pim => pim.TeamMember)
            .WithMany(tm => tm.PortfolioItemMembers)
            .HasForeignKey(pim => pim.TeamMemberId);

        // Configure many-to-many: BlogPost <-> Tag
        modelBuilder.Entity<BlogPostTag>()
            .HasKey(bpt => new { bpt.BlogPostId, bpt.TagId });

        modelBuilder.Entity<BlogPostTag>()
            .HasOne(bpt => bpt.BlogPost)
            .WithMany(bp => bp.BlogPostTags)
            .HasForeignKey(bpt => bpt.BlogPostId);

        modelBuilder.Entity<BlogPostTag>()
            .HasOne(bpt => bpt.Tag)
            .WithMany(t => t.BlogPostTags)
            .HasForeignKey(bpt => bpt.TagId);

        // Seed default roles
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>().HasData(
            new Microsoft.AspNetCore.Identity.IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
            },
            new Microsoft.AspNetCore.Identity.IdentityRole
            {
                Id = "2",
                Name = "Manager",
                NormalizedName = "MANAGER",
                ConcurrencyStamp = "b2c3d4e5-f6a7-8901-bcde-f01234567891"
            },
            new Microsoft.AspNetCore.Identity.IdentityRole
            {
                Id = "3",
                Name = "Author",
                NormalizedName = "AUTHOR",
                ConcurrencyStamp = "c3d4e5f6-a7b8-9012-cdef-012345678912"
            },
            new Microsoft.AspNetCore.Identity.IdentityRole
            {
                Id = "4",
                Name = "Member",
                NormalizedName = "MEMBER",
                ConcurrencyStamp = "d4e5f6a7-b8c9-0123-defa-123456789023"
            }
        );
    }
}
