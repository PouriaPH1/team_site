# Requirements Document

## Introduction

This spec covers a comprehensive visual upgrade of the Team Portfolio website homepage. The goal is to bring the site to the same visual quality level as the reference site (starry-code.ai.studio), fix existing layout/visibility problems, and introduce three new interactive sections: an animated terminal-based team explorer, a redesigned skills section, and a full-page canvas star field that persists across all sections.

**Reference site:** starry-code.ai.studio (analyzed and downloaded)

**Existing problems to fix:**
1. Hero background image is too dim/faded under overlays — Van Gogh character loses visual impact
2. Code snippet block feels static and lacks interaction
3. Skills section uses progress bars with percentages — feels childish and doesn't communicate expertise well
4. "راهنمای توسعه" (Dev Guide) button in navbar must be removed

**New features to add:**
1. Canvas star field (animated particles across all pages)
2. Scroll-triggered section reveals (About, Team, Skills, Portfolio sections)
3. Glass card system (`.glass-panel`, `.glass-card`) matching reference quality
4. Section glow blob decorations
5. Interactive terminal — replaces or augments team section
6. Code snippet "Run" button with animated output
7. Skills section redesign — graphical, no percentages

**Stack:** ASP.NET Core 9 MVC + Bootstrap 5 + Vanilla JS (no frameworks)

---

## Requirement 1: Hero Background Visibility Fix

**User Story:** As a visitor, I want to clearly see the Van Gogh painting and character in the hero section without them being washed out by overlays.

### Acceptance Criteria

1. THE `.hero-overlay` gradient SHALL be reduced so the right side (where Van Gogh character is) has maximum `rgba(10,11,15,0.15)` opacity — currently too dark
2. THE hero background image SHALL use `opacity: 1` on `.hero-bg` (remove any dimming on the image element itself)
3. THE `.hero-overlay-bottom` SHALL stop at 40% height (not 50%) to show more of the painting lower section
4. THE Van Gogh character (right side of painting) SHALL be clearly visible without any dark overlay blocking it
5. THE left-side text area SHALL remain readable with gradient from `rgba(10,11,15,0.90)` at 0% → `rgba(10,11,15,0.60)` at 35% → transparent at 60%

---

## Requirement 2: Canvas Star Field

**User Story:** As a visitor, I want to see an animated starry-night particle field across the entire page that creates depth and atmosphere.

### Acceptance Criteria

1. A `<canvas>` element SHALL be injected as `position: fixed; inset: 0; z-index: 0; pointer-events: none; opacity: 0.75` covering the full page
2. THE canvas SHALL render between 150–300 star particles depending on viewport size (formula: `Math.floor(width * height / 12000)`)
3. EACH star SHALL have:
   - Random position, size (0.5–3px radius), and color from palette: `#F5C842`, `#F97316`, `#38BDF8`, `#818CF8`, `#FEF08A`
   - Orbit animation — star moves in a small circle around its base position
   - Pulse animation on alpha value
4. WHEN mouse moves over the page, stars within 180px of cursor SHALL shift toward cursor by max 12px (attraction effect)
5. THE canvas SHALL use `requestAnimationFrame` with a trail effect: clear with `fillStyle = rgba(7,11,25,0.25)` each frame for motion blur
6. SIX ambient brushstroke shapes SHALL slowly rotate as background texture at `opacity: 0.06`
7. WHEN `prefers-reduced-motion` matches, canvas SHALL NOT render (hidden via CSS)
8. THE canvas SHALL resize on `window.resize` without layout shift

---

## Requirement 3: Code Snippet — Interactive "Run" Button

**User Story:** As a visitor, I want to click "Run" on the code snippet and see an animated output that feels like the code is actually executing.

### Acceptance Criteria

1. THE code block SHALL have a titlebar row with: macOS-style dots (red/yellow/green), filename `starry_create.js`, a copy icon button, and a "▶ Run" button
2. THE "Run" button SHALL have `background: rgba(245,200,66,0.20)`, amber border, amber text
3. WHEN user clicks "Run", an output panel SHALL slide down below the code with typewriter effect showing:
   ```
   > Running starry_create.js...
   [IDEAS: LIMITLESS | STATUS: CREATING...]
   > Innovation loop started ✓
   > Output: Building something beautiful.
   ```
4. THE output text SHALL appear character-by-character over 2 seconds
5. THE output panel background SHALL be `rgba(10,11,15,0.95)` with green `#10B981` text color (terminal green)
6. AFTER output completes, "Run" button SHALL change to "▶ Run Again" and allow re-triggering
7. Copy button SHALL copy the raw code to clipboard and show a brief "Copied!" tooltip for 2 seconds

---

## Requirement 4: Interactive Terminal Team Explorer

**User Story:** As a visitor, I want to explore team members through a Linux-style terminal interface instead of seeing boring profile cards with percentage bars.

### Acceptance Criteria

1. A new section SHALL appear below the hero (or replace/augment the existing team section) with id `#terminal`
2. THE section SHALL render a full terminal window UI with:
   - Dark titlebar: `bg: rgba(10,11,15,0.95)`, macOS dots, title `team@starry:~$`
   - Terminal body: monospace font, dark background, amber cursor
3. THE terminal SHALL boot with an auto-typed welcome message:
   ```
   Welcome to Starry Code Team Explorer v1.0
   Type 'help' to see available commands.
   ```
4. THE following commands SHALL be supported:
   - `ls` or `ls -la` — lists all team members with name and role
   - `cat [name]` — shows detailed profile: name, role, skills, years of experience, bio
   - `help` — shows available commands
   - `clear` — clears terminal output
   - `whoami` — shows a random team philosophy quote
5. THE terminal SHALL have a text input field at the bottom (`$` prompt) that accepts keyboard input
6. PRESSING Enter SHALL execute the command and show output
7. PRESSING ArrowUp/ArrowDown SHALL cycle through command history
8. UNKNOWN commands SHALL show: `command not found: [input]. Type 'help' for available commands.`
9. THE team data SHALL be pulled from a JSON data structure defined in a JS file (no server calls needed for this feature)
10. ON mobile, the input SHALL be tappable and keyboard-friendly

---

## Requirement 5: Skills Section Redesign — No Percentages

**User Story:** As a visitor, I want to see the team's technical skills presented in a visually engaging way without childish percentage bars.

### Acceptance Criteria

1. THE existing skill progress bar UI SHALL be removed entirely
2. THE skills SHALL be displayed as a **technology constellation grid** — icon + name + category badge, no numbers
3. SKILLS SHALL be grouped into categories: Frontend, Backend, Mobile, DevOps, Design
4. EACH skill card SHALL show:
   - Technology SVG/icon (from Simple Icons CDN or inline SVG)
   - Technology name
   - Category badge with color per category
5. ON hover, skill card SHALL show a subtle glow matching the category color
6. THE layout SHALL be a responsive CSS grid: 5 columns on desktop, 3 on tablet, 2 on mobile
7. CARDS SHALL animate in with staggered fade+scale on scroll into view (50ms delay between each)
8. NO percentage numbers, NO progress bars, NO skill level indicators

---

## Requirement 6: Scroll-Triggered Section Reveals

**User Story:** As a visitor, I want sections to smoothly reveal as I scroll, making the page feel alive and premium.

### Acceptance Criteria

1. ALL sections below the hero SHALL have elements with initial state `opacity: 0; transform: translateY(30px)`
2. WHEN a section enters the viewport (IntersectionObserver threshold: 0.15), its elements SHALL transition to `opacity: 1; transform: translateY(0)` over 600ms with `ease-out`
3. CHILD elements within a section SHALL stagger with 100ms delay between each
4. THE following elements SHALL receive scroll reveal:
   - Section headings and eyebrow labels
   - Team cards
   - Portfolio cards
   - About section image and text columns
   - Stats counters
5. WHEN `prefers-reduced-motion` matches, all elements SHALL appear immediately without animation
6. Reveal SHALL happen ONCE — elements do not re-animate if user scrolls back up

---

## Requirement 7: Glass Card System

**User Story:** As a developer, I want a consistent glass-morphism card system that matches the reference site quality across all sections.

### Acceptance Criteria

1. A `.glass-panel` class SHALL be defined with:
   ```css
   backdrop-filter: blur(16px);
   -webkit-backdrop-filter: blur(16px);
   background: rgba(10, 16, 35, 0.55);
   border: 1px solid rgba(245, 200, 66, 0.15);
   ```
2. A `.glass-card` class SHALL be defined with:
   ```css
   backdrop-filter: blur(14px);
   background: linear-gradient(135deg, rgba(12,19,43,0.65), rgba(22,33,62,0.45));
   border: 1px solid rgba(255,255,255,0.08);
   ```
3. `.glass-card:hover` SHALL change border to `rgba(245,200,66,0.35)` and add `box-shadow: 0 12px 35px -10px rgba(245,200,66,0.22)`
4. THESE classes SHALL be added to `design-system.css` and applied to team cards, about cards, and stats counters
5. NO existing Bootstrap card classes SHALL be broken — glass classes are additive

---

## Requirement 8: Section Glow Blobs

**User Story:** As a visitor, I want soft ambient light effects between sections that give the page depth and visual warmth.

### Acceptance Criteria

1. A `.section-glow-amber` class SHALL render a blurred radial gradient blob:
   ```css
   position: absolute;
   width: 500px; height: 500px;
   background: radial-gradient(circle, rgba(245,158,11,0.12), transparent 70%);
   filter: blur(60px);
   border-radius: 50%;
   pointer-events: none;
   ```
2. A `.section-glow-blue` class SHALL be identical with color `rgba(56,189,248,0.10)`
3. EACH section (About, Team, Skills, Portfolio) SHALL have 1-2 glow blobs positioned at corners
4. GLOW blobs SHALL be `position: absolute` within `position: relative` sections, `z-index: 0` so they stay behind content
5. THESE classes SHALL be added to `design-system.css`

---

## Requirement 9: Navbar Cleanup

**User Story:** As a visitor, I want a clean navbar without unnecessary developer-only buttons.

### Acceptance Criteria

1. THE "راهنمای توسعه" (Dev Guide) button SHALL be removed from the navbar entirely
2. THE navbar SHALL remain: Logo + Nav links + "Let's Talk" / "تماس" button only
3. NO other navbar functionality SHALL be changed
4. IF a partial view `_Navigation.cshtml` exists, the button SHALL be removed from there

---

## Requirement 10: Hero Stats Row

**User Story:** As a visitor, I want to see key team stats directly in the hero section to immediately establish credibility.

### Acceptance Criteria

1. A stats row SHALL appear below the CTA buttons in the hero-left column
2. IT SHALL show 3 stats separated by subtle dividers:
   - `+48` Projects Delivered
   - `99%` Client Satisfaction
   - `24/7` Support & Development
3. THE numbers SHALL use font-weight 800, chrome yellow color `#F5C842`, font-family monospace
4. THE labels SHALL use font-size 0.7rem, parchment color `#E8DFC8`, opacity 0.8
5. THE row SHALL be separated from CTA buttons by a `border-top: 1px solid rgba(248,244,232,0.12)` with `padding-top: 16px`
6. ON mobile, the stats row SHALL remain visible and stack as a 3-column grid

---

## What is NOT in Scope

- Changes to any controllers, models, services, repositories, or database logic
- Changes to pages other than Home (About, Team, Portfolio, Blog, Contact pages)
- Light mode changes
- Any server-side data fetching for the terminal (all data is static JS)
- Authentication or user management
- The existing Blog, Contact, or Portfolio sections' backend logic
