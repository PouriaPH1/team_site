# Requirements Document

## Introduction

این پروژه یک وب‌سایت Portfolio مدرن و حرفه‌ای برای معرفی یک تیم برنامه‌نویسی است. سایت شامل معرفی اعضا، نمایش نمونه‌کارها، وبلاگ فنی، سیستم ارتباط با کاربران و یک پنل مدیریت کامل می‌باشد. این سیستم با ASP.NET Core 9 MVC، Entity Framework Core، SQL Server و Bootstrap 5 پیاده‌سازی می‌شود.

---

## Glossary

- **Website**: وب‌سایت Portfolio تیم برنامه‌نویسی
- **Admin**: کاربر با بالاترین سطح دسترسی که می‌تواند تمام بخش‌های سیستم را مدیریت کند
- **Manager**: کاربر با دسترسی مدیریت محتوا و اعضا
- **Author**: کاربر که توانایی نوشتن و مدیریت مقالات خود را دارد
- **Member**: عضو تیم با دسترسی ویرایش پروفایل شخصی
- **Visitor**: کاربر عمومی بدون احراز هویت که محتوای عمومی سایت را مشاهده می‌کند
- **Portfolio_Item**: یک پروژه نمونه‌کار که شامل توضیحات، تکنولوژی‌ها، تصاویر و لینک‌ها است
- **Blog_Post**: مقاله فنی منتشرشده در وبلاگ سایت
- **Team_Member**: عضوی از تیم برنامه‌نویسی که پروفایل عمومی در سایت دارد
- **Comment**: نظر کاربران در مقالات وبلاگ
- **Tag**: برچسب موضوعی برای دسته‌بندی مقالات و پروژه‌ها
- **Category**: دسته‌بندی اصلی برای مقالات وبلاگ
- **Skill**: مهارت فنی عضو تیم با سطح تسلط مشخص
- **Contact_Form**: فرم ارسال پیام توسط بازدیدکننده
- **Admin_Panel**: بخش مدیریت سایت که فقط برای کاربران مجاز قابل دسترسی است
- **Rich_Text_Editor**: ویرایشگر متن غنی برای نوشتن محتوای مقالات
- **Image_Uploader**: سرویس آپلود و پردازش تصاویر
- **Search_Engine**: موتور جستجوی داخلی سایت
- **SEO_Service**: سرویس مدیریت متادیتا، Sitemap و URL های بهینه
- **Cache_Service**: سرویس کش برای بهبود عملکرد
- **Auth_Service**: سرویس احراز هویت مبتنی بر ASP.NET Identity
- **Notification_Service**: سرویس نمایش Toast Notification و SweetAlert

---

## Requirements

### Requirement 1: صفحات عمومی - صفحه اصلی

**User Story:** As a Visitor, I want to see a professional and modern home page, so that I can quickly understand what the team does and navigate to relevant sections.

#### Acceptance Criteria

1. THE Website SHALL display a Hero Section with team name, tagline, and call-to-action buttons on the home page
2. THE Website SHALL display the latest 6 Portfolio_Items on the home page
3. THE Website SHALL display the latest 3 Blog_Posts on the home page
4. THE Website SHALL display team statistics including total number of projects, team members, technologies used, and years of experience on the home page
5. THE Website SHALL display a list of technologies used by the team on the home page
6. THE Website SHALL display a testimonials/reviews section on the home page
7. THE Website SHALL display a Contact_Form on the home page
8. THE Website SHALL display a Footer with social media links, navigation links, and contact information on all pages

### Requirement 2: صفحه درباره ما

**User Story:** As a Visitor, I want to learn about the team's background, goals, and history, so that I can decide whether to engage with the team.

#### Acceptance Criteria

1. THE Website SHALL display a complete team description, mission statement, and goals on the About page
2. THE Website SHALL display the team's history and founding story on the About page
3. THE Website SHALL display a summary of core skills and technologies on the About page

---

### Requirement 3: صفحه اعضای تیم

**User Story:** As a Visitor, I want to browse team members in a visual grid, so that I can find and learn about individual contributors.

#### Acceptance Criteria

1. THE Website SHALL display all active Team_Members in a responsive grid layout on the Team page
2. WHEN displaying a Team_Member card, THE Website SHALL show profile photo, name, role/position, short biography, specializations, and social media links
3. WHEN a Visitor clicks the profile button on a Team_Member card, THE Website SHALL navigate to that member's Profile page
4. THE Website SHALL display a search input on the Team page that filters visible Team_Member cards by name or specialization

---

### Requirement 4: صفحه پروفایل عضو تیم

**User Story:** As a Visitor, I want to see a detailed profile page for each team member, so that I can learn about their skills, experience, and work.

#### Acceptance Criteria

1. THE Website SHALL display a profile banner, profile photo, full name, and role on the Member Profile page
2. THE Website SHALL display a full biography, resume download link, and skills with proficiency levels on the Member Profile page
3. THE Website SHALL display work experience history with company name, role, start date, and end date on the Member Profile page
4. THE Website SHALL display education history with institution name, degree, field of study, start date, and end date on the Member Profile page
5. THE Website SHALL display Portfolio_Items associated with the Team_Member on the Member Profile page
6. THE Website SHALL display Blog_Posts authored by the Team_Member on the Member Profile page
7. THE Website SHALL display social links including GitHub, LinkedIn, Telegram, email, and phone number on the Member Profile page

---

### Requirement 5: صفحه نمونه‌کارها (Portfolio)

**User Story:** As a Visitor, I want to browse and filter the team's projects, so that I can evaluate the team's technical capabilities and work quality.

#### Acceptance Criteria

1. THE Website SHALL display all published Portfolio_Items with cover image, name, short description, technologies used, and date on the Portfolio page
2. THE Website SHALL support filtering Portfolio_Items by technology tag on the Portfolio page
3. WHEN a Visitor clicks on a Portfolio_Item, THE Website SHALL display a detail page showing full description, technology list, team members involved, GitHub link, live demo link, and an image gallery
4. THE Website SHALL support pagination with 9 Portfolio_Items per page on the Portfolio listing page
5. WHEN no Portfolio_Items match the selected filter, THE Website SHALL display a descriptive empty-state message

---

### Requirement 6: وبلاگ - لیست مقالات

**User Story:** As a Visitor, I want to browse published blog posts, so that I can read technical articles and stay updated on the team's knowledge.

#### Acceptance Criteria

1. THE Website SHALL display all published Blog_Posts with cover image, title, short description, author name, publish date, Category, view count, and comment count on the Blog listing page
2. THE Website SHALL support filtering Blog_Posts by Category on the Blog listing page
3. THE Website SHALL support pagination with 10 Blog_Posts per page on the Blog listing page
4. WHEN a Visitor submits a search query, THE Search_Engine SHALL return Blog_Posts whose title or content matches the query

---

### Requirement 7: صفحه مقاله

**User Story:** As a Visitor, I want to read a full blog post with related content and commenting, so that I can engage with the team's technical content.

#### Acceptance Criteria

1. THE Website SHALL display the full title, cover image, body content, Tags, author information, and publish date on the Blog Post detail page
2. THE Website SHALL increment the view count of a Blog_Post by 1 each time the Blog Post detail page is loaded
3. THE Website SHALL display up to 3 related Blog_Posts based on shared Tags or Category on the Blog Post detail page
4. THE Website SHALL display approved Comments on the Blog Post detail page
5. WHEN a Visitor submits a Comment with a non-empty name, valid email address, and non-empty message body, THE Website SHALL save the Comment with a pending approval status and display a confirmation message
6. IF a Visitor submits a Comment with an empty name, invalid email, or empty message body, THEN THE Website SHALL display a validation error message without saving the Comment

---

### Requirement 8: صفحه تماس با ما

**User Story:** As a Visitor, I want to send a message to the team through a contact form, so that I can inquire about collaboration or services.

#### Acceptance Criteria

1. THE Website SHALL display a contact form with fields for full name, email address, subject, and message body on the Contact page
2. THE Website SHALL display team contact information including address, email, and phone number on the Contact page
3. THE Website SHALL display an embedded Google Map on the Contact page
4. THE Website SHALL display social media links on the Contact page
5. WHEN a Visitor submits the contact form with a non-empty full name, valid email address, non-empty subject, and non-empty message body, THE Website SHALL save the Contact_Form submission and display a success notification
6. IF a Visitor submits the contact form with missing required fields or an invalid email format, THEN THE Website SHALL display field-level validation error messages without saving the submission

---

### Requirement 9: احراز هویت

**User Story:** As a registered user, I want to sign up, log in, reset my password, and verify my email, so that I can securely access my account and the Admin_Panel.

#### Acceptance Criteria

1. THE Auth_Service SHALL allow a new user to register with a unique email address and a password that meets minimum complexity requirements of at least 8 characters containing a letter and a number
2. WHEN a new user completes registration, THE Auth_Service SHALL send an email verification link to the provided email address
3. WHEN a user clicks a valid email verification link, THE Auth_Service SHALL mark the account as email-verified
4. WHEN a verified user provides correct email and password credentials, THE Auth_Service SHALL issue an authenticated session
5. WHEN a user requests a password reset, THE Auth_Service SHALL send a password reset link to the registered email address that expires after 24 hours
6. WHEN a user submits a valid password reset link with a new password, THE Auth_Service SHALL update the account password and invalidate the used reset token
7. THE Auth_Service SHALL support a "Remember Me" option that maintains the authenticated session for 30 days
8. WHEN an authenticated user logs out, THE Auth_Service SHALL invalidate the session immediately
9. IF an unauthenticated user attempts to access an Admin_Panel page, THEN THE Auth_Service SHALL redirect the user to the login page

---

### Requirement 10: پنل مدیریت - داشبورد

**User Story:** As an Admin or Manager, I want to see a dashboard with key statistics, so that I can monitor the website's content and activity at a glance.

#### Acceptance Criteria

1. WHILE an Admin or Manager is authenticated, THE Admin_Panel SHALL display a dashboard with total counts of Team_Members, Portfolio_Items, published Blog_Posts, pending Comments, and unread Contact_Form submissions
2. THE Admin_Panel SHALL display a navigation menu that shows only the sections the authenticated user's role has permission to access

---

### Requirement 11: مدیریت اعضای تیم

**User Story:** As an Admin or Manager, I want to perform full CRUD operations on team members, so that I can keep the team listing accurate and up to date.

#### Acceptance Criteria

1. WHILE an Admin or Manager is authenticated, THE Admin_Panel SHALL allow creating a new Team_Member with first name, last name, role, biography, and an optional profile photo
2. WHEN an Admin or Manager uploads a profile photo for a Team_Member, THE Image_Uploader SHALL accept only JPEG, PNG, or WebP files with a maximum size of 5 MB
3. IF an Admin or Manager uploads a file that is not JPEG, PNG, or WebP or exceeds 5 MB, THEN THE Image_Uploader SHALL reject the file and display a descriptive error message
4. WHILE an Admin or Manager is authenticated, THE Admin_Panel SHALL allow editing all fields of an existing Team_Member record
5. WHILE an Admin or Manager is authenticated, THE Admin_Panel SHALL allow deleting a Team_Member record after explicit confirmation

---

### Requirement 12: مدیریت پروفایل شخصی

**User Story:** As a Member, I want to edit my own profile information, so that my public profile stays current and accurate.

#### Acceptance Criteria

1. WHILE a Member is authenticated, THE Admin_Panel SHALL allow the Member to edit their own biography, profile photo, banner photo, skills, work experience, and education history
2. WHEN a Member updates their profile photo or banner, THE Image_Uploader SHALL apply the same file type and size restrictions defined in Requirement 11
3. WHILE a Member is authenticated, THE Admin_Panel SHALL allow the Member to add, edit, or remove Skill entries with a name and proficiency level between 1 and 100
4. WHILE a Member is authenticated, THE Admin_Panel SHALL allow the Member to update their social links including GitHub, LinkedIn, Telegram, email, and phone number
5. WHILE a Member is authenticated, THE Admin_Panel SHALL allow the Member to change their account password by providing the current password and a new password that meets complexity requirements

---

### Requirement 13: مدیریت پروژه‌ها (Portfolio)

**User Story:** As an Admin or Manager, I want to manage portfolio projects with full CRUD, image gallery, and team assignment, so that the portfolio section always reflects the team's latest work.

#### Acceptance Criteria

1. WHILE an Admin or Manager is authenticated, THE Admin_Panel SHALL allow creating a Portfolio_Item with title, description, technologies, start date, GitHub URL, demo URL, and at least one cover image
2. WHEN creating or editing a Portfolio_Item, THE Admin_Panel SHALL allow selecting one or more Team_Members to associate with the project
3. WHEN an Admin or Manager uploads images for a Portfolio_Item, THE Image_Uploader SHALL accept only JPEG, PNG, or WebP files with a maximum size of 5 MB each and a maximum of 10 images per Portfolio_Item
4. WHILE an Admin or Manager is authenticated, THE Admin_Panel SHALL allow editing all fields of an existing Portfolio_Item
5. WHILE an Admin or Manager is authenticated, THE Admin_Panel SHALL allow deleting a Portfolio_Item after explicit confirmation

---

### Requirement 14: مدیریت وبلاگ

**User Story:** As an Author or Admin, I want to write, edit, publish, and manage blog posts with rich text, so that I can publish technical articles for the team's audience.

#### Acceptance Criteria

1. WHILE an Author, Manager, or Admin is authenticated, THE Admin_Panel SHALL allow creating a new Blog_Post with title, cover image, body content via Rich_Text_Editor, Tags, Category, and a status of draft or published
2. WHEN an Author or Admin saves a Blog_Post as draft, THE Admin_Panel SHALL store the post without making it visible on the public Blog listing page
3. WHEN an Author or Admin publishes a Blog_Post, THE Admin_Panel SHALL set the publish date to the current date and time and make the post visible on the public Blog listing page
4. WHILE an Author is authenticated, THE Admin_Panel SHALL restrict the Author to editing and deleting only their own Blog_Posts
5. WHILE an Admin or Manager is authenticated, THE Admin_Panel SHALL allow editing or deleting any Blog_Post regardless of authorship
6. THE Rich_Text_Editor SHALL support text formatting including headings, bold, italic, bulleted lists, numbered lists, code blocks, and inline images

---

### Requirement 15: مدیریت کامنت‌ها

**User Story:** As an Admin or Manager, I want to review, approve, and delete user comments, so that only appropriate content is displayed publicly.

#### Acceptance Criteria

1. WHILE an Admin or Manager is authenticated, THE Admin_Panel SHALL display a list of all pending Comments with commenter name, email, message preview, and the associated Blog_Post title
2. WHEN an Admin or Manager approves a Comment, THE Admin_Panel SHALL set the Comment status to approved and make it visible on the public Blog Post detail page
3. WHEN an Admin or Manager deletes a Comment, THE Admin_Panel SHALL permanently remove the Comment after explicit confirmation
4. WHILE an Admin or Manager is authenticated, THE Admin_Panel SHALL allow editing the body of an existing Comment

---

### Requirement 16: مدیریت دسته‌بندی‌ها

**User Story:** As an Admin or Manager, I want to create and manage blog categories, so that blog posts are organized for easy navigation.

#### Acceptance Criteria

1. WHILE an Admin or Manager is authenticated, THE Admin_Panel SHALL allow creating a new Category with a unique name and an optional description
2. WHILE an Admin or Manager is authenticated, THE Admin_Panel SHALL allow renaming an existing Category
3. WHEN an Admin or Manager attempts to delete a Category that has associated Blog_Posts, THE Admin_Panel SHALL prevent deletion and display an error message indicating the number of associated Blog_Posts
4. WHEN an Admin or Manager deletes a Category with no associated Blog_Posts, THE Admin_Panel SHALL permanently remove the Category after explicit confirmation

---

### Requirement 17: جستجو

**User Story:** As a Visitor, I want to search for members, projects, and articles across the site, so that I can quickly find the content I'm looking for.

#### Acceptance Criteria

1. WHEN a Visitor enters a search query of at least 2 characters, THE Search_Engine SHALL return matching Team_Members, Portfolio_Items, and Blog_Posts within 500ms
2. THE Search_Engine SHALL match search queries against Team_Member names and specializations, Portfolio_Item titles and descriptions, and Blog_Post titles and content
3. WHEN the Search_Engine returns results, THE Website SHALL display results grouped by type (Members, Projects, Articles) with a count per group
4. WHEN the Search_Engine finds no results for a query, THE Website SHALL display a descriptive no-results message with suggested alternative search terms

---

### Requirement 18: رابط کاربری و تجربه کاربری

**User Story:** As a Visitor, I want a visually modern, responsive, and accessible interface with dark/light mode, so that I have a pleasant experience on any device.

#### Acceptance Criteria

1. THE Website SHALL render correctly and maintain full usability on viewport widths from 320px to 2560px using a mobile-first responsive layout
2. THE Website SHALL support switching between Dark Mode and Light Mode, and THE Website SHALL persist the user's preference in browser local storage
3. THE Website SHALL display smooth CSS transition animations for card hover effects, page section reveals, and modal dialogs
4. THE Website SHALL display a loading animation while asynchronous data is being fetched
5. THE Notification_Service SHALL display non-blocking Toast Notifications for successful form submissions, save actions, and error events
6. THE Notification_Service SHALL display SweetAlert confirmation dialogs before executing destructive actions such as delete operations in the Admin_Panel
7. THE Website SHALL display a Breadcrumb navigation component on all pages except the home page

---

### Requirement 19: SEO و متادیتا

**User Story:** As a site owner, I want search-engine-friendly URLs, meta tags, and a sitemap, so that the website ranks well in search results.

#### Acceptance Criteria

1. THE SEO_Service SHALL generate a unique and descriptive HTML title tag and meta description for each public page
2. THE SEO_Service SHALL include Open Graph meta tags with title, description, and cover image on all Blog_Post and Portfolio_Item detail pages
3. THE SEO_Service SHALL generate a valid XML sitemap at /sitemap.xml containing all public pages, Portfolio_Items, and Blog_Posts
4. THE SEO_Service SHALL serve a robots.txt file at /robots.txt that allows indexing of public pages and disallows indexing of Admin_Panel pages
5. THE Website SHALL use human-readable slug-based URLs for Blog_Post and Portfolio_Item detail pages derived from their titles

---

### Requirement 20: امنیت

**User Story:** As a site owner, I want the application to be protected against common web vulnerabilities, so that user data and system integrity are maintained.

#### Acceptance Criteria

1. THE Website SHALL include an Anti-Forgery Token in all HTML forms and THE Website SHALL validate the token on form submission
2. THE Website SHALL validate all user inputs on both the client side and the server side before processing
3. THE Auth_Service SHALL store all passwords as salted cryptographic hashes using ASP.NET Identity's default password hashing algorithm
4. THE Website SHALL enforce Role Based Authorization so that each Admin_Panel action is accessible only to users with the required role
5. THE Image_Uploader SHALL validate uploaded file MIME type and extension on the server side and SHALL reject files that do not match an allowed type
6. THE Image_Uploader SHALL enforce a maximum file size of 5 MB per upload and SHALL reject files that exceed this limit
7. THE Website SHALL sanitize all user-generated content rendered as HTML to prevent Cross-Site Scripting attacks
8. THE Website SHALL use parameterized queries through Entity Framework Core for all database operations to prevent SQL Injection attacks

---

### Requirement 21: عملکرد و کش

**User Story:** As a Visitor, I want pages to load quickly, so that I have a smooth browsing experience.

#### Acceptance Criteria

1. THE Cache_Service SHALL cache the home page content including team statistics, latest Portfolio_Items, and latest Blog_Posts for 10 minutes
2. THE Cache_Service SHALL invalidate the cached home page content when a new Portfolio_Item or Blog_Post is published
3. THE Website SHALL support lazy loading for images in the Portfolio grid and Blog listing pages
4. THE Website SHALL implement pagination on all listing pages to limit the number of database records fetched per request

---

### Requirement 22: صفحات خطا

**User Story:** As a Visitor, I want to see friendly error pages when something goes wrong, so that I understand what happened and can navigate back to the site.

#### Acceptance Criteria

1. WHEN a Visitor navigates to a URL that does not exist, THE Website SHALL display a custom 404 error page with a link to the home page
2. WHEN an unhandled server error occurs, THE Website SHALL display a custom 500 error page with a link to the home page and SHALL log the error details to the server error log
3. THE Website SHALL NOT expose stack traces or internal error details to the Visitor on error pages

---

### Requirement 23: طراحی دیتابیس

**User Story:** As a developer, I want a well-structured relational database schema, so that data integrity is maintained and queries are efficient.

#### Acceptance Criteria

1. THE Website SHALL use Entity Framework Core Code First Migrations to create and version the database schema
2. THE Website's database schema SHALL define a Team_Members table with columns for Id, FirstName, LastName, Role, Biography, ProfilePhotoPath, BannerPhotoPath, IsActive, and CreatedAt
3. THE Website's database schema SHALL define a Portfolio_Items table with columns for Id, Title, Slug, Description, Technologies, StartDate, GitHubUrl, DemoUrl, CoverImagePath, and IsPublished
4. THE Website's database schema SHALL define a Blog_Posts table with columns for Id, Title, Slug, Body, CoverImagePath, Status, ViewCount, PublishDate, AuthorId (FK to Team_Members), and CategoryId (FK to Categories)
5. THE Website's database schema SHALL define a Comments table with columns for Id, CommenterName, CommenterEmail, Body, Status, CreatedAt, and PostId (FK to Blog_Posts)
6. THE Website's database schema SHALL define a Categories table with columns for Id, Name, and Description
7. THE Website's database schema SHALL define a Tags table and a many-to-many relationship between Tags and Blog_Posts
8. THE Website's database schema SHALL define a Skills table with columns for Id, Name, ProficiencyLevel (integer 1–100), and MemberId (FK to Team_Members)
9. THE Website's database schema SHALL define a WorkExperiences table with columns for Id, CompanyName, Role, StartDate, EndDate (nullable), Description, and MemberId (FK to Team_Members)
10. THE Website's database schema SHALL define an Educations table with columns for Id, InstitutionName, Degree, FieldOfStudy, StartDate, EndDate (nullable), and MemberId (FK to Team_Members)
11. THE Website's database schema SHALL define a PortfolioItemMembers join table to represent the many-to-many relationship between Portfolio_Items and Team_Members
12. THE Website's database schema SHALL define a ContactMessages table with columns for Id, FullName, Email, Subject, Body, IsRead, and CreatedAt
