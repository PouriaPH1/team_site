# Implementation Plan: Team Portfolio Website

## Overview

پیاده‌سازی وب‌سایت Portfolio تیم برنامه‌نویسی با ASP.NET Core 9 MVC و Clean Architecture.
تسک‌ها به‌ترتیب از لایه Domain شروع و تا لایه Presentation پیش می‌روند تا وابستگی‌ها رعایت شود.

---

## Tasks

- [x] 1. راه‌اندازی Solution و ساختار پروژه
  - ایجاد Solution با چهار پروژه: Domain، Application، Infrastructure، Web
  - تنظیم Project References مطابق قوانین Clean Architecture
  - نصب NuGet Packages: EF Core، Identity، Serilog، ImageSharp، MailKit، Slugify-Net، HtmlSanitizer، FsCheck.Xunit، FluentAssertions، Moq
  - ایجاد پوشه‌های پایه در هر پروژه مطابق ساختار طراحی
  - _Requirements: 23.1_

- [x] 2. پیاده‌سازی لایه Domain
  - [x] 2.1 ایجاد BaseEntity و Enum ها
    - پیاده‌سازی `BaseEntity` با فیلدهای Id، CreatedAt، UpdatedAt
    - پیاده‌سازی `BlogPostStatus` و `CommentStatus` و `UserRole` enum
    - _Requirements: 23.2, 23.4, 23.5_

  - [x] 2.2 پیاده‌سازی Domain Entities
    - پیاده‌سازی `TeamMember`، `Skill`، `WorkExperience`، `Education`
    - پیاده‌سازی `PortfolioItem`، `PortfolioImage`، `PortfolioItemMember`
    - پیاده‌سازی `BlogPost`، `Category`، `Tag`، `BlogPostTag`
    - پیاده‌سازی `Comment` و `ContactMessage`
    - _Requirements: 23.2, 23.3, 23.4, 23.5, 23.6, 23.7, 23.8, 23.9, 23.10, 23.11, 23.12_


- [x] 3. پیاده‌سازی لایه Infrastructure - پایگاه داده و Identity
  - [x] 3.1 پیاده‌سازی ApplicationDbContext و ApplicationUser
    - پیاده‌سازی `ApplicationUser` که از `IdentityUser` ارث می‌برد
    - پیاده‌سازی `ApplicationDbContext` با تمام DbSet ها
    - تنظیم Entity Configurations (Fluent API) برای روابط و محدودیت‌ها
    - ایجاد اولین Migration و Seed داده اولیه نقش‌ها
    - _Requirements: 9.3, 20.3, 23.1_

  - [ ]* 3.2 تست یکپارچگی دیتابیس
    - بررسی اتصال InMemory Database در تست‌ها
    - بررسی Migration موفق
    - _Requirements: 23.1_


- [x] 4. پیاده‌سازی لایه Application - اینترفیس‌ها و DTOs
  - [x] 4.1 تعریف اینترفیس‌های Repository
    - تعریف `IBaseRepository<T>` و اینترفیس‌های مشتق: `ITeamMemberRepository`، `IPortfolioRepository`، `IBlogRepository`، `ICommentRepository`، `ICategoryRepository`، `IContactMessageRepository`
    - _Requirements: 23.1_

  - [x] 4.2 تعریف اینترفیس‌های Service و DTOs
    - تعریف `ITeamMemberService`، `IPortfolioService`، `IBlogService`، `ICommentService`، `ISearchService`، `IFileUploadService`، `IEmailService`، `ICacheService`، `ISeoService`
    - پیاده‌سازی DTOs: `TeamMemberDto`، `PortfolioItemDto`، `BlogPostDto`، `SearchResultDto`، `CommentDto`
    - پیاده‌سازی `PagedResult<T>`
    - _Requirements: 5.4, 6.3, 17.1_


- [x] 5. پیاده‌سازی لایه Infrastructure - Repositories
  - [x] 5.1 پیاده‌سازی BaseRepository و TeamMemberRepository
    - پیاده‌سازی `BaseRepository<T>` با عملیات CRUD پایه
    - پیاده‌سازی `TeamMemberRepository` با متدهای `GetAllActiveAsync`، `GetBySlugAsync`، `SearchAsync`
    - _Requirements: 3.1, 3.4, 11.1_

  - [ ]* 5.2 نوشتن Property Test برای Active Member Filter
    - **Property 2: Active Member Filter**
    - **Validates: Requirements 3.1**

  - [ ]* 5.3 نوشتن Property Test برای Team Member Search
    - **Property 3: Team Member Search Filter**
    - **Validates: Requirements 3.4**

  - [x] 5.4 پیاده‌سازی PortfolioRepository و BlogRepository
    - پیاده‌سازی `PortfolioRepository` با متدهای `GetPublishedAsync`، `FilterByTagAsync`، `GetLatestAsync`
    - پیاده‌سازی `BlogRepository` با متدهای `GetPublishedAsync`، `GetBySlugAsync`، `GetRelatedAsync`، `GetLatestAsync`
    - _Requirements: 1.2, 1.3, 5.1, 5.2, 6.1, 7.3_

  - [ ]* 5.5 نوشتن Property Test برای Latest Items Ordering
    - **Property 1: Latest Items Ordering Invariant**
    - **Validates: Requirements 1.2, 1.3**

  - [ ]* 5.6 نوشتن Property Test برای Portfolio Tag Filter
    - **Property 4: Portfolio Tag Filter**
    - **Validates: Requirements 5.2**

  - [x] 5.7 پیاده‌سازی CommentRepository و ContactMessageRepository
    - پیاده‌سازی `CommentRepository` با متدهای `GetApprovedForPostAsync`، `GetPendingAsync`
    - پیاده‌سازی `ContactMessageRepository`
    - _Requirements: 7.4, 8.5, 15.1_


- [x] 6. پیاده‌سازی Pagination در Repository/Service
  - [x] 6.1 پیاده‌سازی منطق Pagination در Repository ها
    - افزودن متدهای `GetPagedAsync` به BlogRepository و PortfolioRepository
    - محاسبه درست `TotalPages`، `HasPreviousPage`، `HasNextPage`
    - _Requirements: 5.4, 6.3, 21.4_

  - [ ]* 6.2 نوشتن Property Test برای Pagination Correctness
    - **Property 5: Pagination Correctness**
    - **Validates: Requirements 5.4, 6.3**

- [x] 7. Checkpoint — اطمینان از پاس شدن تمام تست‌ها تا اینجا
  - Ensure all tests pass, ask the user if questions arise.


- [x] 8. پیاده‌سازی لایه Application - Services (Core)
  - [x] 8.1 پیاده‌سازی TeamMemberService
    - پیاده‌سازی تمام متدهای `ITeamMemberService`
    - Slug generation با استفاده از Slugify-Net
    - _Requirements: 3.1, 3.4, 11.1, 11.4, 11.5, 19.5_

  - [ ]* 8.2 نوشتن Property Test برای Slug Generation
    - **Property 15: Slug Generation Correctness**
    - **Validates: Requirements 19.5**

  - [x] 8.3 پیاده‌سازی BlogService
    - پیاده‌سازی تمام متدهای `IBlogService` شامل `PublishAsync`، `UnpublishAsync`، `IncrementViewCountAsync`، `GetRelatedAsync`
    - _Requirements: 1.3, 6.1, 7.2, 7.3, 14.2, 14.3, 14.4, 14.5_

  - [ ]* 8.4 نوشتن Property Test برای View Count Monotonicity
    - **Property 7: View Count Monotonicity**
    - **Validates: Requirements 7.2**

  - [ ]* 8.5 نوشتن Property Test برای Related Posts Relevance
    - **Property 8: Related Posts Relevance**
    - **Validates: Requirements 7.3**

  - [x] 8.6 پیاده‌سازی PortfolioService
    - پیاده‌سازی تمام متدهای `IPortfolioService` شامل CRUD، Publish/Unpublish، Tag filter
    - _Requirements: 5.1, 5.2, 13.1, 13.2, 13.4, 13.5_


- [x] 9. پیاده‌سازی CommentService و ContactService
  - [x] 9.1 پیاده‌سازی CommentService با Validation
    - پیاده‌سازی `SubmitAsync`، `ApproveAsync`، `DeleteAsync`، `UpdateBodyAsync`
    - اعتبارسنجی: نام غیر‌خالی، ایمیل معتبر، متن غیر‌خالی
    - تنظیم `Status = Pending` در ثبت نظر جدید
    - _Requirements: 7.4, 7.5, 7.6, 15.1, 15.2, 15.3, 15.4_

  - [ ]* 9.2 نوشتن Property Test برای Comment Submission Invariant
    - **Property 9: Comment Submission Invariant**
    - **Validates: Requirements 7.5**

  - [ ]* 9.3 نوشتن Property Test برای Comment Validation Rejection
    - **Property 10: Comment Validation Rejection**
    - **Validates: Requirements 7.6**

  - [x] 9.4 پیاده‌سازی ContactService با Validation
    - پیاده‌سازی `SubmitContactAsync` با اعتبارسنجی تمام فیلدهای الزامی
    - تنظیم `IsRead = false` در ثبت پیام جدید
    - _Requirements: 8.5, 8.6_

  - [ ]* 9.5 نوشتن Property Test برای Contact Form Submission Invariant
    - **Property 11: Contact Form Submission Invariant**
    - **Validates: Requirements 8.5**

  - [ ]* 9.6 نوشتن Property Test برای Contact Form Validation Rejection
    - **Property 12: Contact Form Validation Rejection**
    - **Validates: Requirements 8.6**


- [x] 10. پیاده‌سازی SearchService
  - [x] 10.1 پیاده‌سازی SearchService
    - پیاده‌سازی `SearchAsync` که در TeamMembers، PortfolioItems و BlogPosts جستجو می‌کند
    - گروه‌بندی نتایج بر اساس نوع با تعداد هر گروه
    - رد کردن query های کوتاه‌تر از 2 کاراکتر
    - _Requirements: 17.1, 17.2, 17.3, 17.4_

  - [ ]* 10.2 نوشتن Property Test برای Search Query Matching
    - **Property 6: Search Query Matching**
    - **Validates: Requirements 6.4, 17.2, 17.3**


- [x] 11. پیاده‌سازی سرویس‌های Infrastructure - File, Cache, Email
  - [x] 11.1 پیاده‌سازی FileUploadService
    - پیاده‌سازی `IsValidImageFile`: بررسی MIME type (jpeg/png/webp)، extension، حداکثر 5 MB
    - پیاده‌سازی `UploadImageAsync` با ImageSharp برای ذخیره فایل
    - پیاده‌سازی `DeleteAsync`
    - _Requirements: 11.2, 11.3, 12.2, 20.5, 20.6_

  - [ ]* 11.2 نوشتن Property Test برای File Upload Validation
    - **Property 13: File Upload Validation**
    - **Validates: Requirements 11.2, 11.3, 12.2, 20.5, 20.6**

  - [x] 11.3 پیاده‌سازی Skill Validation در TeamMemberService
    - اعمال محدودیت `1 ≤ ProficiencyLevel ≤ 100` در ثبت/ویرایش مهارت
    - _Requirements: 12.3_

  - [ ]* 11.4 نوشتن Property Test برای Skill Proficiency Level Validation
    - **Property 14: Skill Proficiency Level Validation**
    - **Validates: Requirements 12.3**

  - [x] 11.5 پیاده‌سازی CacheService
    - پیاده‌سازی `ICacheService` با `IMemoryCache`
    - پیاده‌سازی `RemoveByPrefixAsync` برای Cache Invalidation
    - _Requirements: 21.1, 21.2_

  - [ ]* 11.6 نوشتن Property Test برای Cache Invalidation on Publish
    - **Property 16: Cache Invalidation on Publish**
    - **Validates: Requirements 21.1, 21.2**

  - [x] 11.7 پیاده‌سازی EmailService
    - پیاده‌سازی `IEmailService` با MailKit/SMTP برای ارسال ایمیل تأیید و بازیابی رمز عبور
    - _Requirements: 9.2, 9.5_


- [x] 12. Checkpoint — اطمینان از پاس شدن تمام تست‌های لایه‌های Domain، Application و Infrastructure
  - Ensure all tests pass, ask the user if questions arise.

- [x] 13. پیاده‌سازی احراز هویت و مجوزدهی
  - [x] 13.1 پیکربندی ASP.NET Identity و Password Validation
    - پیکربندی Identity در `Program.cs` با تنظیمات Password (حداقل 8 کاراکتر، یک حرف، یک عدد)
    - تعریف Policy های نقش: Admin، Manager، Author، Member
    - _Requirements: 9.1, 9.3, 20.3, 20.4_

  - [ ]* 13.2 نوشتن Property Test برای Password Registration Complexity
    - **Property 17: Password Registration Complexity**
    - **Validates: Requirements 9.1**

  - [x] 13.3 پیاده‌سازی Account Controllers (Register/Login/ForgotPassword/ResetPassword)
    - پیاده‌سازی `AccountController` با Actions: Register، Login، Logout، ForgotPassword، ResetPassword، ConfirmEmail
    - پشتیبانی از "Remember Me" با cookie 30 روزه
    - _Requirements: 9.1, 9.2, 9.3, 9.4, 9.5, 9.6, 9.7, 9.8, 9.9_

  - [x] 13.4 پیاده‌سازی Views احراز هویت
    - ایجاد Views: Register.cshtml، Login.cshtml، ForgotPassword.cshtml، ResetPassword.cshtml
    - _Requirements: 9.1, 9.5_


- [x] 14. راه‌اندازی Design System و CSS پایه
  - [x] 14.1 ایجاد CSS Variables و Design Tokens
    - ایجاد فایل `wwwroot/css/design-system.css` با تمام CSS Variables مطابق `design-system/team-portfolio/MASTER.md`
    - تعریف رنگ‌های Dark Mode: `--color-bg: #0A0F1E`، `--color-primary: #3B82F6`، `--color-accent: #8B5CF6` و بقیه tokens
    - تعریف رنگ‌های Light Mode در `[data-theme="light"]`
    - تعریف `--gradient-primary: linear-gradient(135deg, #3B82F6, #8B5CF6)`
    - تعریف spacing، border-radius، shadow و typography tokens
    - _Requirements: 18.1, 18.2_

  - [x] 14.2 اضافه کردن Google Fonts و Typography
    - اضافه کردن import فونت‌های Space Grotesk (heading) و DM Sans (body) در `_Layout.cshtml`
    - تعریف `--font-heading`، `--font-body`، `--font-mono` در CSS
    - اعمال فونت‌ها روی `h1–h6` و `body`
    - _Requirements: 18.1_

  - [x] 14.3 پیاده‌سازی Dark/Light Mode Toggle
    - پیاده‌سازی JavaScript toggle: خواندن از `localStorage`، اعمال روی `<html data-theme="...">`
    - اجرای `applyTheme()` قبل از رندر DOM (داخل `<head>`) برای جلوگیری از flash
    - دکمه toggle در navbar با آیکون Moon/Sun از Font Awesome (بدون emoji)
    - _Requirements: 18.2_

  - [x] 14.4 ایجاد Component Styles پایه
    - استایل‌های `.btn-primary`، `.btn-secondary`، `.btn-ghost` با transition 200ms
    - استایل کارت‌ها: `.card` با `border: 1px solid var(--color-border)`، hover effect `translateY(-2px)`
    - استایل badge/tag، form inputs، skill progress bar
    - کلاس `.text-gradient` برای gradient text روی عنوان‌ها
    - `@media (prefers-reduced-motion: reduce)` برای غیرفعال کردن انیمیشن‌ها
    - _Requirements: 18.3, 18.4_

  - [x] 14.5 پیاده‌سازی Toast Notification و Loading Animation
    - پیاده‌سازی toast system با JavaScript: slide-in از راست، 300ms، رنگ‌بندی success/error
    - پیاده‌سازی skeleton shimmer برای loading state کارت‌ها
    - پیاده‌سازی page loader اولیه با fade-out
    - _Requirements: 18.4, 18.5_

- [x] 15. پیاده‌سازی صفحات عمومی - Layout و صفحه اصلی
  - [x] 15.1 ایجاد Layout، Navigation و Footer
    - پیاده‌سازی `_Layout.cshtml` با Bootstrap 5، Font Awesome، jQuery، فونت‌های Google
    - Navbar: sticky top، height 64px، backdrop-filter blur(20px)، همه لینک‌ها `cursor: pointer`
    - آیکون‌های nav از Font Awesome (بدون emoji)، active link با رنگ primary و border-bottom
    - Mobile navbar: hamburger menu با slide-in drawer
    - Footer: 4 ستون، dark background، لینک‌های شبکه اجتماعی با Font Awesome icons
    - Breadcrumb component در تمام صفحات به‌جز Home
    - _Requirements: 1.8, 18.1, 18.2, 18.7_

  - [ ] 15.2 پیاده‌سازی HomeController و ViewModel
    - پیاده‌سازی `HomeController.Index` که داده‌ها را از Cache می‌خواند
    - پیاده‌سازی `HomeViewModel` با: HeroSection، LatestProjects، LatestPosts، Statistics، Technologies
    - اعمال Cache 10 دقیقه‌ای
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 21.1, 21.2_

  - [ ] 15.3 پیاده‌سازی View صفحه اصلی
    - ایجاد `Views/Home/Index.cshtml` با ترتیب sections: Hero → Stats → Projects → About/Tech → Blog → Testimonials → Contact → Footer
    - Hero: gradient text با `.text-gradient`، دو CTA (primary gradient button + outline button)
    - Stats: 4 counter با count-up animation (2000ms)
    - کارت‌های Projects و Blog: hover `translateY(-2px)`، `cursor: pointer`، badge فناوری‌ها با `.badge-primary`
    - Testimonials: کارت با border-left گرادیان
    - تمام آیکون‌ها از Font Awesome — بدون emoji
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 18.3_


- [x] 15. پیاده‌سازی صفحات عمومی - Layout و صفحه اصلی
  - [x] 15.1 ایجاد Layout، Navigation و Footer
    - پیاده‌سازی `_Layout.cshtml` با Bootstrap 5، Font Awesome، jQuery، فونت‌های Google
    - Navbar: sticky top، height 64px، backdrop-filter blur(20px)، همه لینک‌ها `cursor: pointer`
    - آیکون‌های nav از Font Awesome (بدون emoji)، active link با رنگ primary و border-bottom
    - Mobile navbar: hamburger menu با slide-in drawer
    - Footer: 4 ستون، dark background، لینک‌های شبکه اجتماعی با Font Awesome icons
    - Breadcrumb component در تمام صفحات به‌جز Home
    - _Requirements: 1.8, 18.1, 18.2, 18.7_

  - [x] 15.2 پیاده‌سازی HomeController و ViewModel
    - پیاده‌سازی `HomeController.Index` که داده‌ها را از Cache می‌خواند
    - پیاده‌سازی `HomeViewModel` با: HeroSection، LatestProjects، LatestPosts، Statistics، Technologies
    - اعمال Cache 10 دقیقه‌ای
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 21.1, 21.2_

  - [x] 15.3 پیاده‌سازی View صفحه اصلی
    - ایجاد `Views/Home/Index.cshtml` با ترتیب sections: Hero → Stats → Projects → About/Tech → Blog → Testimonials → Contact → Footer
    - Hero: gradient text با `.text-gradient`، دو CTA (primary gradient button + outline button)
    - Stats: 4 counter با count-up animation (2000ms)
    - کارت‌های Projects و Blog: hover `translateY(-2px)`، `cursor: pointer`، badge فناوری‌ها با `.badge-primary`
    - Testimonials: کارت با border-left گرادیان
    - تمام آیکون‌ها از Font Awesome — بدون emoji
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 18.3_


- [x] 16. پیاده‌سازی صفحات عمومی - تیم و پروفایل
  - [x] 16.1 پیاده‌سازی TeamController و Views
    - پیاده‌سازی `TeamController`: `Index` (لیست اعضا + فیلتر جستجو)، `Profile(slug)` (صفحه پروفایل کامل)
    - پیاده‌سازی `TeamIndexViewModel` و `MemberProfileViewModel`
    - ایجاد `Views/Team/Index.cshtml`: Grid کارت‌ها با hover `translateY(-2px)` و `cursor: pointer`، فیلتر جستجو با JavaScript، badge تخصص‌ها
    - ایجاد `Views/Team/Profile.cshtml`: بنر با gradient overlay، تصویر پروفایل، بیوگرافی، skill bars با gradient fill، سوابق کاری و تحصیلی، پروژه‌ها، مقالات، لینک‌های اجتماعی با Font Awesome icons
    - _Requirements: 2.1, 2.2, 2.3, 3.1, 3.2, 3.3, 3.4, 4.1, 4.2, 4.3, 4.4, 4.5, 4.6, 4.7_

  - [x] 16.2 پیاده‌سازی AboutController و View
    - پیاده‌سازی `AboutController.Index` و `Views/About/Index.cshtml`
    - _Requirements: 2.1, 2.2, 2.3_


- [x] 17. پیاده‌سازی صفحات عمومی - Portfolio
  - [x] 17.1 پیاده‌سازی PortfolioController (عمومی) و Views
    - پیاده‌سازی `PortfolioController`: `Index` (لیست با Pagination و فیلتر Tag)، `Detail(slug)`
    - پیاده‌سازی `PortfolioIndexViewModel` و `PortfolioDetailViewModel`
    - ایجاد `Views/Portfolio/Index.cshtml`: گرید با Lazy Loading، فیلتر تکنولوژی (badge buttons با active state)، Pagination، Empty State، skeleton loading
    - ایجاد `Views/Portfolio/Detail.cshtml`: گالری تصاویر، badge فناوری‌ها، اعضای تیم، لینک‌ها با Font Awesome icons
    - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5, 21.3_


- [x] 18. پیاده‌سازی صفحات عمومی - Blog
  - [x] 18.1 پیاده‌سازی BlogController (عمومی) و Views
    - پیاده‌سازی `BlogController`: `Index` (لیست با Pagination و Category filter)، `Post(slug)`
    - پیاده‌سازی `BlogIndexViewModel` و `BlogPostViewModel`
    - ایجاد `Views/Blog/Index.cshtml`: لیست مقالات با Lazy Loading، فیلتر Category، Pagination، skeleton loading
    - ایجاد `Views/Blog/Post.cshtml`: محتوای کامل با typography مناسب، نظرات تأییدشده، فرم ثبت نظر، مقالات مرتبط
    - فراخوانی `IncrementViewCountAsync` در هر بار بارگذاری صفحه مقاله
    - _Requirements: 6.1, 6.2, 6.3, 6.4, 7.1, 7.2, 7.3, 7.4, 7.5, 7.6, 21.3_


- [x] 19. پیاده‌سازی Search و Contact (عمومی)
  - [x] 19.1 پیاده‌سازی SearchController و View
    - پیاده‌سازی `SearchController.Search(query)` که حداقل 2 کاراکتر نیاز دارد
    - ایجاد `Views/Search/Results.cshtml`: نتایج گروه‌بندی‌شده، Empty State با پیشنهادات
    - _Requirements: 17.1, 17.2, 17.3, 17.4_

  - [x] 19.2 پیاده‌سازی ContactController و View
    - پیاده‌سازی `ContactController`: `Index` (GET)، `Submit` (POST) با Anti-Forgery Token
    - ایجاد `Views/Contact/Index.cshtml`: فرم تماس (split layout)، اطلاعات تماس، Google Map، لینک‌های اجتماعی با Font Awesome
    - Toast Notification برای موفقیت/خطا با slide-in animation
    - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.5, 8.6, 18.5, 20.1_


- [x] 20. Checkpoint — اطمینان از کارکرد صحیح تمام صفحات عمومی و Design System
  - بررسی تمام صفحات در dark و light mode
  - بررسی responsive در 375px، 768px، 1024px، 1440px
  - بررسی Pre-Delivery Checklist از `design-system/team-portfolio/MASTER.md`
  - Ensure all tests pass, ask the user if questions arise.

- [x] 21. پیاده‌سازی Admin Panel - ساختار و Dashboard
  - [x] 21.1 پیکربندی Area مدیریت
    - ایجاد `Areas/Admin` با Controllers، Views و Route Configuration
    - پیاده‌سازی `_AdminLayout.cshtml` با منوی کناری بر اساس نقش کاربر، dark surface colors از Design System
    - اعمال `[Authorize(Roles = "Admin,Manager")]` روی Area
    - _Requirements: 9.9, 10.2, 20.4_

  - [x] 21.2 پیاده‌سازی DashboardController
    - پیاده‌سازی `DashboardController.Index` که آمار: تعداد اعضا، پروژه‌ها، مقالات منتشرشده، نظرات در انتظار، پیام‌های خوانده‌نشده را نمایش می‌دهد
    - کارت‌های آمار با gradient accent، Font Awesome icons
    - _Requirements: 10.1_


- [x] 22. پیاده‌سازی Admin Panel - مدیریت اعضا و پروفایل
  - [x] 22.1 پیاده‌سازی Admin/TeamMembersController
    - پیاده‌سازی CRUD کامل: Index، Create، Edit، Delete
    - اعمال `[Authorize(Roles = "Admin,Manager")]`
    - SweetAlert Confirmation برای Delete
    - _Requirements: 11.1, 11.2, 11.3, 11.4, 11.5_

  - [x] 22.2 پیاده‌سازی Member Profile Controller
    - پیاده‌سازی `ProfileController` برای: ویرایش بیوگرافی، تصویر، Skills، WorkExperience، Education، Social Links، تغییر رمز عبور
    - محدود کردن دسترسی به پروفایل خود
    - _Requirements: 12.1, 12.2, 12.3, 12.4, 12.5_


- [x] 23. پیاده‌سازی Admin Panel - مدیریت Portfolio و Blog
  - [x] 23.1 پیاده‌سازی Admin/PortfolioController
    - پیاده‌سازی CRUD کامل با Image Gallery (حداکثر 10 تصویر) و Team Assignment
    - SweetAlert Confirmation برای Delete
    - فراخوانی Cache Invalidation پس از Publish
    - _Requirements: 13.1, 13.2, 13.3, 13.4, 13.5, 21.2_

  - [x] 23.2 پیاده‌سازی Admin/BlogController
    - پیاده‌سازی CRUD کامل با Rich Text Editor (TinyMCE یا CKEditor 5)، Tag، Category
    - محدود کردن Author به مقالات خودش
    - فراخوانی Cache Invalidation پس از Publish
    - XSS Sanitization روی محتوای HTML
    - _Requirements: 14.1, 14.2, 14.3, 14.4, 14.5, 14.6, 20.7, 21.2_


- [x] 24. پیاده‌سازی Admin Panel - مدیریت Comments، Categories و پیام‌ها
  - [x] 24.1 پیاده‌سازی Admin/CommentsController
    - پیاده‌سازی: لیست نظرات Pending، Approve، Delete (با SweetAlert)، Edit Body
    - _Requirements: 15.1, 15.2, 15.3, 15.4_

  - [x] 24.2 پیاده‌سازی Admin/CategoriesController
    - پیاده‌سازی CRUD با بررسی وجود Blog_Posts مرتبط قبل از حذف
    - _Requirements: 16.1, 16.2, 16.3, 16.4_

  - [x] 24.3 پیاده‌سازی Admin/ContactMessagesController
    - پیاده‌سازی: لیست پیام‌ها (خوانده/نخوانده)، نشانه‌گذاری به عنوان خوانده‌شده، حذف
    - _Requirements: 10.1_


- [x] 25. پیاده‌سازی SEO، Error Pages و Security Headers
  - [x] 25.1 پیاده‌سازی ISeoService
    - پیاده‌سازی `ISeoService` برای تولید Title، Meta Description، Open Graph tags
    - ایجاد `Sitemap.xml` و `robots.txt` endpoint
    - _Requirements: 19.1, 19.2, 19.3, 19.4, 19.5_

  - [x] 25.2 پیاده‌سازی ErrorController و صفحات خطا
    - پیاده‌سازی `ErrorController` با Views برای: 404، 403، 500 — با رنگ‌بندی و استایل Design System
    - پیکربندی `UseExceptionHandler` و `UseStatusCodePagesWithReExecute` در `Program.cs`
    - _Requirements: 22.1, 22.2, 22.3_

  - [x] 25.3 پیکربندی امنیت و Middleware
    - اعمال Anti-Forgery Token در تمام فرم‌ها
    - پیکربندی HTTPS Redirection، HSTS
    - اعتبارسنجی دو طرفه (Client + Server) در تمام فرم‌ها
    - _Requirements: 20.1, 20.2, 20.8_


- [x] 26. اتصال سرویس‌ها در Program.cs و DI Configuration
  - [x] 26.1 پیکربندی Dependency Injection در Program.cs
    - ثبت تمام Repositories، Services، DbContext، Identity، Serilog، Cache، Email در DI Container
    - پیکربندی ConnectionString، Role Seeding اولیه
    - _Requirements: 9.3, 20.3, 20.4_

- [x] 27. Final Checkpoint — اطمینان از کارکرد کامل سیستم و Design System
  - بررسی Pre-Delivery Checklist کامل از `design-system/team-portfolio/MASTER.md`
  - بررسی تمام صفحات در dark/light mode و breakpoints مختلف
  - Ensure all tests pass, ask the user if questions arise.


---

## Notes

- تسک‌های ستاره‌دار (`*`) اختیاری هستند و برای MVP سریع‌تر می‌توانند در ابتدا رد شوند
- هر تسک به Requirements مشخص ارجاع دارد تا Traceability حفظ شود
- Checkpoints در نقاط کلیدی برای اطمینان از صحت تدریجی گنجانده شده‌اند
- Property Tests خواص جهانی سیستم را با FsCheck.Xunit روی ورودی‌های تصادفی اعتبارسنجی می‌کنند
- Unit Tests مثال‌های مشخص و حالات مرزی را پوشش می‌دهند
- تمام فرم‌ها باید هم Client-side (jQuery Unobtrusive Validation) و هم Server-side validation داشته باشند


## Task Dependency Graph

```json
{
  "waves": [
    { "id": 0, "tasks": ["2.1"] },
    { "id": 1, "tasks": ["2.2"] },
    { "id": 2, "tasks": ["3.1", "4.1", "4.2"] },
    { "id": 3, "tasks": ["3.2", "5.1", "5.4", "5.7"] },
    { "id": 4, "tasks": ["5.2", "5.3", "5.5", "5.6", "6.1"] },
    { "id": 5, "tasks": ["6.2", "8.1", "8.3", "8.6", "9.1", "9.4", "10.1"] },
    { "id": 6, "tasks": ["8.2", "8.4", "8.5", "9.2", "9.3", "9.5", "9.6", "10.2", "11.1", "11.3", "11.5", "11.7"] },
    { "id": 7, "tasks": ["11.2", "11.4", "11.6", "13.1"] },
    { "id": 8, "tasks": ["13.2", "13.3", "13.4", "14.1", "14.2", "14.3", "14.4", "14.5"] },
    { "id": 9, "tasks": ["15.1", "15.2", "15.3", "16.1", "16.2", "21.1", "21.2"] },
    { "id": 10, "tasks": ["17.1", "18.1", "19.1", "19.2", "22.1", "22.2", "23.1", "23.2", "23.3"] },
    { "id": 11, "tasks": ["25.1", "25.2", "25.3"] },
    { "id": 12, "tasks": ["26.1"] }
  ]
}
```
