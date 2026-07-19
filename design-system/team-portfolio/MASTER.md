# Design System Master File — Team Portfolio Website

> **LOGIC:** When building a specific page, first check `design-system/pages/[page-name].md`.
> If that file exists, its rules **override** this Master file.
> If not, strictly follow the rules below.

---

**Project:** Team Portfolio Website  
**Style Inspiration:** Vercel, Linear, GitHub, Stripe, Microsoft Developer  
**Architecture:** ASP.NET Core 9 MVC + Bootstrap 5  
**Approach:** Mobile-First, Dark/Light Mode, Clean Minimal Professional

---

## 1. Color Palette

### Dark Mode (Default)

| Role | Hex | CSS Variable | Usage |
|------|-----|--------------|-------|
| Background | `#0A0F1E` | `--color-bg` | Page background |
| Surface | `#111827` | `--color-surface` | Cards, panels |
| Surface Raised | `#1A2235` | `--color-surface-raised` | Elevated cards, navbar |
| Border | `#1E2D40` | `--color-border` | Dividers, card borders |
| Border Subtle | `#162032` | `--color-border-subtle` | Subtle separators |
| Primary | `#3B82F6` | `--color-primary` | Main blue — buttons, links |
| Primary Hover | `#2563EB` | `--color-primary-hover` | Hover state of primary |
| Accent | `#8B5CF6` | `--color-accent` | Purple accent — badges, highlights |
| Accent Hover | `#7C3AED` | `--color-accent-hover` | Hover state of accent |
| Gradient Start | `#3B82F6` | — | Gradient: blue → purple |
| Gradient End | `#8B5CF6` | — | Gradient: blue → purple |
| Text Primary | `#F9FAFB` | `--color-text` | Headings, body text |
| Text Secondary | `#9CA3AF` | `--color-text-secondary` | Subtext, descriptions |
| Text Muted | `#6B7280` | `--color-text-muted` | Placeholders, meta |
| Success | `#10B981` | `--color-success` | Positive states |
| Warning | `#F59E0B` | `--color-warning` | Caution states |
| Danger | `#EF4444` | `--color-danger` | Error states |

### Light Mode

| Role | Hex | CSS Variable | Usage |
|------|-----|--------------|-------|
| Background | `#F8FAFC` | `--color-bg` | Page background |
| Surface | `#FFFFFF` | `--color-surface` | Cards, panels |
| Surface Raised | `#F1F5F9` | `--color-surface-raised` | Elevated cards |
| Border | `#E2E8F0` | `--color-border` | Dividers, card borders |
| Border Subtle | `#F1F5F9` | `--color-border-subtle` | Subtle separators |
| Primary | `#2563EB` | `--color-primary` | Main blue |
| Accent | `#7C3AED` | `--color-accent` | Purple accent |
| Text Primary | `#0F172A` | `--color-text` | Headings, body (slate-900) |
| Text Secondary | `#475569` | `--color-text-secondary` | Subtext (slate-600 minimum) |
| Text Muted | `#94A3B8` | `--color-text-muted` | Placeholders |

### CSS Variables Definition

```css
:root {
  /* Dark Mode (default) */
  --color-bg: #0A0F1E;
  --color-surface: #111827;
  --color-surface-raised: #1A2235;
  --color-border: #1E2D40;
  --color-border-subtle: #162032;
  --color-primary: #3B82F6;
  --color-primary-hover: #2563EB;
  --color-accent: #8B5CF6;
  --color-accent-hover: #7C3AED;
  --color-text: #F9FAFB;
  --color-text-secondary: #9CA3AF;
  --color-text-muted: #6B7280;
  --color-success: #10B981;
  --color-warning: #F59E0B;
  --color-danger: #EF4444;
  --gradient-primary: linear-gradient(135deg, #3B82F6, #8B5CF6);
}

[data-theme="light"] {
  --color-bg: #F8FAFC;
  --color-surface: #FFFFFF;
  --color-surface-raised: #F1F5F9;
  --color-border: #E2E8F0;
  --color-border-subtle: #F1F5F9;
  --color-primary: #2563EB;
  --color-primary-hover: #1D4ED8;
  --color-accent: #7C3AED;
  --color-accent-hover: #6D28D9;
  --color-text: #0F172A;
  --color-text-secondary: #475569;
  --color-text-muted: #94A3B8;
  --gradient-primary: linear-gradient(135deg, #2563EB, #7C3AED);
}
```

---

## 2. Typography

```css
@import url('https://fonts.googleapis.com/css2?family=Space+Grotesk:wght@300;400;500;600;700&family=DM+Sans:ital,opsz,wght@0,9..40,400;0,9..40,500;0,9..40,700;1,9..40,400&display=swap');

:root {
  --font-heading: 'Space Grotesk', sans-serif;
  --font-body: 'DM Sans', sans-serif;
  --font-mono: 'JetBrains Mono', 'Fira Code', monospace;
}
```

| Element | Font | Weight | Size | Line Height |
|---------|------|--------|------|-------------|
| H1 Hero | Space Grotesk | 700 | clamp(2.5rem, 5vw, 4rem) | 1.1 |
| H1 Page | Space Grotesk | 700 | clamp(2rem, 4vw, 3rem) | 1.2 |
| H2 Section | Space Grotesk | 600 | clamp(1.5rem, 3vw, 2.25rem) | 1.3 |
| H3 Card | Space Grotesk | 600 | 1.25rem | 1.4 |
| Body | DM Sans | 400 | 1rem | 1.7 |
| Body Large | DM Sans | 400 | 1.125rem | 1.7 |
| Caption | DM Sans | 400 | 0.875rem | 1.5 |
| Code | JetBrains Mono | 400 | 0.875rem | 1.6 |
| Button | DM Sans | 600 | 0.9375rem | 1 |
| Badge | DM Sans | 500 | 0.75rem | 1 |

---

## 3. Spacing

| Token | Value | Usage |
|-------|-------|-------|
| `--space-1` | `4px` | Micro gaps |
| `--space-2` | `8px` | Icon gaps, inline |
| `--space-3` | `12px` | Tight padding |
| `--space-4` | `16px` | Standard padding |
| `--space-6` | `24px` | Card padding |
| `--space-8` | `32px` | Section inner gap |
| `--space-12` | `48px` | Section padding |
| `--space-16` | `64px` | Hero padding |
| `--space-24` | `96px` | Section margin |

---

## 4. Border Radius

| Token | Value | Usage |
|-------|-------|-------|
| `--radius-sm` | `6px` | Tags, badges, inputs |
| `--radius-md` | `10px` | Buttons |
| `--radius-lg` | `14px` | Cards |
| `--radius-xl` | `20px` | Feature cards, modals |
| `--radius-full` | `9999px` | Pills, avatar circles |

---

## 5. Shadows & Glow Effects

```css
/* Dark mode shadows */
--shadow-sm: 0 1px 3px rgba(0, 0, 0, 0.4);
--shadow-md: 0 4px 16px rgba(0, 0, 0, 0.4);
--shadow-lg: 0 8px 32px rgba(0, 0, 0, 0.5);
--shadow-xl: 0 20px 60px rgba(0, 0, 0, 0.6);

/* Glow effects (use sparingly, only on primary elements) */
--glow-primary: 0 0 20px rgba(59, 130, 246, 0.25);
--glow-accent: 0 0 20px rgba(139, 92, 246, 0.25);

/* Light mode shadows */
[data-theme="light"] {
  --shadow-sm: 0 1px 3px rgba(0, 0, 0, 0.08);
  --shadow-md: 0 4px 16px rgba(0, 0, 0, 0.08);
  --shadow-lg: 0 8px 32px rgba(0, 0, 0, 0.1);
  --glow-primary: 0 0 20px rgba(37, 99, 235, 0.15);
  --glow-accent: 0 0 20px rgba(124, 58, 237, 0.15);
}
```

---

## 6. Component Specifications

### Navbar

```css
.navbar {
  background: rgba(10, 15, 30, 0.85);      /* dark: bg-opacity-85 */
  backdrop-filter: blur(20px);
  -webkit-backdrop-filter: blur(20px);
  border-bottom: 1px solid var(--color-border);
  position: sticky;
  top: 0;
  z-index: 1000;
}

[data-theme="light"] .navbar {
  background: rgba(248, 250, 252, 0.9);
  border-bottom: 1px solid var(--color-border);
}
```

**Rules:**
- Sticky top navbar (NOT floating — content starts below navbar)
- Height: 64px desktop, 56px mobile
- Logo: gradient text (blue→purple) or white logo
- Mobile: hamburger menu with slide-in drawer
- Active nav link: `color: var(--color-primary)` with bottom border indicator
- No emojis — use Font Awesome or Heroicons SVG for nav icons

### Buttons

```css
/* Primary */
.btn-primary {
  background: var(--gradient-primary);
  color: white;
  padding: 10px 24px;
  border-radius: var(--radius-md);
  font-family: var(--font-body);
  font-weight: 600;
  font-size: 0.9375rem;
  border: none;
  cursor: pointer;
  transition: opacity 200ms ease, transform 200ms ease, box-shadow 200ms ease;
}
.btn-primary:hover {
  opacity: 0.9;
  transform: translateY(-1px);
  box-shadow: var(--glow-primary);
}
.btn-primary:active { transform: translateY(0); }

/* Secondary / Outline */
.btn-secondary {
  background: transparent;
  color: var(--color-text);
  padding: 10px 24px;
  border-radius: var(--radius-md);
  border: 1px solid var(--color-border);
  font-weight: 600;
  cursor: pointer;
  transition: all 200ms ease;
}
.btn-secondary:hover {
  border-color: var(--color-primary);
  color: var(--color-primary);
  background: rgba(59, 130, 246, 0.05);
}

/* Ghost */
.btn-ghost {
  background: transparent;
  color: var(--color-text-secondary);
  padding: 8px 16px;
  border-radius: var(--radius-md);
  border: none;
  cursor: pointer;
  transition: all 150ms ease;
}
.btn-ghost:hover {
  background: var(--color-surface-raised);
  color: var(--color-text);
}
```

### Cards

```css
.card {
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  padding: var(--space-6);
  transition: border-color 200ms ease, box-shadow 200ms ease, transform 200ms ease;
  cursor: pointer;
}
.card:hover {
  border-color: var(--color-primary);
  box-shadow: var(--shadow-md);
  transform: translateY(-2px);
}

/* Feature/Highlight Card */
.card-gradient {
  background: linear-gradient(135deg, rgba(59,130,246,0.1), rgba(139,92,246,0.1));
  border: 1px solid rgba(59, 130, 246, 0.2);
}
.card-gradient:hover {
  border-color: rgba(59, 130, 246, 0.4);
  box-shadow: var(--glow-primary);
}
```

### Badges / Tags

```css
.badge {
  display: inline-flex;
  align-items: center;
  padding: 2px 10px;
  border-radius: var(--radius-full);
  font-size: 0.75rem;
  font-weight: 500;
  letter-spacing: 0.01em;
}
.badge-primary {
  background: rgba(59, 130, 246, 0.15);
  color: #60A5FA;
  border: 1px solid rgba(59, 130, 246, 0.25);
}
.badge-accent {
  background: rgba(139, 92, 246, 0.15);
  color: #A78BFA;
  border: 1px solid rgba(139, 92, 246, 0.25);
}

[data-theme="light"] .badge-primary {
  background: rgba(37, 99, 235, 0.08);
  color: #1D4ED8;
  border-color: rgba(37, 99, 235, 0.2);
}
```

### Inputs & Forms

```css
.form-control {
  background: var(--color-surface-raised);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-sm);
  color: var(--color-text);
  padding: 10px 14px;
  font-size: 1rem;
  font-family: var(--font-body);
  transition: border-color 200ms ease, box-shadow 200ms ease;
  width: 100%;
}
.form-control::placeholder { color: var(--color-text-muted); }
.form-control:focus {
  outline: none;
  border-color: var(--color-primary);
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.15);
}

[data-theme="light"] .form-control {
  background: #FFFFFF;
  border-color: #E2E8F0;
  color: #0F172A;
}
```

### Skill Progress Bar

```css
.skill-bar {
  background: var(--color-border);
  border-radius: var(--radius-full);
  height: 6px;
  overflow: hidden;
}
.skill-bar-fill {
  height: 100%;
  background: var(--gradient-primary);
  border-radius: var(--radius-full);
  transition: width 1s ease-in-out;  /* animate on scroll into view */
}
```

### Section Headers

```css
.section-eyebrow {
  /* Small label above section title */
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  color: var(--color-primary);
}
.section-title {
  font-family: var(--font-heading);
  font-size: clamp(1.5rem, 3vw, 2.25rem);
  font-weight: 700;
  color: var(--color-text);
  margin-top: 8px;
}
.section-subtitle {
  font-size: 1.0625rem;
  color: var(--color-text-secondary);
  max-width: 560px;
  margin: 0 auto;
  line-height: 1.7;
}
```

---

## 7. Page Layout Structure

### Home Page Sections (in order)

1. **Hero** — Full-width, centered, gradient text headline, two CTAs (primary + outline), animated particles or gradient mesh background optional
2. **Team Stats** — 4 animated counters (Projects, Members, Technologies, Years)
3. **Latest Projects** — 3-column grid of cards (6 items), filter tags
4. **About / Technologies** — Split layout: text left, tech badge grid right
5. **Latest Blog Posts** — 3-column card grid (3 items)
6. **Testimonials** — Centered carousel or 3-column grid, quote icon, name + role
7. **Contact Form** — Split: form left, contact info + map right
8. **Footer** — Dark, 4-column layout with links, social, copyright

### Consistent Section Wrapper

```html
<section class="py-24">
  <div class="container mx-auto px-4" style="max-width: 1200px;">
    <!-- eyebrow + title + subtitle -->
    <!-- content -->
  </div>
</section>
```

---

## 8. Animation & Transition Rules

| Element | Effect | Duration | Easing |
|---------|--------|----------|--------|
| Buttons | `translateY(-1px)` + opacity | 200ms | ease |
| Cards | `translateY(-2px)` + border color | 200ms | ease |
| Nav links | Color change | 150ms | ease |
| Modal open | Fade + scale(0.95→1) | 250ms | ease-out |
| Section reveal | `translateY(20px)` → 0 + opacity | 600ms | ease-out |
| Skill bar fill | Width animation | 1000ms | ease-in-out |
| Counter | Count-up animation | 2000ms | ease-out |
| Toast | Slide-in from right | 300ms | ease-out |

**Critical rules:**
- Always add `@media (prefers-reduced-motion: reduce)` to disable animations
- Maximum animation duration: 600ms for UI, 2000ms for counting/progress only
- No infinite/looping decorative animations
- Use `will-change: transform` only when needed

```css
@media (prefers-reduced-motion: reduce) {
  *, *::before, *::after {
    animation-duration: 0.01ms !important;
    transition-duration: 0.01ms !important;
  }
}
```

---

## 9. Dark / Light Mode Toggle

```javascript
// Persist to localStorage, apply to <html data-theme="...">
const THEME_KEY = 'tp-theme';
const getTheme = () => localStorage.getItem(THEME_KEY) ||
  (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');
const applyTheme = (t) => {
  document.documentElement.setAttribute('data-theme', t);
  localStorage.setItem(THEME_KEY, t);
};
applyTheme(getTheme()); // run before DOM renders to prevent flash
```

**Toggle button:** Moon icon for dark, Sun icon for light — always visible in navbar.

---

## 10. Responsive Breakpoints

| Breakpoint | Width | Grid |
|------------|-------|------|
| Mobile | 375px+ | 1 column |
| Tablet | 768px+ | 2 columns |
| Desktop | 1024px+ | 3 columns |
| Wide | 1440px+ | 3–4 columns, wider container |

Bootstrap 5 breakpoints apply: `xs / sm(576) / md(768) / lg(992) / xl(1200) / xxl(1400)`

---

## 11. Icons

**Library:** Font Awesome 6 Free (already in project stack)  
**Rule:** Use `<i class="fa-brands fa-github"></i>` style — NEVER emojis as icons  
**Sizing:** Consistent sizing per context:
- Nav icons: `fa-fw` with `font-size: 1rem`
- Card icons: `font-size: 1.25rem`
- Hero icons: `font-size: 1.5rem`
- Social links: `font-size: 1.25rem`

---

## 12. Gradient Text

```css
.text-gradient {
  background: var(--gradient-primary);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}
/* Use on hero H1 partial word or section eyebrow */
```

---

## 13. Loading States

```css
/* Page loader (initial) */
#page-loader {
  position: fixed; inset: 0;
  background: var(--color-bg);
  display: flex; align-items: center; justify-content: center;
  z-index: 9999;
  transition: opacity 400ms ease;
}
/* Skeleton card */
.skeleton {
  background: linear-gradient(90deg,
    var(--color-surface) 25%,
    var(--color-surface-raised) 50%,
    var(--color-surface) 75%);
  background-size: 200% 100%;
  animation: skeleton-shimmer 1.5s infinite;
  border-radius: var(--radius-md);
}
@keyframes skeleton-shimmer {
  0% { background-position: 200% 0; }
  100% { background-position: -200% 0; }
}
```

---

## 14. Toast Notifications

```css
.toast {
  position: fixed; bottom: 24px; right: 24px;
  min-width: 280px; max-width: 400px;
  background: var(--color-surface-raised);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  padding: 14px 18px;
  box-shadow: var(--shadow-lg);
  z-index: 9998;
  animation: toast-in 300ms ease-out forwards;
}
@keyframes toast-in {
  from { transform: translateX(120%); opacity: 0; }
  to   { transform: translateX(0);    opacity: 1; }
}
.toast.success { border-left: 4px solid var(--color-success); }
.toast.error   { border-left: 4px solid var(--color-danger);  }
```

---

## 15. Anti-Patterns (NEVER USE)

| Forbidden | Reason |
|-----------|--------|
| Emojis as UI icons | Inconsistent rendering, not accessible |
| `transform: scale(1.05+)` on card hover | Causes layout shift |
| White `#FFFFFF` background in dark mode | Too harsh, use `#111827` |
| Gray-400 or lighter for body text in light mode | Fails contrast ratio |
| `border: 1px solid rgba(255,255,255,0.1)` in light mode | Invisible |
| Infinite decorative animations | Distracting, battery drain |
| `!important` overrides | Cascade violations |
| Hardcoded pixel values ignoring responsive | Not mobile-first |
| Neon glow on every element | Cyberpunk ≠ professional |

---

## 16. Pre-Delivery Checklist

### Visual Quality
- [ ] No emojis used as icons — only Font Awesome / SVG
- [ ] Hover states don't cause layout shift (no scale > 1.03)
- [ ] Gradient text uses `background-clip: text` pattern correctly
- [ ] Both dark and light mode tested visually

### Interaction
- [ ] `cursor: pointer` on ALL clickable elements
- [ ] Hover states provide clear visual feedback
- [ ] Transitions 150–300ms (max 600ms for reveals)
- [ ] Focus states visible (3px ring in primary color)

### Light Mode Contrast
- [ ] Body text ≥ `#475569` (slate-600) — never lighter
- [ ] Card backgrounds: `#FFFFFF` with `border: 1px solid #E2E8F0`
- [ ] No transparent glass cards in light mode unless opacity ≥ 80%

### Layout
- [ ] Sticky navbar — content starts directly below (not floating)
- [ ] `max-width: 1200px` container on all sections
- [ ] No horizontal scroll at 375px
- [ ] Breadcrumb on all pages except Home

### Accessibility
- [ ] All `<img>` have `alt` text
- [ ] Form inputs have `<label>` elements
- [ ] Color is never the only indicator
- [ ] `prefers-reduced-motion` CSS block included
- [ ] ARIA labels on icon-only buttons
