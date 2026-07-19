# Requirements Document — Van Gogh Hero Redesign

## Introduction

**v2 — Updated based on reference image and Van Gogh aesthetic direction.**

This spec covers the visual redesign of the Team Portfolio home page hero section and global navigation. The goal is to recreate the reference image exactly: a full-viewport hero using `raw-vangogh-without-text.png` as background, with all HTML/CSS UI elements positioned on top — text, nav, code snippet, buttons — in a way that feels like they were painted into the scene.

**Core philosophy:** Technology living INSIDE the painting, not ON TOP of it.
- ❌ No neon, no cyberpunk, no glassmorphism excess, no sci-fi
- ✅ Warm oil-paint palette, organic animations, candlelight glow
- ✅ Van Gogh character visible on right — UI elements on left

**Assets:**
- `wwwroot/images/raw-vangogh-without-text.png` — clean background (no text)
- `wwwroot/images/vangogh-hero.png` — reference image (with UI elements)

---

## Glossary

- **Hero_Background** — `raw-vangogh-without-text.png` used as full-viewport CSS background-image
- **Ken_Burns** — slow scale+pan animation on background image that makes it "breathe"
- **Oil_Palette** — warm color set: prussian blue, chrome yellow, ochre, moon white, parchment
- **Eyebrow** — small uppercase label above main headline (e.g. "WE ARE")
- **Create_Word** — the gold "Create." word in the headline — largest, focal point of page
- **Code_Snippet** — compact code block at bottom-left of hero
- **Comment_Right** — floating `// comment` lines at top-right of hero (like in reference)
- **Scroll_Indicator** — "SCROLL DOWN" text + mouse icon at bottom center
- **Parallax** — subtle mouse-move effect on background image for depth
- **Typewriter** — character-by-character text reveal on subtitle

---

## Requirements

---

### Requirement 1: Background Image

**User Story:** As a visitor, I want the hero to feel like I stepped inside Van Gogh's Starry Night painting, not a space/NASA website.

#### Acceptance Criteria

1. THE Hero_Background SHALL use `raw-vangogh-without-text.png` as `background-image` with `background-size: cover` and `background-position: center center`
2. THE Hero_Background SHALL cover exactly 100vh height with no scroll needed
3. THE Hero_Background SHALL animate with Ken_Burns effect: very slow scale from 1.0 to 1.05 over 20 seconds, alternating, with `ease-in-out`
4. WHEN the user moves the mouse, THE Hero_Background SHALL shift slightly (parallax), maximum 15px displacement at viewport edges
5. THE Hero_Background SHALL have a dark gradient overlay on the left side (`rgba(10,11,15,0.85)` at left edge fading to transparent at ~65% width) to make text readable while keeping the character visible on the right

---

### Requirement 2: Navigation

**User Story:** As a visitor, I want a clean transparent navigation bar that feels part of the painting, not a tech overlay.

#### Acceptance Criteria

1. THE Navigation SHALL be `position: fixed; top: 0` full-width, with dark semi-transparent background (`rgba(10,11,15,0.82)`) and `backdrop-filter: blur(12px)`
2. THE Navigation height SHALL be exactly 72px
3. THE Navigation SHALL contain: Logo (left) + Nav links (center) + "Let's Talk" button (right)
4. Nav link color SHALL be `rgba(248,244,232,0.72)` (warm white at 72% opacity) in inactive state
5. Nav link color SHALL be `#F5C842` (chrome yellow) in active state, with a 4px gold dot above it
6. THE "Let's Talk" button SHALL have transparent background with `1px solid rgba(248,244,232,0.35)` border, no fill
7. WHEN viewport is below 768px, desktop links SHALL hide and hamburger icon SHALL appear

---

### Requirement 3: Hero Headline

**User Story:** As a visitor, I want the headline to have visual hierarchy matching the reference image exactly.

#### Acceptance Criteria

1. THE Hero headline SHALL be left-aligned, positioned at 8vw from left
2. ABOVE the headline, a small eyebrow label "WE ARE" SHALL appear in chrome yellow (`#F5C842`), uppercase, letter-spacing 0.25em, font-size 0.8rem
3. "Code. Design." SHALL appear in warm white (`#F8F4E8`), font-size `clamp(2.8rem, 6vw, 5.5rem)`, font-weight 800, Manrope font
4. "Create." SHALL appear on its own line, in chrome yellow (`#F5C842`), font-size `clamp(3.5rem, 8vw, 7rem)`, font-weight 800, with a soft gold glow animation cycling between `0 0 40px rgba(245,200,66,0.25)` and `0 0 80px rgba(245,200,66,0.45)` over 4 seconds
5. All headline text SHALL have `text-shadow: 0 2px 20px rgba(0,0,0,0.6)` for readability over the painting

---

### Requirement 4: Hero Subtitle

**User Story:** As a visitor, I want a subtitle that introduces the team in one warm sentence.

#### Acceptance Criteria

1. THE subtitle SHALL read: "We're a team of passionate developers and designers building beautiful digital experiences."
2. THE subtitle SHALL use Typewriter effect — character by character over 3 seconds
3. THE subtitle font SHALL be IBM Plex Sans, 1rem–1.15rem, warm parchment color `#E8DFC8`, opacity 0.88
4. WHEN `prefers-reduced-motion` matches, full text SHALL appear immediately

---

### Requirement 5: CTA Button

**User Story:** As a visitor, I want a single clear call-to-action that invites me to explore the work.

#### Acceptance Criteria

1. ONE primary CTA button SHALL be visible: "Explore Our Work →"
2. THE button background SHALL be cobalt blue (`#1E3A5F`), NOT gradient, NOT neon
3. THE button SHALL have `border-radius: 8px`, padding `14px 28px`, font-weight 600
4. ON hover, button background SHALL deepen to `#2B4B8C` with a subtle `box-shadow: 0 0 20px rgba(74,155,181,0.20)`
5. ON hover, button SHALL translate up 2px
6. A secondary ghost button "Meet the Team" SHALL appear beside it with transparent background and `1px solid rgba(248,244,232,0.25)` border

---

### Requirement 6: Code Snippet

**User Story:** As a visitor, I want to see a poetic code snippet that hints at the team's craft.

#### Acceptance Criteria

1. THE code snippet SHALL be positioned at bottom-left of hero, above the fold
2. THE code snippet SHALL be compact — max-width 340px, small font (0.78rem)
3. THE code snippet background SHALL be `rgba(10,11,15,0.82)` with `1px solid rgba(245,200,66,0.12)` border
4. THE code snippet SHALL show line numbers (01, 02, 03...) in muted amber
5. THE code content SHALL be poetic C# code:
   ```
   const create = () => {
     let ideas = 'limitless';
     let passion = true;
     while (passion) {
       code();
       design();
       innovate();
     }
   }
   ```
6. Syntax highlighting SHALL use warm colors: keywords in muted cyan, strings in straw yellow, comments in translucent amber

---

### Requirement 7: Right-Side Comment Block

**User Story:** As a visitor, I want to see floating code comments on the right side of the hero that complement the painting without covering the character.

#### Acceptance Criteria

1. THREE floating comment lines SHALL appear at top-right of hero (approximately 70–80% from left, 30–40% from top)
2. THE comment text SHALL be:
   - `// The best code`
   - `// is written with`
   - `// passion.`
3. THE comment color SHALL be `rgba(200,180,140,0.55)` — translucent warm amber, font JetBrains Mono, italic, 0.8rem
4. THE comments SHALL NOT overlap the Van Gogh character (positioned above the character area)

---

### Requirement 8: Scroll Indicator

**User Story:** As a visitor, I want a scroll hint at the bottom that feels part of the design.

#### Acceptance Criteria

1. A scroll indicator SHALL appear at bottom-center of hero
2. IT SHALL consist of a mouse/chevron SVG icon + "SCROLL DOWN" text below
3. THE color SHALL be `rgba(248,244,232,0.55)`
4. IT SHALL animate with a gentle bounce: `translateY(0)` to `translateY(6px)` over 2 seconds, infinite
5. IT SHALL fade out when the user scrolls past the hero section
6. `aria-hidden="true"` and `tabindex="-1"` SHALL be applied

---

### Requirement 9: Animations — Harmony with Painting

**User Story:** As a visitor, I want all animations to feel organic and painterly, not digital or mechanical.

#### Acceptance Criteria

1. ALL animations SHALL be slow and breathing — no fast flashes, no abrupt changes
2. THE background SHALL use Ken_Burns animation: `scale(1.0)` → `scale(1.05)` over 20s, alternating, `ease-in-out`
3. THE "Create." word SHALL have a gold glow pulse: dim → bright over 4s, `ease-in-out infinite alternate`
4. THE scroll indicator SHALL bounce gently: `translateY(0)` → `translateY(6px)` over 2s
5. ON mouse enter hero, elements SHALL have subtle parallax: background shifts max 15px, text shifts max 5px in opposite direction (depth illusion)
6. Entrance animations: headline fades in from `translateY(20px), opacity:0` over 800ms; subtitle after 200ms delay; button after 400ms delay; code snippet after 600ms delay
7. WHEN `prefers-reduced-motion` matches, ALL animations SHALL be disabled except typewriter (which snaps immediately)

---

### Requirement 10: Typography

**User Story:** As a developer/designer, I want typography that feels warm and humanist, not tech-cold.

#### Acceptance Criteria

1. Heading font SHALL be **Manrope** (weights 700, 800) — warm, humanist sans-serif
2. Body font SHALL be **IBM Plex Sans** (weights 400, 500, 600) — slightly warm, readable
3. Mono font SHALL remain **JetBrains Mono** (weight 400) — for code snippets
4. Letter-spacing on eyebrow label SHALL be 0.25em
5. Letter-spacing on nav links SHALL be 0.02em

---

### Requirement 11: Color Palette

**User Story:** As a designer, I want the palette to feel like oil paint on canvas, not pixels on a screen.

#### Acceptance Criteria

1. Primary background: `#0A0B0F` (near-black canvas)
2. Night sky blue: `#1A2744` (Prussian blue — Van Gogh used this)
3. Main text: `#F8F4E8` (warm moon white — not pure white)
4. Gold accent: `#F5C842` (chrome yellow — Van Gogh's signature color)
5. Secondary text: `#E8DFC8` (parchment)
6. Muted text: `#D4B87A` (warm straw)
7. Button blue: `#1E3A5F` (cobalt)
8. NO neon, NO electric cyan, NO RGB glow

---

### Requirement 12: Accessibility

#### Acceptance Criteria

1. Skip nav link SHALL be first focusable element
2. All interactive elements SHALL have `:focus-visible` ring in chrome yellow
3. Text contrast SHALL meet WCAG AA (4.5:1 for normal text)
4. `aria-hidden="true"` on decorative elements (scroll indicator, background)
5. Hamburger button SHALL have proper `aria-label` and `aria-expanded`

---

### Requirement 13: Responsive

#### Acceptance Criteria

1. AT 375px: headline font-size reduces, code snippet goes full-width, comment block hides
2. AT 768px: side-by-side layout maintained
3. AT 1440px: max-width container prevents content from stretching too wide
4. NO horizontal scroll at any breakpoint

---

### Requirement 14: Image Optimization

#### Acceptance Criteria

1. `raw-vangogh-without-text.png` SHALL be the background image source
2. RECOMMENDED image dimensions: minimum 1920×1080px for crisp display on retina
3. `background-position: center 30%` (slightly above center to keep character in view)
4. On mobile (`<768px`), `background-position: 70% center` to favor the character side

---

## What is NOT in scope

- Any changes to controllers, models, services, or database
- Other pages (About, Team, Portfolio, Blog, Contact) — only Home hero
- Light mode
- Any neon/cyberpunk/sci-fi elements
