# Implementation Plan: Visual Upgrade V2

## Overview

Implements all 10 requirements from the Visual Upgrade V2 spec. Tasks are ordered from quickest win to most complex. Tasks 1–3 have no dependencies and can proceed immediately. Tasks 6–9 depend on Tasks 3 and 7 (glass system and scroll reveal must be in place first).

## Tasks

- [x] 1. Navbar Cleanup — Remove Dev Guide Button
  - Open `Views/Shared/_Navigation.cshtml`
  - Find and remove the "راهنمای توسعه" button element entirely
  - Verify navbar still has: Logo, Nav links, Contact/Talk button
  - Test mobile hamburger menu is unaffected
  - _Requirements: [Req 9]_

- [x] 2. Hero Overlay Fix — Restore Van Gogh Visibility
  - In `wwwroot/css/hero.css`, update `.hero-overlay` gradient: left side `rgba(10,11,15,0.90)` at 0%, `rgba(10,11,15,0.60)` at 35%, `rgba(10,11,15,0.20)` at 55%, `rgba(10,11,15,0.05)` at 70%, transparent at 100%
  - Update `.hero-overlay-bottom` height from 50% to 40%, reduce bottom opacity from 0.82 to 0.75
  - Verify Van Gogh character is clearly visible on the right
  - _Requirements: [Req 1]_

- [x] 3. Glass Card & Glow System
  - In `wwwroot/css/design-system.css`, add `.glass-panel` class: `backdrop-filter: blur(16px)`, background `rgba(10,16,35,0.55)`, border `rgba(245,200,66,0.15)`
  - Add `.glass-card` class: `backdrop-filter: blur(14px)`, gradient background, white border at 8% opacity
  - Add `.glass-card:hover` with amber border at 35% opacity and glow box-shadow
  - Add `.section-glow-amber` and `.section-glow-blue` blob classes (500×500px, blur 60px, position absolute)
  - _Requirements: [Req 7, Req 8]_

- [x] 4. Hero Stats Row
  - In `Views/Home/_HeroSection.cshtml`, add `.hero-stats-row` div after `.hero-cta-row` with three stats: "+48 Projects Delivered", "99% Client Satisfaction", "24/7 Support & Development"
  - Add `data-reveal` attribute to stats row
  - In `wwwroot/css/hero.css`, add styles for `.hero-stats-row` (grid layout), `.hero-stat`, `.hero-stat-num` (chrome yellow, mono font, 800 weight), `.hero-stat-label` (parchment, 0.68rem), `.hero-stat-divider` (1px vertical line)
  - Test at 375px, 768px, 1440px
  - _Requirements: [Req 10]_

- [x] 5. Code Snippet Interactive Titlebar
  - In `Views/Home/_HeroSection.cshtml`, add titlebar row inside `.hero-code-inner`: macOS dots (red/yellow/green), filename `starry_create.js`, copy icon button, "▶ Run" button
  - Add output panel `<div class="hero-code-output" hidden>` below the `<pre>` element
  - Create `wwwroot/js/hero-interactions.js`: Run button shows output panel, typewriter-animates 4 output lines over 2s; copy button writes raw code to clipboard and shows "Copied!" tooltip for 2s; "Run Again" state after completion
  - Add titlebar CSS (flex row, dots sizing/colors, button styles, output panel green text on dark bg) to `wwwroot/css/hero.css`
  - Add `<script src="~/js/hero-interactions.js" defer></script>` to `Views/Shared/_Layout.cshtml`
  - _Requirements: [Req 3]_

- [x] 6. Canvas Star Field
  - Create `wwwroot/js/canvas-stars.js`: initialize canvas `position:fixed; inset:0; z-index:0; pointer-events:none; opacity:0.75`; generate stars (`Math.floor(W*H/12000)` clamped 150–300); render loop with trail effect (`rgba(7,11,25,0.25)` per frame); orbit animation per star; mouse attraction (max 12px within 180px); 6 slow-rotating brushstroke shapes; window resize handler
  - In `Views/Shared/_Layout.cshtml`, add `<canvas id="star-canvas" aria-hidden="true">` as first element inside `<body>` and `<script src="~/js/canvas-stars.js" defer>` before `</body>`
  - In `wwwroot/css/motion.css` add `@media (prefers-reduced-motion: reduce) { #star-canvas { display: none; } }`
  - _Requirements: [Req 2]_

- [x] 7. Scroll Reveal System
  - Create `wwwroot/js/scroll-reveal.js`: IntersectionObserver with threshold 0.15 targeting `[data-reveal]` elements; on intersect add class `.revealed` then `unobserve`; if `prefers-reduced-motion` active skip animation (add `.revealed` immediately on init)
  - In `wwwroot/css/motion.css` add: `[data-reveal]` initial state `opacity:0; transform:translateY(24px); transition:opacity 600ms ease-out, transform 600ms ease-out`; `.revealed` restores both; `[data-reveal-delay="1"]` through `[data-reveal-delay="4"]` add `transition-delay` 100ms–400ms
  - Apply `data-reveal` and `data-reveal-delay` attributes to existing section elements in `Views/Home/Index.cshtml` and any other home partial views
  - Add `<script src="~/js/scroll-reveal.js" defer>` to `Views/Shared/_Layout.cshtml`
  - _Requirements: [Req 6]_

- [x] 8. Skills Section Redesign — Constellation Grid
  - Create `wwwroot/css/skills.css`: `.skills-grid` (5 cols desktop / 3 tablet / 2 mobile), `.skill-card` (flex-col, centered, 20px padding), `.skill-icon` (40×40px), `.skill-name` (0.8rem, 600 weight), `.skill-badge` base, category badge variants (`.badge-frontend` sky-blue, `.badge-backend` purple, `.badge-mobile` amber, `.badge-devops` emerald, `.badge-design` rose)
  - Create `Views/Home/_SkillsSection.cshtml`: section with eyebrow "Tech Stack", h2 "Technologies We Master", `.section-glow-blue` bottom-left, `.skills-grid` with 15+ skill cards — each with inline SVG icon, name, category badge, `.glass-card` class, `data-reveal` and staggered `data-reveal-delay`; skills include: React, TypeScript, Vue.js, C#/.NET, Node.js, Python, Flutter, Kotlin, Docker, Kubernetes, PostgreSQL, Redis, CI/CD, Git, Figma
  - In `Views/Home/Index.cshtml`, replace or remove the existing skills section and include `@await Html.PartialAsync("_SkillsSection")`
  - Add `<link rel="stylesheet" href="~/css/skills.css" asp-append-version="true">` to `Views/Shared/_Layout.cshtml`
  - _Requirements: [Req 5]_
  - _Dependencies: [Task 3, Task 7]_

- [x] 9. Terminal Team Explorer
  - Create `wwwroot/css/terminal.css`: `.terminal-window` (border-radius 12px, overflow hidden), `.terminal-titlebar` (dark bg, flex row, height 40px), `.terminal-dots` (flex gap-2), `.dot` (10×10px rounded-full), `.terminal-body` (min-height 280px, padding 16px, font mono, overflow-y auto, max-height 400px), `.terminal-output-line` (text parchment, margin 2px 0), `.terminal-input-row` (flex, border-top amber 10% opacity, padding 8px 16px), `.terminal-prompt` (amber, mono, margin-right 8px), `.terminal-input` (flex-1, transparent bg, no border/outline, parchment color, mono font), cursor blink animation
  - Create `wwwroot/js/terminal.js`: `TEAM` array with team member objects (id, name, role, skills array, experience, bio); `COMMANDS` object with handlers for `help`, `ls`, `cat [name]`, `whoami`, `clear`; command history array with ArrowUp/ArrowDown cycling; Enter keydown handler that runs command, appends output to `#terminal-output`, scrolls to bottom; boot sequence that auto-types welcome message on DOMContentLoaded
  - Create `Views/Home/_TerminalSection.cshtml`: `<section id="terminal">` with container, section eyebrow + heading, `.section-glow-amber` top-right, terminal window HTML with titlebar (macOS dots + `team@starry:~$` title), `#terminal-output` div, input row with `$` prompt and `#terminal-input` text input; apply `.glass-card` to terminal window; apply `data-reveal` to terminal window
  - In `Views/Home/Index.cshtml`, include `@await Html.PartialAsync("_TerminalSection")` after the hero section
  - Add `<link rel="stylesheet" href="~/css/terminal.css" asp-append-version="true">` and `<script src="~/js/terminal.js" defer></script>` to `Views/Shared/_Layout.cshtml`
  - _Requirements: [Req 4]_
  - _Dependencies: [Task 3, Task 7]_

## Task Dependency Graph

```json
{
  "waves": [
    {
      "wave": 1,
      "tasks": [1, 2, 3, 4, 5, 6, 7],
      "description": "All independent tasks — no dependencies, can run in parallel"
    },
    {
      "wave": 2,
      "tasks": [8, 9],
      "description": "Depend on Task 3 (glass system) and Task 7 (scroll reveal)"
    }
  ]
}
```

## Notes

- All tasks are frontend-only (CSS, JS, Razor views). No backend C# changes needed.
- Task 6 (Canvas) and Task 7 (Scroll Reveal) are independent of each other and can run in parallel.
- Tasks 8 and 9 should run after Task 3 (glass system) and Task 7 (reveal) are complete so the classes exist.
- The canvas `z-index: 0` must be lower than all section content `z-index` values — verify `z-index` stacking in `design-system.css` before Task 6.
- Terminal data (Task 9) uses the actual team members from the site — update `TEAM` array with real data from the existing Team controller/models.
