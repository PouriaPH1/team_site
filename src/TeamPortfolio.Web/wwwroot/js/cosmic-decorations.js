/**
 * cosmic-decorations.js — Dynamic SVG Decoration Generator
 * 
 * Adds atmospheric SVG elements throughout the page:
 * - Floating stars (small, medium, large)
 * - Constellation connection lines
 * - Flowing curves
 * - Radial glows
 * 
 * These elements enhance the "Starry Night" atmosphere without cluttering content.
 */
(function () {
  'use strict';

  // Configuration — Van Gogh inspired
  const CONFIG = {
    stars: {
      small: { count: 18, size: 2, color: '#F5C842' },
      medium: { count: 8, size: 4, color: '#FFD700' },
      large: { count: 4, size: 6, color: '#FEF08A' }
    },
    constellations: 0, // Disabled — Van Gogh doesn't have constellation lines
    curves: 6, // More swirling curves
    glows: 0  // Disabled — using blob shapes in CSS instead
  };

  // Helper: Create SVG element
  function createSVG(tag, attrs = {}) {
    const el = document.createElementNS('http://www.w3.org/2000/svg', tag);
    Object.entries(attrs).forEach(([key, val]) => el.setAttribute(key, val));
    return el;
  }

  // Helper: Random position within viewport
  function randomPos(exclude = { top: 0, bottom: 0 }) {
    const vw = window.innerWidth;
    const vh = window.innerHeight;
    return {
      x: Math.random() * vw,
      y: exclude.top + Math.random() * (vh - exclude.top - exclude.bottom)
    };
  }

  // Generate glowing star SVG
  function createStar(size, color, position) {
    const container = document.createElement('div');
    container.className = 'cosmic-star cosmic-star--pulse';
    container.style.cssText = `left: ${position.x}px; top: ${position.y}px;`;

    const svg = createSVG('svg', {
      width: size * 3,
      height: size * 3,
      viewBox: `0 0 ${size * 3} ${size * 3}`
    });

    // Outer glow
    const glow = createSVG('circle', {
      cx: size * 1.5,
      cy: size * 1.5,
      r: size * 1.2,
      fill: color,
      opacity: '0.3'
    });

    // Core
    const core = createSVG('circle', {
      cx: size * 1.5,
      cy: size * 1.5,
      r: size / 2,
      fill: '#FFFFFF'
    });

    svg.appendChild(glow);
    svg.appendChild(core);
    container.appendChild(svg);

    return container;
  }

  // Generate constellation (connected stars)
  function createConstellation(pointCount = 4) {
    const container = document.createElement('div');
    container.className = 'constellation-group';

    const baseX = Math.random() * (window.innerWidth - 400);
    const baseY = Math.random() * (window.innerHeight - 300);

    container.style.cssText = `left: ${baseX}px; top: ${baseY}px;`;

    const svg = createSVG('svg', {
      width: 400,
      height: 300,
      viewBox: '0 0 400 300'
    });

    // Generate random points
    const points = Array.from({ length: pointCount }, () => ({
      x: 50 + Math.random() * 300,
      y: 50 + Math.random() * 200
    }));

    // Draw connection lines
    for (let i = 0; i < points.length - 1; i++) {
      const line = createSVG('line', {
        x1: points[i].x,
        y1: points[i].y,
        x2: points[i + 1].x,
        y2: points[i + 1].y,
        stroke: '#38BDF8',
        'stroke-width': '0.5',
        opacity: '0.4'
      });
      svg.appendChild(line);
    }

    // Draw star points
    points.forEach(p => {
      const star = createSVG('circle', {
        cx: p.x,
        cy: p.y,
        r: 2,
        fill: '#F5C842'
      });
      svg.appendChild(star);
    });

    container.appendChild(svg);
    return container;
  }

  // Generate flowing curve — Van Gogh swirl style
  function createCurve() {
    const container = document.createElement('div');
    container.className = 'cosmic-curve';

    const x = Math.random() * window.innerWidth;
    const y = Math.random() * window.innerHeight;

    container.style.cssText = `left: ${x}px; top: ${y}px;`;

    const svg = createSVG('svg', {
      width: 800,
      height: 400,
      viewBox: '0 0 800 400'
    });

    // Van Gogh golden swirl colors only
    const colors = ['#F5C842', '#FFD700', '#F1C40F'];
    const color = colors[Math.floor(Math.random() * colors.length)];

    // More dramatic swirl curve
    const path = createSVG('path', {
      d: `M 0 200 Q ${Math.random() * 400} ${Math.random() * 400}, 800 200`,
      stroke: color,
      'stroke-width': '1.5',
      fill: 'none',
      opacity: '0.25'
    });

    svg.appendChild(path);
    container.appendChild(svg);
    return container;
  }

  // Generate radial glow
  function createGlow(color) {
    const glow = document.createElement('div');
    glow.className = 'cosmic-glow';

    const x = 10 + Math.random() * 80; // 10-90%
    const y = 10 + Math.random() * 80;
    const size = 300 + Math.random() * 200;

    glow.style.cssText = `
      left: ${x}%;
      top: ${y}%;
      width: ${size}px;
      height: ${size}px;
      background: radial-gradient(circle, ${color} 0%, transparent 70%);
    `;

    return glow;
  }

  // Main initialization
  function init() {
    // Only run on desktop
    if (window.innerWidth < 768) return;

    const body = document.body;

    // Add stars
    ['small', 'medium', 'large'].forEach(type => {
      const config = CONFIG.stars[type];
      for (let i = 0; i < config.count; i++) {
        const pos = randomPos({ top: 100, bottom: 100 }); // Avoid header/footer
        const star = createStar(config.size, config.color, pos);
        star.style.animationDelay = `${Math.random() * 2}s`;
        body.appendChild(star);
      }
    });

    // Add constellations
    for (let i = 0; i < CONFIG.constellations; i++) {
      const constellation = createConstellation(3 + Math.floor(Math.random() * 3));
      constellation.style.animationDelay = `${Math.random() * 5}s`;
      body.appendChild(constellation);
    }

    // Add flowing curves
    for (let i = 0; i < CONFIG.curves; i++) {
      const curve = createCurve();
      curve.style.animationDelay = `${Math.random() * 10}s`;
      body.appendChild(curve);
    }

    // Add radial glows
    const glowColors = [
      'rgba(245, 200, 66, 0.08)',
      'rgba(59, 130, 246, 0.06)',
      'rgba(139, 92, 246, 0.05)'
    ];

    glowColors.forEach((color, i) => {
      const glow = createGlow(color);
      glow.style.animationDelay = `${i * 2}s`;
      body.appendChild(glow);
    });
  }

  // Bootstrap
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }

})();
