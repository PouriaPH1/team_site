# TeamPortfolio

A full-featured team portfolio web application built with ASP.NET Core 9 MVC. Showcases team members, projects, and a blog — with a complete admin panel for content management.

---

## Features

- **Home** — Starry-night hero section with animated canvas background
- **Team** — Member profiles with photos, skills, and social links
- **Portfolio** — Project showcase with image gallery and filtering
- **Blog** — Full blog with categories, tags, comments, and rich content
- **Search** — Site-wide search across all content
- **Contact** — Contact form with message management
- **Admin Panel** — Complete CRUD for all content types
- **Authentication** — ASP.NET Identity with role-based access (Admin, Manager, Author, Member)
- **SEO** — Sitemap, meta tags, and Open Graph support

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 9 MVC |
| Language | C# 13 |
| Database | SQLite + Entity Framework Core 9 |
| Auth | ASP.NET Identity |
| Frontend | Bootstrap 5, Vanilla JS |
| Icons | Font Awesome 6 |
| Architecture | Clean Architecture (Domain / Application / Infrastructure / Web) |

---

## Project Structure

```
team_site/
├── src/
│   ├── TeamPortfolio.Domain/          # Entities, Enums, base classes
│   ├── TeamPortfolio.Application/     # DTOs, Interfaces, Services
│   ├── TeamPortfolio.Infrastructure/  # EF Core, Repositories, Migrations, Identity
│   └── TeamPortfolio.Web/             # MVC Controllers, Views, wwwroot
│       ├── Areas/Admin/               # Admin panel (dashboard, CRUD)
│       ├── Controllers/               # Public-facing controllers
│       ├── Views/                     # Razor views & layouts
│       └── wwwroot/                   # Static assets (CSS, JS, images)
├── DEPLOY.fa.md                       # راهنمای دیپلوی (فارسی)
├── DEPLOY.en.md                       # Deployment guide (English)
└── TeamPortfolio.sln
```

---

## Getting Started (Local Development)

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Git

### Run locally

```bash
git clone https://github.com/YOUR_USERNAME/team_site.git
cd team_site

# Restore & run
dotnet run --project src/TeamPortfolio.Web/TeamPortfolio.Web.csproj
```

The app starts at `https://localhost:5001` (or the port shown in the terminal).

### Database

SQLite database is created automatically on first run via EF Core migrations. No setup needed.

### Default Admin Account

| Field | Value |
|-------|-------|
| Email | `admin@teamportfolio.dev` |
| Password | `Admin@1234` |

> **Change this password immediately after first login.**

---

## Deployment

Full step-by-step deployment guides are included:

- 🇮🇷 **[DEPLOY.fa.md](./DEPLOY.fa.md)** — راهنمای کامل دیپلوی به فارسی
- 🇬🇧 **[DEPLOY.en.md](./DEPLOY.en.md)** — Complete deployment guide in English

### Summary (Ubuntu 22.04 + Nginx + Cloudflare)

```bash
# 1. Clone on server
git clone git@github.com:YOUR_USERNAME/team_site.git

# 2. Build
dotnet publish src/TeamPortfolio.Web/TeamPortfolio.Web.csproj -c Release -o /var/www/teamportfolio-app

# 3. Run migrations
dotnet ef database update ...

# 4. Start with systemd
systemctl start teamportfolio
```

---

## Environment Configuration

The following files are **not committed** to git and must be created manually on the server:

| File | Purpose |
|------|---------|
| `appsettings.Production.json` | DB connection string, AllowedHosts |
| `wwwroot/uploads/` | User-uploaded images |
| `data/teamportfolio.db` | SQLite database |

---

## Roles & Permissions

| Role | Access |
|------|--------|
| Admin | Full access to everything |
| Manager | Content management, no user management |
| Author | Create/edit own blog posts |
| Member | Read-only authenticated access |

---

## License

This project is private. All rights reserved.
