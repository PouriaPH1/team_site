# Design Document: Team Portfolio Website

## Overview

این سند طراحی فنی برای وب‌سایت Portfolio تیم برنامه‌نویسی است که با ASP.NET Core 9 MVC پیاده‌سازی می‌شود. معماری انتخاب‌شده Clean Architecture با Repository Pattern و Service Layer است که تفکیک وابستگی‌ها، تست‌پذیری و نگهداری‌پذیری بلندمدت را تضمین می‌کند.

### اهداف طراحی

- **تفکیک دغدغه‌ها**: هر لایه مسئولیت مشخص دارد و به لایه‌های پایین‌تر وابسته نیست
- **تست‌پذیری**: همه منطق کسب‌وکار از وابستگی‌های زیرساخت جدا است
- **مقیاس‌پذیری**: ساختار ماژولار امکان افزودن ویژگی‌های جدید را بدون تغییر در کد موجود می‌دهد
- **امنیت**: احراز هویت و مجوزدهی مبتنی بر نقش با ASP.NET Identity
- **عملکرد**: کش‌گذاری هوشمند، Lazy Loading و Pagination برای بارگذاری سریع

### Stack فنی

| لایه | تکنولوژی |
|------|-----------|
| Backend Framework | ASP.NET Core 9 MVC |
| ORM | Entity Framework Core 9 |
| Database | SQL Server 2022 |
| Authentication | ASP.NET Core Identity |
| Frontend CSS | Bootstrap 5.3 |
| Frontend JS | Vanilla JS + jQuery 3.7 |
| Icons | Font Awesome 6 (فقط SVG/class — بدون emoji) |
| Fonts | Space Grotesk (heading) + DM Sans (body) — Google Fonts |
| Rich Text Editor | TinyMCE یا CKEditor 5 |
| Image Processing | SixLabors.ImageSharp |
| Caching | IMemoryCache (In-Memory) |
| Email | MailKit / SMTP |
| Logging | Serilog |
| Slug Generation | Slugify-Net |
| HTML Sanitizer | HtmlSanitizer (XSS prevention) |

---

## UI/UX Design System

> **منبع:** `design-system/team-portfolio/MASTER.md`
> هنگام پیاده‌سازی هر صفحه، ابتدا `design-system/pages/[page].md` را بررسی کن؛ اگر وجود داشت آن override می‌کند، وگرنه از MASTER.md پیروی کن.

### سبک بصری

- **الگو:** Portfolio Grid + Dark Mode Professional
- **الهام بصری:** Vercel، Linear، GitHub، Stripe، Microsoft Developer
- **رویکرد:** Mobile-First، Clean، Minimal، Professional

### رنگ‌بندی

#### Dark Mode (پیش‌فرض)

| نقش | Hex | CSS Variable |
|-----|-----|--------------|
| Background | `#0A0F1E` | `--color-bg` |
| Surface (کارت‌ها) | `#111827` | `--color-surface` |
| Surface Raised | `#1A2235` | `--color-surface-raised` |
| Border | `#1E2D40` | `--color-border` |
| Primary (آبی) | `#3B82F6` | `--color-primary` |
| Accent (بنفش) | `#8B5CF6` | `--color-accent` |
| Gradient | `#3B82F6 → #8B5CF6` | `--gradient-primary` |
| Text | `#F9FAFB` | `--color-text` |
| Text Secondary | `#9CA3AF` | `--color-text-secondary` |

#### Light Mode

| نقش | Hex | CSS Variable |
|-----|-----|--------------|
| Background | `#F8FAFC` | `--color-bg` |
| Surface | `#FFFFFF` | `--color-surface` |
| Border | `#E2E8F0` | `--color-border` |
| Primary | `#2563EB` | `--color-primary` |
| Accent | `#7C3AED` | `--color-accent` |
| Text | `#0F172A` | `--color-text` |
| Text Secondary | `#475569` | `--color-text-secondary` (حداقل — هرگز روشن‌تر نشود) |

### تایپوگرافی

```css
@import url('https://fonts.googleapis.com/css2?family=Space+Grotesk:wght@300;400;500;600;700&family=DM+Sans:ital,opsz,wght@0,9..40,400;0,9..40,500;0,9..40,700&display=swap');

--font-heading: 'Space Grotesk', sans-serif;  /* H1–H3 */
--font-body:    'DM Sans', sans-serif;         /* body, buttons, labels */
--font-mono:    'JetBrains Mono', monospace;   /* code blocks */
```

### انیمیشن و ترنزیشن

| المان | افکت | مدت |
|-------|------|-----|
| دکمه‌ها | `translateY(-1px)` + opacity | 200ms |
| کارت‌ها | `translateY(-2px)` + border-color | 200ms |
| لینک‌های nav | تغییر رنگ | 150ms |
| Section reveal | `translateY(20px)→0` + opacity | 600ms |
| Skill bar | width animation | 1000ms |
| Toast | slide-in از راست | 300ms |

**قانون اجباری:**
```css
@media (prefers-reduced-motion: reduce) {
  *, *::before, *::after {
    animation-duration: 0.01ms !important;
    transition-duration: 0.01ms !important;
  }
}
```

### ساختار صفحه اصلی (ترتیب Sections)

1. Hero — گرادیان متن، دو CTA (primary + outline)
2. Team Stats — 4 عدد animated counter
3. Latest Projects — گرید 3 ستونه (6 آیتم)
4. About / Technologies — split layout
5. Latest Blog Posts — گرید 3 ستونه (3 آیتم)
6. Testimonials — کارت‌های 3 ستونه
7. Contact Form — split: فرم + اطلاعات تماس
8. Footer — 4 ستون، dark

### قوانین کلیدی UI

| قانون | جزئیات |
|-------|---------|
| آیکون | فقط Font Awesome class — هرگز emoji |
| cursor | `cursor: pointer` روی تمام المان‌های کلیکی |
| hover shift | هرگز `scale > 1.03` روی کارت‌ها |
| کنتراست light | متن ثانویه حداقل `#475569` |
| کارت در light mode | `background: #fff; border: 1px solid #E2E8F0` |
| navbar | sticky (نه floating) — محتوا زیر navbar شروع می‌شود |
| Dark/Light toggle | ذخیره در `localStorage`، اعمال روی `<html data-theme="...">` |
| Breadcrumb | در تمام صفحات به‌جز Home |
| Responsive | 375px / 768px / 1024px / 1440px |

### Gradient Text

```css
.text-gradient {
  background: linear-gradient(135deg, #3B82F6, #8B5CF6);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}
```

### Pre-Delivery Checklist (اجباری برای هر صفحه)

- [ ] بدون emoji به عنوان آیکون
- [ ] `cursor: pointer` روی همه المان‌های کلیکی
- [ ] hover states با transition 150–300ms
- [ ] light mode: کنتراست متن حداقل 4.5:1
- [ ] focus states برای keyboard navigation
- [ ] `prefers-reduced-motion` رعایت شده
- [ ] responsive در 375px، 768px، 1024px، 1440px
- [ ] محتوا پشت navbar پنهان نشده
- [ ] هر دو dark و light mode تست شده

---

## Architecture

### Clean Architecture لایه‌بندی

```
┌──────────────────────────────────────────────────────────────┐
│                    Presentation Layer                         │
│         (Controllers, Views, ViewModels, Middlewares)         │
├──────────────────────────────────────────────────────────────┤
│                   Application Layer                           │
│      (Services, DTOs, Interfaces, Validators, Mappings)       │
├──────────────────────────────────────────────────────────────┤
│                    Domain Layer                               │
│         (Entities, Enums, Domain Events, Specifications)      │
├──────────────────────────────────────────────────────────────┤
│                 Infrastructure Layer                          │
│   (EF Core DbContext, Repositories, Email, FileStorage, Cache)│
└──────────────────────────────────────────────────────────────┘
```

### قوانین وابستگی

- **Domain** به هیچ لایه‌ای وابسته نیست
- **Application** فقط به Domain وابسته است
- **Infrastructure** به Application و Domain وابسته است
- **Presentation** به Application وابسته است (نه مستقیم به Infrastructure)
- تمام وابستگی‌های خارجی از طریق Dependency Injection تزریق می‌شوند

### نمودار جریان درخواست

```
HTTP Request
    │
    ▼
Middleware Pipeline
(Auth, CSRF, Error Handling, Cache)
    │
    ▼
Controller Action
    │
    ▼
Service Layer (Application)
    │
    ▼
Repository Layer (Infrastructure)
    │
    ▼
Entity Framework Core
    │
    ▼
SQL Server
```

### نقش‌های کاربری و سطح دسترسی

| نقش | صفحات عمومی | مدیریت محتوای خود | مدیریت همه محتوا | مدیریت اعضا | تنظیمات سیستم |
|-----|-------------|------------------|-----------------|------------|--------------|
| Visitor | ✅ | ❌ | ❌ | ❌ | ❌ |
| Member | ✅ | پروفایل خود | ❌ | ❌ | ❌ |
| Author | ✅ | Blog_Posts خود | ❌ | ❌ | ❌ |
| Manager | ✅ | ✅ | ✅ | ✅ | ❌ |
| Admin | ✅ | ✅ | ✅ | ✅ | ✅ |

---

## Components and Interfaces

### ساختار پوشه‌های پروژه

```
TeamPortfolio/
├── src/
│   ├── TeamPortfolio.Domain/
│   │   ├── Entities/
│   │   │   ├── TeamMember.cs
│   │   │   ├── PortfolioItem.cs
│   │   │   ├── BlogPost.cs
│   │   │   ├── Comment.cs
│   │   │   ├── Category.cs
│   │   │   ├── Tag.cs
│   │   │   ├── Skill.cs
│   │   │   ├── WorkExperience.cs
│   │   │   ├── Education.cs
│   │   │   └── ContactMessage.cs
│   │   ├── Enums/
│   │   │   ├── CommentStatus.cs
│   │   │   ├── BlogPostStatus.cs
│   │   │   └── UserRole.cs
│   │   └── Common/
│   │       └── BaseEntity.cs
│   │
│   ├── TeamPortfolio.Application/
│   │   ├── Interfaces/
│   │   │   ├── Repositories/
│   │   │   │   ├── ITeamMemberRepository.cs
│   │   │   │   ├── IPortfolioRepository.cs
│   │   │   │   ├── IBlogRepository.cs
│   │   │   │   ├── ICommentRepository.cs
│   │   │   │   ├── ICategoryRepository.cs
│   │   │   │   └── IContactMessageRepository.cs
│   │   │   └── Services/
│   │   │       ├── ITeamMemberService.cs
│   │   │       ├── IPortfolioService.cs
│   │   │       ├── IBlogService.cs
│   │   │       ├── ICommentService.cs
│   │   │       ├── ISearchService.cs
│   │   │       ├── IFileUploadService.cs
│   │   │       ├── IEmailService.cs
│   │   │       ├── ICacheService.cs
│   │   │       └── ISeoService.cs
│   │   ├── Services/
│   │   │   ├── TeamMemberService.cs
│   │   │   ├── PortfolioService.cs
│   │   │   ├── BlogService.cs
│   │   │   ├── CommentService.cs
│   │   │   └── SearchService.cs
│   │   └── DTOs/
│   │       ├── TeamMemberDto.cs
│   │       ├── PortfolioItemDto.cs
│   │       ├── BlogPostDto.cs
│   │       └── SearchResultDto.cs
│   │
│   ├── TeamPortfolio.Infrastructure/
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── Migrations/
│   │   │   └── Configurations/
│   │   ├── Repositories/
│   │   │   ├── BaseRepository.cs
│   │   │   ├── TeamMemberRepository.cs
│   │   │   ├── PortfolioRepository.cs
│   │   │   ├── BlogRepository.cs
│   │   │   └── CommentRepository.cs
│   │   ├── Services/
│   │   │   ├── FileUploadService.cs
│   │   │   ├── EmailService.cs
│   │   │   └── CacheService.cs
│   │   └── Identity/
│   │       └── ApplicationUser.cs
│   │
│   └── TeamPortfolio.Web/
│       ├── Controllers/
│       │   ├── HomeController.cs
│       │   ├── TeamController.cs
│       │   ├── PortfolioController.cs
│       │   ├── BlogController.cs
│       │   ├── SearchController.cs
│       │   └── ContactController.cs
│       ├── Areas/Admin/Controllers/
│       │   ├── DashboardController.cs
│       │   ├── TeamMembersController.cs
│       │   ├── PortfolioController.cs
│       │   ├── BlogController.cs
│       │   ├── CommentsController.cs
│       │   └── CategoriesController.cs
│       ├── Views/
│       ├── ViewModels/
│       └── wwwroot/
└── tests/
    ├── TeamPortfolio.UnitTests/
    └── TeamPortfolio.PropertyTests/
```

### اینترفیس‌های کلیدی سرویس‌ها

#### ITeamMemberService

```csharp
public interface ITeamMemberService
{
    Task<IEnumerable<TeamMemberDto>> GetAllActiveAsync();
    Task<TeamMemberDto?> GetByIdAsync(int id);
    Task<TeamMemberDto?> GetBySlugAsync(string slug);
    Task<IEnumerable<TeamMemberDto>> SearchAsync(string query);
    Task<TeamMemberDto> CreateAsync(CreateTeamMemberDto dto);
    Task<TeamMemberDto> UpdateAsync(int id, UpdateTeamMemberDto dto);
    Task DeleteAsync(int id);
}
```

#### IBlogService

```csharp
public interface IBlogService
{
    Task<PagedResult<BlogPostDto>> GetPublishedAsync(int page, int pageSize, int? categoryId = null);
    Task<BlogPostDto?> GetBySlugAsync(string slug);
    Task IncrementViewCountAsync(int postId);
    Task<IEnumerable<BlogPostDto>> GetRelatedAsync(int postId, int count = 3);
    Task<IEnumerable<BlogPostDto>> GetLatestAsync(int count = 3);
    Task<BlogPostDto> CreateAsync(CreateBlogPostDto dto, string authorId);
    Task<BlogPostDto> UpdateAsync(int id, UpdateBlogPostDto dto, string userId, bool isAdmin);
    Task DeleteAsync(int id, string userId, bool isAdmin);
    Task PublishAsync(int id);
    Task UnpublishAsync(int id);
}
```

#### ISearchService

```csharp
public interface ISearchService
{
    Task<SearchResultDto> SearchAsync(string query);
}

public class SearchResultDto
{
    public string Query { get; set; } = "";
    public IEnumerable<TeamMemberDto> Members { get; set; } = [];
    public IEnumerable<PortfolioItemDto> Projects { get; set; } = [];
    public IEnumerable<BlogPostDto> Articles { get; set; } = [];
    public int TotalCount => Members.Count() + Projects.Count() + Articles.Count();
}
```

#### IFileUploadService

```csharp
public interface IFileUploadService
{
    Task<FileUploadResult> UploadImageAsync(IFormFile file, string folder);
    Task DeleteAsync(string filePath);
    bool IsValidImageFile(IFormFile file);   // validates MIME + extension + size
}

public record FileUploadResult(bool Success, string? FilePath, string? ErrorMessage);
```

#### ICommentService

```csharp
public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetApprovedForPostAsync(int postId);
    Task<IEnumerable<CommentDto>> GetPendingAsync();
    Task<CommentDto> SubmitAsync(SubmitCommentDto dto);
    Task ApproveAsync(int id);
    Task DeleteAsync(int id);
    Task UpdateBodyAsync(int id, string newBody);
}
```

#### ICacheService

```csharp
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
    Task RemoveByPrefixAsync(string prefix);
}
```

---

## Data Models

### نمودار Entity-Relationship

```
ApplicationUser (Identity)
    │ 1
    │
    ▼ *
TeamMember ──────────── Skills (1:*)
    │                   WorkExperiences (1:*)
    │                   Educations (1:*)
    │
    ├──────────────────► BlogPosts (1:*)
    │                         │
    │                         ├── Category (many:1)
    │                         ├── Tags (many:many via BlogPostTags)
    │                         └── Comments (1:*)
    │
    └──────────────────► PortfolioItemMembers (many:many)
                              │
                              ▼
                         PortfolioItems ──── PortfolioImages (1:*)

ContactMessages (standalone)
```

### Entities (Domain Layer)

#### BaseEntity

```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
```

#### TeamMember

```csharp
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
```

#### PortfolioItem

```csharp
public class PortfolioItem : BaseEntity
{
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Description { get; set; } = "";
    public string Technologies { get; set; } = "";   // comma-separated or JSON
    public DateTime StartDate { get; set; }
    public string? GitHubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string CoverImagePath { get; set; } = "";
    public bool IsPublished { get; set; } = false;
    public ICollection<PortfolioItemMember> Members { get; set; } = [];
    public ICollection<PortfolioImage> Images { get; set; } = [];
}
```

#### BlogPost

```csharp
public class BlogPost : BaseEntity
{
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Body { get; set; } = "";
    public string? CoverImagePath { get; set; }
    public BlogPostStatus Status { get; set; } = BlogPostStatus.Draft;
    public int ViewCount { get; set; } = 0;
    public DateTime? PublishDate { get; set; }
    public int AuthorId { get; set; }
    public TeamMember Author { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<BlogPostTag> BlogPostTags { get; set; } = [];
}
```

#### Comment

```csharp
public class Comment : BaseEntity
{
    public string CommenterName { get; set; } = "";
    public string CommenterEmail { get; set; } = "";
    public string Body { get; set; } = "";
    public CommentStatus Status { get; set; } = CommentStatus.Pending;
    public int PostId { get; set; }
    public BlogPost Post { get; set; } = null!;
}
```

#### Skill

```csharp
public class Skill : BaseEntity
{
    public string Name { get; set; } = "";
    public int ProficiencyLevel { get; set; }  // 1–100
    public int MemberId { get; set; }
    public TeamMember Member { get; set; } = null!;
}
```

#### ContactMessage

```csharp
public class ContactMessage : BaseEntity
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Body { get; set; } = "";
    public bool IsRead { get; set; } = false;
}
```

### Enums

```csharp
public enum BlogPostStatus { Draft, Published }
public enum CommentStatus { Pending, Approved, Rejected }
```

### ApplicationDbContext (Infrastructure)

```csharp
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<PortfolioItem> PortfolioItems => Set<PortfolioItem>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<WorkExperience> WorkExperiences => Set<WorkExperience>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<PortfolioItemMember> PortfolioItemMembers => Set<PortfolioItemMember>();
    public DbSet<BlogPostTag> BlogPostTags => Set<BlogPostTag>();
    public DbSet<PortfolioImage> PortfolioImages => Set<PortfolioImage>();
}
```

### PagedResult\<T\>

```csharp
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}
```

---

## Correctness Properties

*A property is a characteristic or behavior that should hold true across all valid executions of a system — essentially, a formal statement about what the system should do. Properties serve as the bridge between human-readable specifications and machine-verifiable correctness guarantees.*

---

### Property 1: Latest Items Ordering Invariant

*For any* collection of Portfolio_Items or Blog_Posts, the "get latest N" operation SHALL return exactly N items (or all items if fewer than N exist) ordered by creation/publish date descending, with the most recent item first.

**Validates: Requirements 1.2, 1.3**

---

### Property 2: Active Member Filter

*For any* collection of Team_Members containing a mix of active and inactive records, querying for all active members SHALL return every active member and no inactive member — regardless of insertion order or total count.

**Validates: Requirements 3.1**

---

### Property 3: Team Member Search Filter

*For any* search query string Q and any collection of Team_Members, every member returned by the search SHALL have a name or role/specialization that contains Q (case-insensitive), and no member whose name and role both do not contain Q SHALL appear in the results.

**Validates: Requirements 3.4**

---

### Property 4: Portfolio Tag Filter

*For any* technology tag T and any collection of Portfolio_Items, every item returned by the tag filter SHALL include T in its technology list, and no item lacking T SHALL appear in the filtered results.

**Validates: Requirements 5.2**

---

### Property 5: Pagination Correctness

*For any* ordered list of N items, page number P, and page size S: the paginated result SHALL contain exactly min(S, N - (P-1)*S) items (clamped to 0 minimum), the items SHALL match the correct positional slice of the ordered list, and TotalPages SHALL equal ⌈N/S⌉.

**Validates: Requirements 5.4, 6.3**

---

### Property 6: Search Query Matching

*For any* non-empty search query Q of at least 2 characters and any backing dataset of Team_Members, Portfolio_Items, and Blog_Posts: every result in the Members group SHALL match Q in name or specialization, every result in the Projects group SHALL match Q in title or description, every result in the Articles group SHALL match Q in title or body content; and results SHALL be grouped by type with accurate per-group counts.

**Validates: Requirements 6.4, 17.2, 17.3**

---

### Property 7: View Count Monotonicity

*For any* Blog_Post with an initial view count V, calling IncrementViewCount exactly N times SHALL result in a final view count of exactly V + N.

**Validates: Requirements 7.2**

---

### Property 8: Related Posts Relevance

*For any* Blog_Post P, the set of related posts returned SHALL have cardinality ≤ 3, SHALL NOT include P itself, and every returned post SHALL share at least one Tag with P OR belong to the same Category as P.

**Validates: Requirements 7.3**

---

### Property 9: Comment Submission Invariant

*For any* comment submission with a non-empty name, syntactically valid email address, and non-empty body: the persisted Comment SHALL have Status = Pending, and the CommenterName, CommenterEmail, and Body fields SHALL exactly match the submitted values.

**Validates: Requirements 7.5**

---

### Property 10: Comment Validation Rejection

*For any* comment submission where at least one of the following holds — name is empty or whitespace-only, email is syntactically invalid, body is empty or whitespace-only — the submission SHALL be rejected without persisting any record.

**Validates: Requirements 7.6**

---

### Property 11: Contact Form Submission Invariant

*For any* contact form submission with non-empty full name, syntactically valid email, non-empty subject, and non-empty body: the persisted ContactMessage SHALL have IsRead = false and all four fields SHALL exactly match the submitted values.

**Validates: Requirements 8.5**

---

### Property 12: Contact Form Validation Rejection

*For any* contact form submission where at least one required field (full name, email, subject, body) is empty/whitespace-only, or the email is syntactically invalid: the submission SHALL be rejected without persisting any record.

**Validates: Requirements 8.6**

---

### Property 13: File Upload Validation

*For any* uploaded file F: F SHALL be accepted if and only if its MIME type is one of {image/jpeg, image/png, image/webp}, its file extension matches the MIME type, AND its size is ≤ 5 MB. Any file failing any of these conditions SHALL be rejected with a descriptive error message.

**Validates: Requirements 11.2, 11.3, 12.2, 20.5, 20.6**

---

### Property 14: Skill Proficiency Level Validation

*For any* integer value V submitted as a skill proficiency level: the value SHALL be accepted if and only if 1 ≤ V ≤ 100. Any value outside this range SHALL be rejected with a validation error.

**Validates: Requirements 12.3**

---

### Property 15: Slug Generation Correctness

*For any* title string T, the generated slug SHALL be non-empty, SHALL contain only lowercase ASCII letters, digits, and hyphens, SHALL NOT begin or end with a hyphen, and SHALL NOT contain consecutive hyphens. Furthermore, parsing the original title from the slug's hyphen-separated tokens SHALL recover the meaningful words of the original title (case-insensitive, ignoring stop words and special characters).

**Validates: Requirements 19.5**

---

### Property 16: Cache Invalidation on Publish

*For any* cached home page snapshot: after a new Portfolio_Item or Blog_Post is published, the next call to fetch home page data SHALL NOT return the old cached snapshot but SHALL reflect the newly published item.

**Validates: Requirements 21.1, 21.2**

---

### Property 17: Password Registration Complexity

*For any* password string P: registration SHALL succeed if P has length ≥ 8 AND contains at least one letter AND at least one digit. Registration SHALL be rejected with a validation error for any P that fails any of these conditions.

**Validates: Requirements 9.1**

---

## Error Handling

### استراتژی کلی مدیریت خطا

سیستم از یک رویکرد لایه‌بندی‌شده برای مدیریت خطا استفاده می‌کند:

```
Controller Action
    │
    ├── ValidationException  ──► بازگشت ViewModel با خطاهای اعتبارسنجی (400)
    ├── NotFoundException    ──► صفحه 404 سفارشی
    ├── ForbiddenException   ──► صفحه 403 یا Redirect به Login
    └── Exception            ──► صفحه 500 سفارشی + لاگ Serilog
```

### Middleware مدیریت خطای سراسری

```csharp
// Program.cs
app.UseExceptionHandler("/Error/500");
app.UseStatusCodePagesWithReExecute("/Error/{0}");
```

```csharp
// ErrorController.cs
public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult Index(int statusCode) => statusCode switch
    {
        404 => View("NotFound"),
        403 => View("Forbidden"),
        _   => View("ServerError")
    };
}
```

### قوانین مدیریت خطا

| نوع خطا | رفتار | لاگ |
|---------|-------|-----|
| اعتبارسنجی فرم (ModelState invalid) | نمایش خطاها در فرم، بدون Redirect | خیر |
| آپلود فایل نامعتبر | پیام خطای توصیفی به کاربر | خیر |
| رکورد پیدا نشد (NotFound) | صفحه 404 سفارشی | خیر |
| دسترسی غیرمجاز | Redirect به صفحه Login یا 403 | خیر |
| خطای پایگاه داده | صفحه 500 سفارشی، بدون جزئیات فنی | بله (Serilog) |
| خطای ناشناخته سرور | صفحه 500 سفارشی، بدون Stack Trace | بله (Serilog) |

### اعتبارسنجی ورودی‌ها

**لایه Client-Side (JavaScript/jQuery Unobtrusive Validation):**
- اعتبارسنجی فرم‌ها بلادرنگ قبل از ارسال
- نمایش خطاها در کنار فیلد مربوطه

**لایه Server-Side (Data Annotations + Fluent Validation):**
```csharp
// مثال: SubmitCommentDto
public class SubmitCommentDto
{
    [Required(ErrorMessage = "نام الزامی است")]
    [StringLength(100)]
    public string CommenterName { get; set; } = "";

    [Required(ErrorMessage = "ایمیل الزامی است")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل نامعتبر است")]
    public string CommenterEmail { get; set; } = "";

    [Required(ErrorMessage = "متن نظر الزامی است")]
    [StringLength(2000, MinimumLength = 10)]
    public string Body { get; set; } = "";

    public int PostId { get; set; }
}
```

**لایه Domain (Business Rules):**
- بررسی قوانین کسب‌وکار در Service Layer
- پرتاب Exception های Domain-specific در صورت نقض قوانین

### XSS Prevention

تمام محتوای تولیدشده توسط کاربر قبل از رندر در View باید Sanitize شود:

```csharp
// در FileUploadService یا BlogService
public string SanitizeHtml(string rawHtml)
{
    // استفاده از HtmlSanitizer NuGet package
    var sanitizer = new HtmlSanitizer();
    sanitizer.AllowedTags.Add("pre");
    sanitizer.AllowedTags.Add("code");
    return sanitizer.Sanitize(rawHtml);
}
```

---

## Testing Strategy

### رویکرد کلی

این پروژه از دو نوع تست مکمل استفاده می‌کند:

- **Unit Tests**: بررسی مثال‌های مشخص، حالت‌های مرزی و شرایط خطا
- **Property Tests**: بررسی خواص جهانی روی ورودی‌های تصادفی (Property-Based Testing)

Unit Tests برای موارد مشخص مناسب‌اند؛ Property Tests از طریق تولید ورودی تصادفی، لبه‌های پنهان باگ‌ها را پیدا می‌کنند که با مثال‌های دستی قابل کشف نیستند.

### کتابخانه‌های تست

| نوع تست | کتابخانه |
|--------|----------|
| Unit Testing | xUnit |
| Mocking | Moq |
| Property-Based Testing | FsCheck.Xunit (یا CsCheck) |
| Assertion | FluentAssertions |
| In-Memory DB | EF Core InMemory Provider |

### Unit Tests

**تمرکز Unit Tests:**
- مثال‌های مشخص برای رفتار صحیح سرویس‌ها
- Integration points بین Controller و Service
- حالت‌های مرزی و شرایط خطا
- رفتار Middleware و Authorization

نمونه Unit Tests:
- `HomeService.GetHomePageData_Returns_Correct_Structure`
- `BlogService.Publish_Sets_PublishDate_To_Now`
- `CommentService.Approve_Changes_Status_To_Approved`
- `CategoryService.Delete_WithPosts_Throws_InvalidOperation`
- `SearchService.EmptyQuery_Returns_EmptyResult`

### Property Tests

هر Property Test باید حداقل **100 iteration** اجرا شود.

فرمت تگ برای هر تست:
```
Feature: team-portfolio-website, Property {N}: {property_text}
```

#### Property 1: Latest Items Ordering — `LatestItemsOrdering_ReturnsCorrectCountAndOrder`

```csharp
[Property(Arbitrary = new[] { typeof(PortfolioItemArbitrary) })]
// Feature: team-portfolio-website, Property 1: Latest items ordering invariant
public Property GetLatestPortfolioItems_ReturnsCorrectCountAndOrder(
    NonEmptyArray<PortfolioItem> items)
{
    // Arrange: items با تاریخ‌های تصادفی
    // Act: GetLatestAsync(6)
    // Assert: count <= 6 AND ordered by CreatedAt DESC
}
```

#### Property 2: Active Member Filter — `ActiveMemberFilter_NeverReturnsInactive`

```csharp
// Feature: team-portfolio-website, Property 2: Active member filter
public Property GetAllActive_NeverReturnsInactiveMembers(
    TeamMember[] members)
{
    // Assert: all returned members have IsActive == true
}
```

#### Property 3: Team Member Search — `SearchMembers_AllResultsMatchQuery`

```csharp
// Feature: team-portfolio-website, Property 3: Team member search filter
public Property SearchMembers_AllResultsMatchQuery(
    string query, TeamMember[] members)
{
    // Assert: every result has Name.Contains(query) || Role.Contains(query)
}
```

#### Property 4: Portfolio Tag Filter — `TagFilter_AllResultsContainTag`

```csharp
// Feature: team-portfolio-website, Property 4: Portfolio tag filter
public Property FilterByTag_AllResultsContainTag(
    string tag, PortfolioItem[] items)
{
    // Assert: every result has Technologies.Contains(tag)
}
```

#### Property 5: Pagination Correctness — `Pagination_ReturnsCorrectSlice`

```csharp
// Feature: team-portfolio-website, Property 5: Pagination correctness
public Property Pagination_ReturnsCorrectSliceAndCounts(
    PositiveInt n, PositiveInt page, PositiveInt pageSize)
{
    // Assert: items.Count == min(pageSize, max(0, n - (page-1)*pageSize))
    // Assert: TotalPages == Ceiling(n / pageSize)
}
```

#### Property 6: Search Query Matching — `Search_AllResultsMatchQuery`

```csharp
// Feature: team-portfolio-website, Property 6: Search query matching
public Property Search_AllResultsMatchQueryInAppropriateFields(
    NonNull<string> query, SearchDataset dataset)
{
    // Assert: members match in name/role
    // Assert: projects match in title/description
    // Assert: posts match in title/body
}
```

#### Property 7: View Count Monotonicity — `IncrementViewCount_IsMonotonic`

```csharp
// Feature: team-portfolio-website, Property 7: View count monotonicity
public Property IncrementViewCount_NTimesIncreasesBy_N(
    BlogPost post, PositiveInt n)
{
    var initial = post.ViewCount;
    for (int i = 0; i < n.Get; i++) IncrementViewCount(post);
    return (post.ViewCount == initial + n.Get).ToProperty();
}
```

#### Property 8: Related Posts Relevance — `RelatedPosts_ShareTagOrCategory`

```csharp
// Feature: team-portfolio-website, Property 8: Related posts relevance
public Property GetRelated_ReturnsAtMost3_SharingTagOrCategory(
    BlogPost targetPost, BlogPost[] allPosts)
{
    var related = GetRelated(targetPost, allPosts);
    // Assert: related.Count() <= 3
    // Assert: all share a tag or same category with targetPost
    // Assert: none is targetPost itself
}
```

#### Property 9: Comment Submission Invariant — `ValidComment_SavedWithPendingStatus`

```csharp
// Feature: team-portfolio-website, Property 9: Comment submission invariant
public Property ValidCommentSubmission_AlwaysSavedAsPending(
    ValidCommentInput input)
{
    var comment = Submit(input);
    // Assert: comment.Status == CommentStatus.Pending
    // Assert: comment.CommenterName == input.Name
    // Assert: comment.CommenterEmail == input.Email
    // Assert: comment.Body == input.Body
}
```

#### Property 10: Comment Validation Rejection — `InvalidComment_IsRejected`

```csharp
// Feature: team-portfolio-website, Property 10: Comment validation rejection
public Property InvalidCommentSubmission_AlwaysRejected(
    InvalidCommentInput input)
{
    var result = Validate(input);
    return (!result.IsValid).ToProperty();
}
```

#### Property 11 & 12: Contact Form (Valid/Invalid) — same pattern as 9 & 10

#### Property 13: File Upload Validation — `FileUpload_AcceptsOnlyValidMimeAndSize`

```csharp
// Feature: team-portfolio-website, Property 13: File upload validation
public Property FileUpload_ValidMimeAndSize_Accepted(FileInput file)
{
    var isValid = file.Mime is "image/jpeg" or "image/png" or "image/webp"
                  && file.Size <= 5 * 1024 * 1024;
    var result = Validate(file);
    return (result.Success == isValid).ToProperty();
}
```

#### Property 14: Skill Level Validation — `SkillLevel_1To100_Accepted`

```csharp
// Feature: team-portfolio-website, Property 14: Skill proficiency level validation
public Property SkillProficiency_ValidRange_Accepted(int level)
{
    var valid = level >= 1 && level <= 100;
    var result = ValidateSkillLevel(level);
    return (result.IsValid == valid).ToProperty();
}
```

#### Property 15: Slug Generation — `Slug_IsUrlSafe`

```csharp
// Feature: team-portfolio-website, Property 15: Slug generation correctness
public Property GenerateSlug_ProducesUrlSafeString(NonNull<string> title)
{
    var slug = GenerateSlug(title.Get);
    // Assert: slug matches regex ^[a-z0-9]+(-[a-z0-9]+)*$
    // Assert: slug is non-empty
}
```

#### Property 16: Cache Invalidation — `Publish_InvalidatesCache`

```csharp
// Feature: team-portfolio-website, Property 16: Cache invalidation on publish
public Property PublishingNewItem_InvalidatesHomepageCache(
    CachedHomePageData cached, PortfolioItem newItem)
{
    var cache = BuildCache(cached);
    Publish(newItem);
    return (cache.Get(HomeCacheKey) == null).ToProperty();
}
```

#### Property 17: Password Complexity — `ValidPassword_MeetsComplexity`

```csharp
// Feature: team-portfolio-website, Property 17: Password registration complexity
public Property Password_ValidComplexity_Accepted(string password)
{
    var isValid = password.Length >= 8
                  && password.Any(char.IsLetter)
                  && password.Any(char.IsDigit);
    var result = ValidatePassword(password);
    return (result.IsValid == isValid).ToProperty();
}
```

### تست‌های یکپارچه‌سازی (Integration Tests)

- اتصال پایگاه داده و اعمال Migration
- CSRF Token validation روی تمام فرم‌ها (Requirement 20.1)
- Role-Based Authorization: بررسی دسترسی هر نقش به Controller Actions
- Email ارسال‌شده در ثبت‌نام و بازیابی رمز عبور
- File Storage: آپلود واقعی و حذف فایل
- Sitemap و robots.txt معتبر

### تست‌های Smoke

- Database Connection و Migration موفق
- ASP.NET Identity پیکربندی صحیح (رمزهای هش‌شده)
- سرویس کش در دسترس
- SMTP Configuration

### Coverage هدف

| لایه | هدف Coverage |
|------|-------------|
| Domain Entities + Enums | 100% |
| Application Services | ≥ 90% |
| Validators / DTOs | 100% |
| Infrastructure Repositories | ≥ 80% (با InMemory DB) |
| Controllers (Unit) | ≥ 75% |
