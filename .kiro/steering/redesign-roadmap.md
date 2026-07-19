---
inclusion: auto
---

# Starry Night Visual Redesign — Roadmap

این فایل roadmap کامل بازطراحی بصری سایت Team Portfolio رو نگه می‌داره.
در هر سشن جدید، این فایل به عنوان مرجع وضعیت پارت‌ها و scope هر spec استفاده می‌شه.

---

## پروژه‌ی پایه

**سایت:** Team Portfolio Website  
**Stack:** ASP.NET Core 9 MVC · Entity Framework Core · SQL Server · Razor Views  
**وضعیت functionality:** ✅ کامل — تمام کنترلرها، سرویس‌ها، ریپوزیتوری‌ها، و ویوها پیاده‌سازی شده‌اند  
**Spec مرجع functionality:** `.kiro/specs/team-portfolio-website/` (requirements + design + tasks — تمام‌شده)

---

## بازطراحی بصری — 4-Part Plan

### ✅ Part 1 — Design System & Foundation
**Spec:** `.kiro/specs/starry-night-design-system/`  
**وضعیت:** Requirements ✅ · Design ✅ · Tasks ⏳

**Scope:**
- `design-system.css` — تمام CSS tokens (رنگ، تایپوگرافی، فاصله، radius، انیمیشن، glassmorphism)
- `background-system.css` + `background.js` — سیستم پس‌زمینه ۸ لایه‌ای انیمیت‌شده (ستاره، ابر، کهکشان، ذرات، ...)
- `navigation.css` — ناوبار floating glassmorphism
- `hero.css` — سکشن hero تمام‌صفحه
- `motion.css` + `motion.js` — سیستم motion (scroll reveal، parallax، magnetic button، card tilt)
- `_Layout.cshtml` — اضافه کردن skip-nav، background container، لینک‌های CSS/JS جدید
- `_Navigation.cshtml` — partial جدید برای nav
- `_HeroSection.cshtml` — partial جدید فقط در Home/Index

**وابستگی:** هیچ — این فونداسیون همه پارت‌های بعدیه

---

### ⏳ Part 2 — Interior Pages Redesign
**Spec:** ساخته نشده (باید با `starry-night-interior-pages` نام‌گذاری بشه)  
**وضعیت:** Pending — بعد از تکمیل Part 1

**Scope:**
- **Team Members page** (`/Team`) — کارت‌های glassmorphism برای هر عضو، skill badges با گلو، hover 3D tilt
- **Portfolio page** (`/Portfolio`) — گرید پروژه‌ها با فیلتر animated، کارت‌های glassmorphism، modal یا detail page
- **Blog page** (`/Blog`) — لیست پست‌ها، کارت‌های glassmorphism، pagination styled، search bar
- **Blog Detail page** (`/Blog/{slug}`) — typography مناسب برای خواندن، sidebar, کامنت‌ها
- **Contact page** (`/Contact`) — فرم glassmorphism، validation states animated
- **Section headings** — همه `<h2>` ها با scroll reveal و gradient text
- **Footer** — glassmorphism footer با social links

**وابستگی:** Part 1 باید merge شده باشه (design tokens در دسترس باشن)

---

### ⏳ Part 3 — Admin Panel Redesign
**Spec:** ساخته نشده (باید با `starry-night-admin` نام‌گذاری بشه)  
**وضعیت:** Pending — بعد از تکمیل Part 2

**Scope:**
- **Admin layout** (`/Admin`) — sidebar glassmorphism، dark nav با همون palette
- **Dashboard** — کارت‌های stats با glow، چارت‌های styled
- **Blog management** — جدول‌ها با glassmorphism، دکمه‌های styled، rich text editor themed
- **Portfolio management** — image uploader styled، form‌های glassmorphism
- **Team member management** — skill editor styled، پروفایل avatar uploader
- **Contact messages** — inbox styled با read/unread states
- **Settings pages** — فرم‌های consistency با design system

**وابستگی:** Part 1 (tokens) و Part 2 (component patterns)

---

### ⏳ Part 4 — Polish & Advanced Interactions
**Spec:** ساخته نشده (باید با `starry-night-polish` نام‌گذاری بشه)  
**وضعیت:** Pending — بعد از تکمیل Part 3

**Scope:**
- **Page transitions** — GSAP-driven transition بین صفحات (fade + slide)
- **Loading states** — skeleton loaders با shimmer animation در glassmorphism style
- **Cursor custom** — cursor سفارشی با glow trail (اختیاری، فقط desktop)
- **Aurora background per-page** — تنوع رنگی background بر اساس صفحه (مثلاً Portfolio با رنگ متفاوت)
- **Performance audit** — بهینه‌سازی LCP، CLS، FID با Lighthouse
- **Cross-browser polish** — تست Safari (backdrop-filter)، Firefox، edge cases
- **Print styles** — حذف background و animations برای چاپ
- **Error pages** — ۴۰۴ و ۵۰۰ pages با همون aesthetic

**وابستگی:** هر سه پارت قبلی باید کامل باشن

---

## Design Language Reference

**نام تم:** Starry Night × Apple × Stripe × Linear × Vercel × Awwwards  
**مود:** Dark-only (بدون light mode در این redesign)  
**Palette اصلی:** Navy (#07152E) · Cyan (#00D4FF) · Gold (#F5C842) · Purple (#8B5CF6) · Star White (#F0F4FF)  
**Fonts:** Space Grotesk (heading) · DM Sans (body) · JetBrains Mono (code)  
**Design System Source of Truth:** `wwwroot/css/design-system.css` (بعد از Part 1)

---

## قوانین برای سشن‌های بعدی

1. **هر پارت spec جداگانه دارد** — هیچ‌وقت scope یک پارت رو به spec پارت دیگه اضافه نکن
2. **Part 1 باید implement و merge بشه قبل از شروع Part 2** — tokens باید در دسترس باشن
3. **همه کامپوننت‌های جدید از `design-system.css` tokens استفاده می‌کنن** — هیچ hex hardcode نمی‌شه
4. **Razor views موجود (کنترلرها، مدل‌ها، روت‌ها) دست نمی‌خوره** — فقط visual layer تغییر می‌کنه
5. **هر spec باید با `starry-night-` prefix نام‌گذاری بشه** تا مشخص باشه بخشی از این redesign هست
