# Design Document — Visual Upgrade V2

## Overview

This document defines the technical architecture for implementing all requirements in the Visual Upgrade V2 spec. The implementation targets ASP.NET Core 9 MVC with vanilla JS — no React, no build tools, no new npm packages.

---

## Architecture Overview

```
wwwroot/
├── css/
│   ├── design-system.css     ← ADD: .glass-panel, .glass-card, .section-glow-*
│   ├── hero.css              ← MODIFY: overlay opacity fix, stats row, code titlebar
│   ├── terminal.css          ← NEW: terminal section styles
│   └── skills.css            ← NEW: constellation grid styles
├── js/
│   ├── canvas-stars.js       ← NEW: star field canvas animation
│   ├── scroll-reveal.js      ← NEW: IntersectionObserver reveal system
│   ├── terminal.js           ← NEW: terminal command handler
│   ├── hero-interactions.js  ← NEW: run button, typewriter, copy
│   └── site.js               ← MODIFY: initialize all modules
└── Views/
    ├── Home/
    │   ├── _HeroSection.cshtml   ← MODIFY: overlay fix, stats row, code titlebar
    │   ├── _TerminalSection.cshtml ← NEW: terminal UI
    │   ├── _SkillsSection.cshtml   ← NEW: constellation grid
    │   └── Index.cshtml          ← MODIFY: include new sections
    └── Shared/
        └── _Navigation.cshtml    ← MODIFY: remove dev guide button
```

---

## Component 1: Hero Overlay Fix

### Problem
Current overlay is: `rgba(10,11,15,0.88)` at 0% → transparent at 100%. This makes the right side (Van Gogh) too dark.

### Solution — hero.css changes

```css
/* BEFORE */
.hero-overlay {
  background: linear-gradient(90deg,
    rgba(10,11,15,0.88) 0%,
    rgba(10,11,15,0.75) 30%,
    rgba(10,11,15,0.40) 55%,
    rgba(10,11,15,0.10) 75%,
    transparent 100%
  );
}

/* AFTER */
.hero-overlay {
  background: linear-gradient(90deg,
    rgba(10,11,15,0.90) 0%,
    rgba(10,11,15,0.60) 35%,
    rgba(10,11,15,0.20) 55%,
    rgba(10,11,15,0.05) 70%,
    transparent 100%
  );
}

/* Bottom overlay — reduce height from 50% to 40% */
.hero-overlay-bottom {
  height: 40%;  /* was 50% */
  background: linear-gradient(0deg,
    rgba(10,11,15,0.75) 0%,  /* was 0.82 */
    rgba(10,11,15,0.35) 40%,
    transparent 100%
  );
}
```

---

## Component 2: Hero Code Block — Interactive Titlebar

### HTML Structure (_HeroSection.cshtml)

```html
<div class="hero-code-block" data-reveal>
  <div class="hero-code-inner">
    <!-- Titlebar -->
    <div class="hero-code-titlebar">
      <div class="hero-code-dots">
        <span class="dot dot-red"></span>
        <span class="dot dot-yellow"></span>
        <span class="dot dot-green"></span>
        <span class="hero-code-filename">starry_create.js</span>
      </div>
      <div class="hero-code-actions">
        <button class="hero-code-copy" title="Copy code" aria-label="Copy code">
          <!-- copy SVG icon -->
        </button>
        <button class="hero-code-run" aria-label="Run code">
          <!-- play SVG icon --> Run
        </button>
      </div>
    </div>
    <!-- Code content -->
    <pre class="hero-code-snippet">...</pre>
    <!-- Output panel (hidden by default) -->
    <div class="hero-code-output" aria-live="polite" hidden>
      <span class="output-line"></span>
    </div>
  </div>
</div>
```

### JS Logic (hero-interactions.js)

```javascript
// Run button click handler
runBtn.addEventListener('click', () => {
  outputPanel.hidden = false;
  const lines = [
    '> Running starry_create.js...',
    '[IDEAS: LIMITLESS | STATUS: CREATING...]',
    '> Innovation loop started ✓',
    '> Output: Building something beautiful.'
  ];
  typewriterLines(outputPanel, lines, 40); // 40ms per char
});
```

---

## Component 3: Canvas Star Field (canvas-stars.js)

### Data Model

```javascript
const Star = {
  x, y,           // base position
  radius,          // 0.5–3px
  baseAngle,       // current orbit angle
  orbitRadius,     // 5–30px  
  speed,           // orbit speed (positive or negative)
  color,           // one of 5 palette colors
  alpha,           // 0.3–1.0, pulsing
  pulseSpeed       // alpha pulse rate
}
```

### Render Loop

```javascript
function render() {
  // Trail effect — semi-transparent clear
  ctx.fillStyle = 'rgba(7,11,25,0.25)';
  ctx.fillRect(0, 0, W, H);

  // Mouse attraction
  stars.forEach(star => {
    star.baseAngle += star.speed;
    // orbit position
    let x = star.x + Math.cos(star.baseAngle) * star.orbitRadius;
    let y = star.y + Math.sin(star.baseAngle) * star.orbitRadius;
    // mouse attraction offset (max 12px within 180px)
    const dx = mouse.x - star.x;
    const dy = mouse.y - star.y;
    const dist = Math.sqrt(dx*dx + dy*dy);
    if (dist < 180) {
      const pull = (180-dist)/180;
      x += Math.cos(star.baseAngle) * pull * 12;
      y += Math.sin(star.baseAngle) * pull * 12;
    }
    // radial gradient glow
    const g = ctx.createRadialGradient(x,y,0,x,y,star.radius*4);
    g.addColorStop(0, star.color);
    g.addColorStop(1, 'transparent');
    ctx.fillStyle = g;
    ctx.beginPath();
    ctx.arc(x, y, star.radius*4, 0, Math.PI*2);
    ctx.fill();
    // white core
    ctx.fillStyle = `rgba(255,255,255,${star.alpha})`;
    ctx.beginPath();
    ctx.arc(x, y, star.radius, 0, Math.PI*2);
    ctx.fill();
  });
  requestAnimationFrame(render);
}
```

### Canvas Initialization (in _Layout.cshtml)

```html
<!-- Added before </body> -->
<canvas id="star-canvas" aria-hidden="true"
        style="position:fixed;inset:0;z-index:0;pointer-events:none;opacity:0.75"></canvas>
<script src="~/js/canvas-stars.js" defer></script>
```

---

## Component 4: Scroll Reveal System (scroll-reveal.js)

### CSS Initial State (motion.css)

```css
[data-reveal] {
  opacity: 0;
  transform: translateY(24px);
  transition: opacity 600ms ease-out, transform 600ms ease-out;
}
[data-reveal].revealed {
  opacity: 1;
  transform: translateY(0);
}
/* Stagger via CSS custom property */
[data-reveal-delay="1"] { transition-delay: 100ms; }
[data-reveal-delay="2"] { transition-delay: 200ms; }
[data-reveal-delay="3"] { transition-delay: 300ms; }
[data-reveal-delay="4"] { transition-delay: 400ms; }
```

### JS Observer (scroll-reveal.js)

```javascript
const observer = new IntersectionObserver((entries) => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      entry.target.classList.add('revealed');
      observer.unobserve(entry.target); // once only
    }
  });
}, { threshold: 0.15 });

document.querySelectorAll('[data-reveal]').forEach(el => observer.observe(el));
```

---

## Component 5: Terminal Section

### HTML Structure (_TerminalSection.cshtml)

```html
<section id="terminal" class="terminal-section">
  <div class="container-main">
    <!-- Section header -->
    <div class="section-glow-amber" style="top:-50px;right:0"></div>
    <div data-reveal class="section-eyebrow">Team Explorer</div>
    <h2 data-reveal>Meet the Team</h2>

    <!-- Terminal window -->
    <div class="terminal-window" data-reveal>
      <!-- Titlebar -->
      <div class="terminal-titlebar">
        <div class="terminal-dots">
          <span class="dot dot-red"></span>
          <span class="dot dot-yellow"></span>
          <span class="dot dot-green"></span>
        </div>
        <span class="terminal-title">team@starry:~$</span>
      </div>

      <!-- Output area -->
      <div class="terminal-body" id="terminal-output" role="log" aria-live="polite">
        <!-- JS renders output lines here -->
      </div>

      <!-- Input row -->
      <div class="terminal-input-row">
        <span class="terminal-prompt">$</span>
        <input type="text" id="terminal-input"
               class="terminal-input"
               autocomplete="off"
               autocorrect="off"
               spellcheck="false"
               aria-label="Terminal command input"
               placeholder="Type a command (try 'help')..." />
      </div>
    </div>
  </div>
</section>
```

### Team Data (terminal.js)

```javascript
const TEAM = [
  {
    id: 'leon',
    name: 'Leon',
    role: 'Full-Stack Developer & Architect',
    skills: ['C#', 'ASP.NET Core', 'React', 'SQL Server', 'Azure'],
    experience: '8 years',
    bio: 'Architect of scalable systems. Believes clean code is a form of art.'
  },
  // ... other members
];
```

### Command Handlers (terminal.js)

```javascript
const COMMANDS = {
  help: () => `Available commands:\n  ls        - List all team members\n  cat [name] - View member profile\n  whoami    - Team philosophy\n  clear     - Clear terminal`,
  ls: () => TEAM.map(m => `  ${m.id.padEnd(12)} ${m.role}`).join('\n'),
  cat: (args) => {
    const member = TEAM.find(m => m.id === args[0]);
    if (!member) return `cat: ${args[0]}: No such team member`;
    return `Name: ${member.name}\nRole: ${member.role}\nExp:  ${member.experience}\nSkills: ${member.skills.join(', ')}\n\n${member.bio}`;
  },
  whoami: () => `"The best code is written with passion."`,
  clear: () => { clearOutput(); return null; }
};
```

---

## Component 6: Skills Constellation Grid

### HTML Structure (_SkillsSection.cshtml)

```html
<section id="skills" class="skills-section">
  <div class="container-main">
    <div class="section-glow-blue" style="bottom:0;left:0"></div>
    <div data-reveal class="section-eyebrow">Tech Stack</div>
    <h2 data-reveal>Technologies We Master</h2>

    <!-- Category filters (optional) -->
    <div class="skills-filters" data-reveal>
      <button class="skills-filter active" data-cat="all">All</button>
      <button class="skills-filter" data-cat="frontend">Frontend</button>
      <button class="skills-filter" data-cat="backend">Backend</button>
      <button class="skills-filter" data-cat="mobile">Mobile</button>
      <button class="skills-filter" data-cat="devops">DevOps</button>
    </div>

    <!-- Skills grid -->
    <div class="skills-grid">
      <!-- Each card: -->
      <div class="skill-card glass-card" data-cat="frontend" data-reveal data-reveal-delay="1">
        <div class="skill-icon"><!-- SVG --></div>
        <span class="skill-name">React</span>
        <span class="skill-badge badge-frontend">Frontend</span>
      </div>
      <!-- ... more cards -->
    </div>
  </div>
</section>
```

### CSS (skills.css)

```css
.skills-grid {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 16px;
}

.skill-card {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
  padding: 20px 12px;
  border-radius: 14px;
  cursor: default;
  transition: border-color 200ms, box-shadow 200ms, transform 200ms;
}

.skill-card:hover {
  transform: translateY(-3px);
}

.skill-icon { width: 40px; height: 40px; }
.skill-name { font-size: 0.8rem; font-weight: 600; color: var(--color-moon-white); }

/* Category badge colors */
.badge-frontend { background: rgba(56,189,248,0.15); color: #38BDF8; border: 1px solid rgba(56,189,248,0.3); }
.badge-backend  { background: rgba(139,92,246,0.15); color: #A78BFA; border: 1px solid rgba(139,92,246,0.3); }
.badge-mobile   { background: rgba(245,158,11,0.15); color: #F5C842; border: 1px solid rgba(245,158,11,0.3); }
.badge-devops   { background: rgba(16,185,129,0.15); color: #10B981; border: 1px solid rgba(16,185,129,0.3); }
.badge-design   { background: rgba(244,63,94,0.15);  color: #FB7185; border: 1px solid rgba(244,63,94,0.3);  }

@media (max-width: 1024px) { .skills-grid { grid-template-columns: repeat(3, 1fr); } }
@media (max-width: 768px)  { .skills-grid { grid-template-columns: repeat(2, 1fr); } }
```

---

## Component 7: Hero Stats Row

### HTML addition to _HeroSection.cshtml

```html
<!-- Add after .hero-cta-row -->
<div class="hero-stats-row" data-reveal>
  <div class="hero-stat">
    <span class="hero-stat-num">+48</span>
    <span class="hero-stat-label">Projects Delivered</span>
  </div>
  <div class="hero-stat-divider"></div>
  <div class="hero-stat">
    <span class="hero-stat-num">99%</span>
    <span class="hero-stat-label">Client Satisfaction</span>
  </div>
  <div class="hero-stat-divider"></div>
  <div class="hero-stat">
    <span class="hero-stat-num">24/7</span>
    <span class="hero-stat-label">Support & Development</span>
  </div>
</div>
```

### CSS addition to hero.css

```css
.hero-stats-row {
  display: grid;
  grid-template-columns: 1fr auto 1fr auto 1fr;
  align-items: center;
  gap: 0;
  padding-top: 16px;
  border-top: 1px solid rgba(248,244,232,0.12);
  margin-top: 8px;
  max-width: 400px;
}
.hero-stat { text-align: center; }
.hero-stat-num {
  display: block;
  font-family: var(--font-mono);
  font-size: 1.5rem;
  font-weight: 800;
  color: var(--color-chrome-yellow);
}
.hero-stat-label {
  display: block;
  font-size: 0.68rem;
  color: var(--color-parchment);
  opacity: 0.8;
}
.hero-stat-divider {
  width: 1px;
  height: 32px;
  background: rgba(248,244,232,0.15);
  margin: 0 12px;
}
```

---

## Component 8: Glass Card & Glow System (design-system.css additions)

```css
/* ── Glass Components ─────────── */
.glass-panel {
  -webkit-backdrop-filter: blur(16px);
  backdrop-filter: blur(16px);
  background: rgba(10,16,35,0.55);
  border: 1px solid rgba(245,200,66,0.15);
}

.glass-card {
  -webkit-backdrop-filter: blur(14px);
  backdrop-filter: blur(14px);
  background: linear-gradient(135deg, rgba(12,19,43,0.65), rgba(22,33,62,0.45));
  border: 1px solid rgba(255,255,255,0.08);
  transition: border-color 200ms ease, box-shadow 200ms ease;
}
.glass-card:hover {
  border-color: rgba(245,200,66,0.35);
  box-shadow: 0 12px 35px -10px rgba(245,200,66,0.22);
}

/* ── Section Glow Blobs ───────── */
.section-glow-amber,
.section-glow-blue {
  position: absolute;
  width: 500px;
  height: 500px;
  border-radius: 50%;
  pointer-events: none;
  filter: blur(60px);
  z-index: 0;
}
.section-glow-amber {
  background: radial-gradient(circle, rgba(245,158,11,0.12), transparent 70%);
}
.section-glow-blue {
  background: radial-gradient(circle, rgba(56,189,248,0.10), transparent 70%);
}
```

---

## Implementation Order (Recommended)

1. **Req 9** — Navbar cleanup (5 min, trivial, immediate visual win)
2. **Req 1** — Hero overlay fix (10 min, biggest visual impact)
3. **Req 7 + 8** — Glass card + glow system in design-system.css (15 min)
4. **Req 10** — Hero stats row (20 min)
5. **Req 3** — Code snippet interactive titlebar (30 min)
6. **Req 2** — Canvas star field (45 min, most complex JS)
7. **Req 6** — Scroll reveal system (30 min)
8. **Req 5** — Skills constellation grid (45 min)
9. **Req 4** — Terminal team explorer (60 min, most complex feature)

**Total estimated: ~4 hours of focused implementation**

---

## Files to Create

| File | Type | Purpose |
|------|------|---------|
| `wwwroot/js/canvas-stars.js` | NEW | Star field canvas |
| `wwwroot/js/scroll-reveal.js` | NEW | IntersectionObserver reveals |
| `wwwroot/js/terminal.js` | NEW | Terminal command system |
| `wwwroot/js/hero-interactions.js` | NEW | Run button, copy, typewriter |
| `wwwroot/css/terminal.css` | NEW | Terminal section styles |
| `wwwroot/css/skills.css` | NEW | Skills grid styles |
| `Views/Home/_TerminalSection.cshtml` | NEW | Terminal HTML |
| `Views/Home/_SkillsSection.cshtml` | NEW | Skills grid HTML |

## Files to Modify

| File | Changes |
|------|---------|
| `wwwroot/css/hero.css` | Overlay fix, stats row, code titlebar |
| `wwwroot/css/design-system.css` | glass-panel, glass-card, glow blobs |
| `Views/Home/_HeroSection.cshtml` | Stats row, code titlebar HTML |
| `Views/Home/Index.cshtml` | Include new sections |
| `Views/Shared/_Navigation.cshtml` | Remove dev guide button |
| `Views/Shared/_Layout.cshtml` | Add canvas element, new CSS/JS links |
