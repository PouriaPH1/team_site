# Implementation Plan — Van Gogh Hero (Phase 1)

## Overview

Deliver a complete, working Van Gogh hero section using `raw-vangogh-without-text.png` as background image. All UI elements are HTML/CSS. Three animations only: Ken Burns, gold glow pulse, scroll bounce.

**Approach:** Modify existing files in-place. No new architecture. Ship fast.

---

## Tasks

- [ ] 1. Simplify background-system (remove CSS-based 8-layer bg)
  - Remove the 8 `.bg-layer` divs from `_Layout.cshtml` — the painting replaces them
  - Keep `#background-system` div but make it empty (for future layers)
  - Simplify `background-system.css` — remove all layer animations (star-twinkle, cloud-drift, etc.)
  - Keep only: `#background-system { display: none; }` — disabled for now
  - _Why: The Van Gogh image replaces all CSS background layers_

- [ ] 2. Update `design-system.css` — Van Gogh palette (already done, verify)
  - Confirm Manrope + IBM Plex Sans fonts loading correctly
  - Confirm all warm color tokens present
  - Confirm backward-compat remaps intact (so site.css doesn't break)
  - _File: `src/TeamPortfolio.Web/wwwroot/css/design-system.css`_

- [ ] 3. Rewrite `_HeroSection.cshtml` — exact reference image layout
  - Background layer: `<div class="hero-bg">` with inline style referencing `raw-vangogh-without-text.png`
  - Overlay layer: `<div class="hero-overlay">` — left-to-right gradient
  - Content wrapper: left-aligned, 8vw from left, centered vertically
  - Eyebrow: `<p class="hero-eyebrow">WE ARE</p>`
  - Headline block:
    - `<span class="hero-line-main">Code. Design.</span>` — warm white
    - `<span class="hero-line-create">Create.</span>` — gold, larger, glow animation
  - Subtitle: `<p class="hero-subtitle" data-typewriter="...">` — typewriter target
  - CTA row: primary "Explore Our Work →" + secondary ghost "Meet the Team"
  - Code snippet: `<div class="hero-code-block">` at bottom-left with JS/C# code
  - Right comments: `<div class="hero-comments-right">` with 3 comment lines
  - Scroll indicator: bottom-center, aria-hidden
  - _File: `src/TeamPortfolio.Web/Views/Home/_HeroSection.cshtml`_

- [ ] 4. Finalize `hero.css` — match design spec
  - `.hero-bg`: `background-image: url('/images/raw-vangogh-without-text.png')`, cover, center 30%
  - Ken Burns animation: `scale(1.0)→scale(1.06)`, 20s, alternate, ease-in-out
  - `.hero-overlay`: left gradient `rgba(10,11,15,0.88)→transparent` at 65% width
  - `.hero-overlay-bottom`: bottom gradient for code snippet readability
  - `.hero-line-main`: warm white, clamp(2.8rem, 6vw, 5.5rem), weight 800
  - `.hero-line-create`: chrome yellow, clamp(3.5rem, 8vw, 7rem), weight 800, gold glow animation
  - `.hero-code-block`: positioned at bottom-left, compact size
  - `.hero-comments-right`: positioned top-right, monospace italic, translucent
  - Responsive: mobile adjustments for font sizes and code block
  - _File: `src/TeamPortfolio.Web/wwwroot/css/hero.css`_

- [ ] 5. Update `navigation.css` — transparent Van Gogh nav
  - Full-width bar, NOT pill/floating — flush to top
  - Height: 72px
  - Background: `rgba(10,11,15,0.88)` + `backdrop-filter: blur(12px)`
  - `border-bottom: 1px solid rgba(245,200,66,0.08)`
  - Nav link inactive: `rgba(248,244,232,0.72)`
  - Nav link active: `#F5C842` (chrome yellow)
  - Active dot: 4px gold dot above active link
  - "Let's Talk" button: ghost style (transparent bg, warm white border)
  - _File: `src/TeamPortfolio.Web/wwwroot/css/navigation.css`_

- [ ] 6. Simplify `motion.js` — entrance animations + typewriter + parallax
  - Remove: star generation, particle generation, constellation SVG, galaxy canvas
  - Keep: scroll reveal, typewriter, entrance stagger for hero elements
  - Add: mouse-move parallax on hero background (subtle, single layer)
    - On mousemove: `background-position` shifts max ±15px (NOT a separate layer)
    - This simulates depth without needing separate image layers
  - Keep: magnetic button, card tilt, mobile drawer
  - _File: `src/TeamPortfolio.Web/wwwroot/js/motion.js`_

- [ ] 7. Update `_Navigation.cshtml` — use new nav CSS classes
  - Ensure nav links use correct classes matching updated `navigation.css`
  - Logo: keep SVG constellation dots + "TeamPortfolio" brand text
  - No changes to tag helpers or active-state logic
  - _File: `src/TeamPortfolio.Web/Views/Shared/_Navigation.cshtml`_

- [ ] 8. Update `_Layout.cshtml` — remove bg-layer markup
  - Remove the 8 `.bg-layer` divs inside `#background-system`
  - Keep `#background-system` div empty (for future phase 2 layers)
  - All CSS links remain in correct order
  - _File: `src/TeamPortfolio.Web/Views/Shared/_Layout.cshtml`_

- [ ] 9. Build and verify
  - Run `dotnet build` — must succeed with 0 errors
  - Open `http://localhost:5200` — verify:
    - Background image fills viewport
    - Gradient overlay makes text readable (left side)
    - Van Gogh character visible (right side)
    - Ken Burns animation playing (slow breathe)
    - "Create." has gold glow
    - Nav bar is 72px, transparent, links visible
    - Code snippet visible at bottom-left
    - Scroll indicator bouncing at bottom-center
    - Typewriter working on subtitle
    - Entrance animations playing on load
    - No horizontal overflow

---

## Notes

- `raw-vangogh-without-text.png` is the background (1536×1024) — no resize needed
- `background-size: cover` handles all viewport sizes
- Do NOT add Canvas, Three.js, WebGL, or particle systems — Phase 3 only
- Do NOT add glassmorphism panels — they break the oil-paint feel
- Keep all existing controller/service/DB code untouched

## Task Dependency Order

```
1 → 2 → 3 → 4 → 5 → 6 → 7 → 8 → 9
```

Sequential — each task depends on the previous.
