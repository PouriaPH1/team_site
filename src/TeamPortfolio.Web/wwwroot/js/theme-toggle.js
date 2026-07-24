(function() {
  const THEME_KEY = 'tp-theme';

  function getTheme() {
    return localStorage.getItem(THEME_KEY) ||
      (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');
  }

  function applyTheme(theme) {
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem(THEME_KEY, theme);
    // Update desktop button icon
    const btn = document.getElementById('theme-toggle-btn');
    if (btn) {
      const icon = btn.querySelector('i');
      if (icon) {
        icon.className = theme === 'dark' ? 'fa-solid fa-sun' : 'fa-solid fa-moon';
      }
      btn.setAttribute('aria-label', theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode');
    }
    // Update mobile drawer button
    const mobileBtn = document.getElementById('mobile-theme-toggle-btn');
    if (mobileBtn) {
      mobileBtn.setAttribute('aria-label', theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode');
    }
  }

  function toggleTheme() {
    const current = getTheme();
    applyTheme(current === 'dark' ? 'light' : 'dark');
  }

  document.addEventListener('DOMContentLoaded', function() {
    applyTheme(getTheme());
    const btn = document.getElementById('theme-toggle-btn');
    if (btn) btn.addEventListener('click', toggleTheme);
    // Wire up mobile drawer theme toggle
    const mobileBtn = document.getElementById('mobile-theme-toggle-btn');
    if (mobileBtn) mobileBtn.addEventListener('click', toggleTheme);
  });
})();
