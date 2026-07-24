using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Application.Interfaces.Services;
using TeamPortfolio.Application.Services;
using TeamPortfolio.Infrastructure.Data;
using TeamPortfolio.Infrastructure.Identity;
using TeamPortfolio.Infrastructure.Repositories;
using TeamPortfolio.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// EF Core — SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity with password policy (Req 9.1, 9.3)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.SignIn.RequireConfirmedEmail = false; // برای محیط dev
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Cookie settings for Remember Me (Req 9.7)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Error/403";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});

// Authorization policies (Req 20.4)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
    options.AddPolicy("AdminOrManager", policy =>
        policy.RequireRole("Admin", "Manager"));
    options.AddPolicy("AdminManagerOrAuthor", policy =>
        policy.RequireRole("Admin", "Manager", "Author"));
    options.AddPolicy("MemberAndAbove", policy =>
        policy.RequireRole("Admin", "Manager", "Author", "Member"));
});

// Memory cache
builder.Services.AddMemoryCache();

// Antiforgery
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

// === Application Services ===
builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<ISeoService, SeoService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileUploadService>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    var uploadBasePath = Path.Combine(env.WebRootPath, "uploads");
    return new FileUploadService(uploadBasePath);
});

// === Repositories ===
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/500");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

// Areas routing
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// ── Fix empty slugs and unpublished items on startup ─────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TeamPortfolio.Infrastructure.Data.ApplicationDbContext>();
    var slugHelper = new Slugify.SlugHelper();
    var items = db.PortfolioItems.ToList();
    bool changed = false;
    foreach (var item in items)
    {
        if (string.IsNullOrWhiteSpace(item.Slug))
        {
            item.Slug = slugHelper.GenerateSlug(item.Title + "-" + item.Id);
            changed = true;
        }
        if (!item.IsPublished)
        {
            item.IsPublished = true;
            changed = true;
        }
        // Fix legacy image paths that were stored without /uploads/ prefix
        if (!string.IsNullOrEmpty(item.CoverImagePath) &&
            !item.CoverImagePath.StartsWith("/uploads/") &&
            !item.CoverImagePath.StartsWith("http"))
        {
            item.CoverImagePath = "/uploads" + item.CoverImagePath;
            changed = true;
        }
    }
    
    // Fix legacy paths in PortfolioImages gallery too
    var images = db.PortfolioImages.ToList();
    foreach (var img in images)
    {
        if (!string.IsNullOrEmpty(img.ImagePath) &&
            !img.ImagePath.StartsWith("/uploads/") &&
            !img.ImagePath.StartsWith("http"))
        {
            img.ImagePath = "/uploads" + img.ImagePath;
            changed = true;
        }
    }
    
    // Fix legacy paths in TeamMembers too
    var members = db.TeamMembers.ToList();
    foreach (var member in members)
    {
        if (!string.IsNullOrEmpty(member.ProfilePhotoPath) &&
            !member.ProfilePhotoPath.StartsWith("/uploads/") &&
            !member.ProfilePhotoPath.StartsWith("http"))
        {
            var bare = member.ProfilePhotoPath.TrimStart('/');
            member.ProfilePhotoPath = $"/uploads/profiles/{bare}";
            changed = true;
        }
        if (!string.IsNullOrEmpty(member.BannerPhotoPath) &&
            !member.BannerPhotoPath.StartsWith("/uploads/") &&
            !member.BannerPhotoPath.StartsWith("http"))
        {
            var bare = member.BannerPhotoPath.TrimStart('/');
            member.BannerPhotoPath = $"/uploads/profiles/{bare}";
            changed = true;
        }
    }
    
    if (changed) await db.SaveChangesAsync();
}
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    const string adminEmail = "admin@teamportfolio.dev";
    const string adminPassword = "Admin@1234";

    if (await userManager.FindByEmailAsync(adminEmail) is null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var createResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (createResult.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

app.Run();
