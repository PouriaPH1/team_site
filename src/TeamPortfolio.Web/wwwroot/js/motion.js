/* ==========================================================================
   motion.js — Van Gogh Motion System (Phase 1)
   
   Responsibilities:
   1. Hero entrance animations (stagger on load)
   2. Typewriter effect on subtitle
   3. Parallax on background image (mouse-move)
   4. Scroll indicator hide on scroll
   5. Mobile drawer open/close + focus trap
   6. Magnetic button on nav CTA
   7. Reduced-motion gate
   ========================================================================== */

(function () {
  'use strict';

  /* ── State ─────────────────────────────────────────────── */
  var isRunning = false;
  var parallaxRafId = null;
  var parallaxDirty = false;
  var lastMouse = null;
  var magneticRafIds = new WeakMap();
  var activeObservers = [];

  /* ── Reduced Motion Gate ────────────────────────────────── */
  function handleReducedMotion() {
    var prefersReduced = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    var lowPerf = (navigator.hardwareConcurrency || 4) < 4;

    if (prefersReduced || lowPerf) {
      // Snap all reveal elements immediately
      document.querySelectorAll('[data-reveal]').forEach(function (el) {
        el.classList.add('revealed');
        el.setAttribute('data-revealed', 'true');
      });
      activeObservers.forEach(function (obs) { obs.disconnect(); });
      activeObservers = [];
      return true;
    }
    return false;
  }

  /* ── Scroll Reveal ──────────────────────────────────────── */
  function initScrollReveal() {
    var elements = document.querySelectorAll('[data-reveal]');
    if (!elements.length) return;

    var observer = new IntersectionObserver(function (entries) {
      entries.forEach(function (entry) {
        if (entry.isIntersecting) {
          entry.target.classList.add('revealed');
          entry.target.setAttribute('data-revealed', 'true');
          observer.unobserve(entry.target);
        }
      });
    }, { threshold: 0.15 });

    activeObservers.push(observer);

    // Stagger index for groups
    document.querySelectorAll('[data-reveal-group]').forEach(function (group) {
      group.querySelectorAll('[data-reveal]').forEach(function (child, idx) {
        child.style.setProperty('--stagger-index', Math.min(idx, 6));
      });
    });

    elements.forEach(function (el) { observer.observe(el); });
  }

  /* ── Hero Entrance Animations ───────────────────────────── */
  function initHeroEntrance() {
    var hero = document.getElementById('hero-section');
    if (!hero) return;

    var eyebrow  = hero.querySelector('.hero-eyebrow');
    var headline = hero.querySelector('.hero-headline-block');
    var subtitle = hero.querySelector('.hero-subtitle');
    var ctaRow   = hero.querySelector('.hero-cta-row');
    var codeBlock = hero.querySelector('.hero-code-block');
    var comments  = hero.querySelector('.hero-comments-right');
    var scrollInd = hero.querySelector('.hero-scroll-indicator');

    var sequence = [
      { el: eyebrow,   delay: 0   },
      { el: headline,  delay: 100 },
      { el: subtitle,  delay: 200 },
      { el: ctaRow,    delay: 400 },
      { el: codeBlock, delay: 600 },
      { el: comments,  delay: 800 },
      { el: scrollInd, delay: 900 }
    ];

    sequence.forEach(function (item) {
      if (!item.el) return;

      // Set initial state
      item.el.style.opacity = '0';
      item.el.style.transform = 'translateY(18px)';
      item.el.style.transition =
        'opacity 700ms cubic-bezier(0.25,0.46,0.45,0.94), ' +
        'transform 700ms cubic-bezier(0.25,0.46,0.45,0.94)';

      setTimeout(function () {
        if (!isRunning) return;
        item.el.style.opacity = '1';
        item.el.style.transform = 'translateY(0)';
        item.el.setAttribute('data-revealed', 'true');
      }, item.delay);
    });

    // Typewriter on subtitle
    var text = subtitle && subtitle.getAttribute('data-typewriter');
    if (text && subtitle) {
      setTimeout(function () {
        initTypewriter(subtitle, text, 3000);
      }, 300);
    }

    // Hide scroll indicator when hero leaves viewport
    if (scrollInd) {
      var heroObs = new IntersectionObserver(function (entries) {
        if (!entries[0].isIntersecting) {
          scrollInd.classList.add('hidden');
        } else {
          scrollInd.classList.remove('hidden');
        }
      }, { threshold: 0.1 });
      activeObservers.push(heroObs);
      heroObs.observe(hero);
    }
  }

  /* ── Typewriter ─────────────────────────────────────────── */
  function initTypewriter(el, text, duration) {
    if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
      el.textContent = text;
      return;
    }

    var charDelay = duration / text.length;
    var idx = 0;
    el.textContent = '';

    function typeNext() {
      if (idx < text.length) {
        el.textContent += text[idx];
        idx++;
        setTimeout(typeNext, charDelay);
      }
    }
    typeNext();
  }

  /* ── Background Parallax (mouse-move) ───────────────────── */
  /* Subtle shift of background-position on mouse move.
     This gives depth illusion without needing separate image layers.
     Max shift: ±15px — subtle, not distracting. */
  function initParallax() {
    var hero = document.getElementById('hero-section');
    var bg = hero && hero.querySelector('.hero-bg');
    if (!hero || !bg) return;

    hero.addEventListener('mousemove', function (e) {
      lastMouse = e;
      if (!parallaxDirty) {
        parallaxDirty = true;
        parallaxRafId = requestAnimationFrame(updateParallax);
      }
    });

    hero.addEventListener('mouseleave', function () {
      lastMouse = null;
      if (parallaxRafId) {
        cancelAnimationFrame(parallaxRafId);
        parallaxRafId = null;
        parallaxDirty = false;
      }
      // Smoothly reset background position
      bg.style.transition = 'transform 600ms cubic-bezier(0.25,0.46,0.45,0.94)';
      bg.style.transform = 'scale(1.0) translate(0, 0)';
      setTimeout(function () {
        // Restore Ken Burns animation
        bg.style.transition = '';
        bg.style.transform = '';
      }, 650);
    });
  }

  function updateParallax() {
    parallaxDirty = false;
    if (!lastMouse || !isRunning) return;

    var hero = document.getElementById('hero-section');
    var bg = hero && hero.querySelector('.hero-bg');
    if (!bg) return;

    var rect = hero.getBoundingClientRect();
    var cx = rect.width / 2;
    var cy = rect.height / 2;
    var dx = lastMouse.clientX - rect.left - cx;
    var dy = lastMouse.clientY - rect.top  - cy;

    // Normalize to [-1, 1] then scale to max 15px
    var tx = (dx / cx) * -12;  /* negative: bg moves opposite to cursor */
    var ty = (dy / cy) * -8;

    bg.style.transition = 'none';
    bg.style.transform = 'scale(1.04) translate(' + tx + 'px, ' + ty + 'px)';
  }

  /* ── Magnetic Button ────────────────────────────────────── */
  function initMagneticButton(btn) {
    if (!btn) return;

    btn.addEventListener('mousemove', function (e) {
      var rect = btn.getBoundingClientRect();
      var cx = rect.left + rect.width  / 2;
      var cy = rect.top  + rect.height / 2;
      var dx = e.clientX - cx;
      var dy = e.clientY - cy;
      var dist = Math.hypot(dx, dy);

      if (dist < 60) {
        var tx = (dx / dist) * Math.min(dist * 0.35, 7);
        var ty = (dy / dist) * Math.min(dist * 0.35, 7);
        var raf = magneticRafIds.get(btn);
        if (raf) cancelAnimationFrame(raf);
        magneticRafIds.set(btn, requestAnimationFrame(function () {
          btn.style.transition = 'none';
          btn.style.transform = 'translate(' + tx + 'px,' + ty + 'px)';
        }));
      }
    });

    btn.addEventListener('mouseleave', function () {
      var raf = magneticRafIds.get(btn);
      if (raf) cancelAnimationFrame(raf);
      btn.style.transition = 'transform 400ms cubic-bezier(0.25,0.46,0.45,0.94)';
      btn.style.transform = 'translate(0,0)';
    });
  }

  /* ── Focus Trap for Drawer ──────────────────────────────── */
  function initDrawerFocusTrap(drawer) {
    if (!drawer) return;

    var FOCUSABLE = [
      'a[href]', 'button:not([disabled])',
      '[tabindex]:not([tabindex="-1"])'
    ].join(',');

    drawer.addEventListener('keydown', function (e) {
      if (!drawer.classList.contains('open') || e.key !== 'Tab') return;

      var focusable = Array.from(drawer.querySelectorAll(FOCUSABLE))
        .filter(function (el) { return el.offsetParent !== null; });
      if (!focusable.length) return;

      var first = focusable[0];
      var last  = focusable[focusable.length - 1];

      if (e.shiftKey && document.activeElement === first) {
        e.preventDefault(); last.focus();
      } else if (!e.shiftKey && document.activeElement === last) {
        e.preventDefault(); first.focus();
      }
    });
  }

  /* ── Mobile Drawer ──────────────────────────────────────── */
  function initMobileDrawer() {
    var drawer    = document.getElementById('mobile-drawer');
    var overlay   = document.getElementById('mobile-drawer-overlay');
    var hamburger = document.querySelector('.nav-hamburger') ||
                    document.getElementById('mobile-menu-btn');
    var closeBtn  = document.querySelector('.mobile-drawer-close') ||
                    document.getElementById('mobile-drawer-close');
    var navLinks  = document.querySelectorAll('.mobile-nav-link');

    if (!drawer) return;
    initDrawerFocusTrap(drawer);

    function openDrawer() {
      drawer.classList.add('open');
      if (overlay) overlay.classList.add('show');
      document.body.classList.add('drawer-open');
      if (hamburger) {
        hamburger.setAttribute('aria-expanded', 'true');
        hamburger.setAttribute('aria-label', 'Close navigation menu');
      }
      drawer.setAttribute('aria-hidden', 'false');
      if (closeBtn) closeBtn.focus();
    }

    function closeDrawer() {
      drawer.classList.remove('open');
      if (overlay) overlay.classList.remove('show');
      document.body.classList.remove('drawer-open');
      if (hamburger) {
        hamburger.setAttribute('aria-expanded', 'false');
        hamburger.setAttribute('aria-label', 'Open navigation menu');
      }
      drawer.setAttribute('aria-hidden', 'true');
      if (hamburger) hamburger.focus();
    }

    if (hamburger) hamburger.addEventListener('click', openDrawer);
    if (closeBtn)  closeBtn.addEventListener('click', closeDrawer);
    if (overlay)   overlay.addEventListener('click', closeDrawer);

    navLinks.forEach(function (link) {
      link.addEventListener('click', closeDrawer);
    });

    document.addEventListener('keydown', function (e) {
      if (e.key === 'Escape' && drawer.classList.contains('open')) {
        closeDrawer();
      }
    });
  }

  /* ── Lifecycle ──────────────────────────────────────────── */
  function pause() {
    isRunning = false;
    if (parallaxRafId) {
      cancelAnimationFrame(parallaxRafId);
      parallaxRafId = null;
      parallaxDirty = false;
    }
  }

  function resume() { isRunning = true; }

  /* ── Init ───────────────────────────────────────────────── */
  function init() {
    // Mark HTML as JS-ready (activates CSS reveal states)
    document.documentElement.setAttribute('data-js-ready', '');
    isRunning = true;

    // Reduced motion / low perf gate
    if (handleReducedMotion()) {
      // Still init drawer and magnetic for accessibility
      initMobileDrawer();
      initMagneticButton(document.querySelector('.nav-cta-magnetic'));
      return;
    }

    // Core animations
    initHeroEntrance();
    initScrollReveal();
    initParallax();

    // Interactive
    initMagneticButton(document.querySelector('.nav-cta-magnetic'));
    initMobileDrawer();

    // Page Visibility — pause Ken Burns + parallax when tab hidden
    document.addEventListener('visibilitychange', function () {
      if (document.hidden) { pause(); } else { resume(); }
    });
  }

  /* ── Public API ─────────────────────────────────────────── */
  window.MotionSystem = { init: init, pause: pause, resume: resume };

  /* Auto-init after DOM ready */
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function () {
      setTimeout(init, 0);
    });
  } else {
    setTimeout(init, 0);
  }

}());
