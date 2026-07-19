# Design Document — Van Gogh Hero (Phase 1)

## Philosophy

**Technology living INSIDE the painting, not ON TOP of it.**

Phase 1 goal: ship a complete, polished hero section that feels like Van Gogh × Code, using the minimum viable asset set. No Canvas, no WebGL, no particle systems. Clean HTML + CSS + lightweight JS only.

---

## Asset Map

```
wwwroot/images/
├── raw-vangogh-without-text.png   ← hero background (1536×1024)
└── vangogh-hero.png               ← reference image (with UI overlaid)
```

---

## Layer Architecture

```
z-index stack (bottom to top)
──────────────────────────────────────────────
z: 0   .hero-bg           background-image (raw-vangogh)
z: 1   .hero-overlay      CSS gradient (left dark → right transparent)
z: 2   (reserved for future character/desk layers)
z: 10  .hero-content      all HTML UI: text, buttons, code block
z: 100 #main-navbar       fixed navigation bar
z: 9999 #page-loader      page loader
```

---

## Hero Layout (exact match to reference image)

```
┌─────────────────────────────────────────────────────────────┐
│ NAVBAR (fixed, full-width, 72px, dark bg + blur)            │
│ [Logo]        [Home About Team Portfolio Blog Contact]  [Let's Talk] [🌙] │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  WE ARE                         Van Gogh sky →             │
│                                                             │
│  Code. Design.                  (swirls, moon, stars)       │
│  Create.  ← GOLD, LARGE                                     │
│                                                             │
│  We're a team of passionate     Van Gogh character          │
│  developers...                  sitting at desk →           │
│                                                             │
│  [Explore Our Work →]                                       │
│                                                             │
│ 01 const create = () => {       // The best code            │
│ 02   let ideas = 'limitless';   // is written with          │
│ 03   let passion = true;        // passion.                 │
│ 04   while (passion) { ... }                                │
│                                                             │
│                    ○ SCROLL DOWN                            │
└─────────────────────────────────────────────────────────────┘
```

---

## Color Tokens (Van Gogh Oil Palette)

```css
/* Backgrounds */
--color-canvas-black:  #0A0B0F;   /* near-black */
--color-night-deep:    #0D1117;
--color-prussian:      #1A2744;   /* Prussian blue */
--color-cobalt:        #1E3A5F;   /* cobalt blue */

/* Warm lights */
--color-moon-white:    #F8F4E8;   /* warm white, not LED */
--color-parchment:     #E8DFC8;   /* warm parchment */
--color-straw:         #D4B87A;   /* warm straw */
--color-chrome-yellow: #F5C842;   /* Van Gogh chrome yellow */
--color-ochre:         #C8903A;   /* yellow ochre */

/* Accents */
--color-cerulean:      #4A9BB5;   /* muted blue for buttons */
```

---

## Typography

| Role | Font | Weight | Size |
|------|------|--------|------|
| Eyebrow ("WE ARE") | Manrope | 700 | 0.8rem, letter-spacing 0.25em |
| "Code. Design." | Manrope | 800 | clamp(2.8rem, 6vw, 5.5rem) |
| "Create." | Manrope | 800 | clamp(3.5rem, 8vw, 7rem) |
| Subtitle | IBM Plex Sans | 400 | 1rem–1.15rem |
| Nav links | IBM Plex Sans | 500 | 0.875rem |
| Code snippet | JetBrains Mono | 400 | 0.78rem |

---

## Animations (Phase 1 — 3 only)

### 1. Ken Burns (background breathes)
```css
@keyframes ken-burns {
  from { transform: scale(1.0) translate(0, 0); }
  to   { transform: scale(1.06) translate(-1%, -0.5%); }
}
/* duration: 20s, alternate, ease-in-out */
```

### 2. "Create." gold glow pulse
```css
@keyframes create-glow {
  from { text-shadow: 0 2px 20px rgba(0,0,0,0.6), 0 0 40px rgba(245,200,66,0.25); }
  to   { text-shadow: 0 2px 20px rgba(0,0,0,0.6), 0 0 80px rgba(245,200,66,0.45); }
}
/* duration: 4s, alternate, ease-in-out infinite */
```

### 3. Scroll indicator bounce
```css
@keyframes scroll-bounce {
  0%, 100% { transform: translateX(-50%) translateY(0); opacity: 0.5; }
  50%       { transform: translateX(-50%) translateY(6px); opacity: 0.8; }
}
/* duration: 2s, ease-in-out infinite */
```

### Entrance animations (JS-driven, one-time)
- Eyebrow: fade in from opacity 0, translateY(15px), delay 0ms, 700ms
- Headline: fade in from opacity 0, translateY(20px), delay 100ms, 800ms
- Subtitle: fade in + typewriter, delay 200ms
- Button: fade in from opacity 0, translateY(10px), delay 400ms, 700ms
- Code block: fade in from opacity 0, delay 600ms, 700ms
- Comment right: fade in, delay 800ms, 600ms

---

## Navigation Design

```
Full-width bar, position: fixed, height: 72px
Background: rgba(10,11,15,0.88) + backdrop-filter: blur(12px)
Border-bottom: 1px solid rgba(245,200,66,0.08)

Left:   Logo SVG (constellation dots) + "TeamPortfolio" text
Center: Nav links (Home About Team Portfolio Blog Contact)
Right:  "Let's Talk" ghost button + theme toggle icon

Nav link colors:
  inactive: rgba(248,244,232,0.72)
  hover:    #F8F4E8 (full opacity)
  active:   #F5C842 (chrome yellow) + 4px gold dot above
```

---

## Code Snippet Design

```
Position: absolute, bottom: 80px, left: 8vw
Max-width: 340px
Background: rgba(10,11,15,0.82)
Border: 1px solid rgba(245,200,66,0.12)
Border-radius: 8px
Padding: 16px 20px

Content:
01  const create = () => {
02    let ideas = 'limitless';
03    let passion = true;
04    while (passion) {
05      code();
06      design();
07    }
08  }

Line numbers: rgba(200,144,58,0.5) — muted amber
Keywords (const/let/while): #6BAFC5 — muted cerulean
Strings ('limitless'): #D4B87A — straw yellow
Functions (code/design): #E8DFC8 — parchment
```

---

## Right-Side Comment Block

```
Position: absolute, top: 38%, right: 5%
Font: JetBrains Mono, 0.8rem, italic
Color: rgba(200,180,140,0.55)

Lines:
  // The best code
  // is written with
  // passion.
```

---

## Files to Create/Modify

| File | Action |
|------|--------|
| `wwwroot/css/design-system.css` | ✅ Done — new Van Gogh palette |
| `wwwroot/css/hero.css` | ✅ Done — needs minor tweaks |
| `wwwroot/css/navigation.css` | ✅ Done — needs minor tweaks |
| `wwwroot/css/background-system.css` | Simplify — remove 8-layer CSS bg system |
| `wwwroot/css/motion.css` | Update — simpler entrance animations |
| `wwwroot/js/background.js` | Simplify — remove star/particle DOM generation |
| `wwwroot/js/motion.js` | Update — keep entrance + typewriter, add parallax |
| `Views/Home/_HeroSection.cshtml` | Rewrite — new HTML structure |
| `Views/Shared/_Navigation.cshtml` | Minor update — style classes |
| `Views/Shared/_Layout.cshtml` | Remove background-system markup |

---

## Phase 2 (Future — do not implement now)

- Separate Van Gogh character as transparent PNG layer
- Separate desk/lantern layer
- Add mouse-move multi-layer parallax
- Add laptop screen glow animation

## Phase 3 (Future)

- Canvas particle system for stars
- Scroll-triggered animations
- More interactive elements
