/**
 * canvas-stars.js — Animated star field canvas (Visual Upgrade V2, Req 2)
 *
 * - Fixed canvas behind all content (z-index: 0)
 * - 150–300 star particles depending on viewport size
 * - Orbit + alpha pulse animation per star
 * - Mouse attraction (max 12px within 180px)
 * - 6 slow-rotating ambient brushstroke shapes at opacity 0.06
 * - Trail effect: semi-transparent clear each frame
 * - Resize-safe — recalculates on window.resize
 */
(function () {
  'use strict';

  // ── Canvas Setup ─────────────────────────────────────────
  const canvas = document.getElementById('star-canvas');
  if (!canvas) return;

  const ctx = canvas.getContext('2d');

  let W, H;
  let stars = [];
  let brushstrokes = [];
  let rafId = null;

  const PALETTE = ['#F1C40F', '#F5C842', '#FFD700', '#FDB813', '#FEF08A'];

  // Mouse position — default to off-screen so no attraction on load
  const mouse = { x: -9999, y: -9999 };

  // ── Star Factory ─────────────────────────────────────────
  function createStar(w, h) {
    return {
      x: Math.random() * w,
      y: Math.random() * h,
      radius: 0.5 + Math.random() * 2.5,          // 0.5–3px
      baseAngle: Math.random() * Math.PI * 2,
      orbitRadius: 5 + Math.random() * 25,          // 5–30px
      speed: (0.002 + Math.random() * 0.008) * (Math.random() < 0.5 ? 1 : -1),
      color: PALETTE[Math.floor(Math.random() * PALETTE.length)],
      alpha: 0.3 + Math.random() * 0.7,            // 0.3–1.0
      pulseSpeed: 0.005 + Math.random() * 0.015,
      pulseDir: Math.random() < 0.5 ? 1 : -1
    };
  }

  // ── Brushstroke Shape Factory ─────────────────────────────
  function createBrushstroke(w, h) {
    return {
      x: Math.random() * w,
      y: Math.random() * h,
      rx: 80 + Math.random() * 160,               // ellipse x-radius
      ry: 20 + Math.random() * 60,                // ellipse y-radius
      angle: Math.random() * Math.PI * 2,
      rotSpeed: (0.0002 + Math.random() * 0.0006) * (Math.random() < 0.5 ? 1 : -1),
      color: PALETTE[Math.floor(Math.random() * PALETTE.length)]
    };
  }

  // ── Resize / Init ─────────────────────────────────────────
  function resize() {
    W = canvas.width = window.innerWidth;
    H = canvas.height = window.innerHeight;

    // Recalculate star count clamped 150–300
    const count = Math.min(300, Math.max(150, Math.floor(W * H / 12000)));

    // Rebuild stars preserving count change
    stars = Array.from({ length: count }, () => createStar(W, H));

    // Rebuild 6 brushstrokes
    brushstrokes = Array.from({ length: 6 }, () => createBrushstroke(W, H));
  }

  // ── Render Brushstrokes ───────────────────────────────────
  function drawBrushstrokes() {
    ctx.save();
    ctx.globalAlpha = 0.06;
    brushstrokes.forEach(b => {
      b.angle += b.rotSpeed;
      ctx.save();
      ctx.translate(b.x, b.y);
      ctx.rotate(b.angle);
      ctx.beginPath();
      ctx.ellipse(0, 0, b.rx, b.ry, 0, 0, Math.PI * 2);
      ctx.fillStyle = b.color;
      ctx.fill();
      ctx.restore();
    });
    ctx.globalAlpha = 1;
    ctx.restore();
  }

  // ── Render Stars ──────────────────────────────────────────
  function drawStars() {
    stars.forEach(star => {
      // Advance orbit angle
      star.baseAngle += star.speed;

      // Pulse alpha
      star.alpha += star.pulseSpeed * star.pulseDir;
      if (star.alpha >= 1.0) { star.alpha = 1.0; star.pulseDir = -1; }
      if (star.alpha <= 0.3) { star.alpha = 0.3; star.pulseDir = 1; }

      // Orbit position
      let x = star.x + Math.cos(star.baseAngle) * star.orbitRadius;
      let y = star.y + Math.sin(star.baseAngle) * star.orbitRadius;

      // Mouse attraction offset (max 12px within 180px)
      const dx = mouse.x - star.x;
      const dy = mouse.y - star.y;
      const dist = Math.sqrt(dx * dx + dy * dy);
      if (dist < 180) {
        const pull = (180 - dist) / 180;
        x += Math.cos(star.baseAngle) * pull * 12;
        y += Math.sin(star.baseAngle) * pull * 12;
      }

      // Radial gradient glow
      const glowRadius = star.radius * 4;
      const g = ctx.createRadialGradient(x, y, 0, x, y, glowRadius);
      g.addColorStop(0, star.color);
      g.addColorStop(1, 'transparent');
      ctx.fillStyle = g;
      ctx.beginPath();
      ctx.arc(x, y, glowRadius, 0, Math.PI * 2);
      ctx.fill();

      // White core
      ctx.fillStyle = `rgba(255,255,255,${star.alpha})`;
      ctx.beginPath();
      ctx.arc(x, y, star.radius, 0, Math.PI * 2);
      ctx.fill();
    });
  }

  // ── Main Render Loop ──────────────────────────────────────
  function render() {
    // Trail effect — semi-transparent clear
    ctx.fillStyle = 'rgba(7,11,25,0.25)';
    ctx.fillRect(0, 0, W, H);

    drawBrushstrokes();
    drawStars();

    rafId = requestAnimationFrame(render);
  }

  // ── Mouse Tracking ────────────────────────────────────────
  window.addEventListener('mousemove', function (e) {
    mouse.x = e.clientX;
    mouse.y = e.clientY;
  }, { passive: true });

  window.addEventListener('mouseleave', function () {
    mouse.x = -9999;
    mouse.y = -9999;
  });

  // ── Resize Handler ────────────────────────────────────────
  let resizeTimer;
  window.addEventListener('resize', function () {
    clearTimeout(resizeTimer);
    resizeTimer = setTimeout(function () {
      if (rafId !== null) {
        cancelAnimationFrame(rafId);
        rafId = null;
      }
      resize();
      render();
    }, 150);
  });

  // ── Bootstrap ─────────────────────────────────────────────
  resize();
  render();

})();
