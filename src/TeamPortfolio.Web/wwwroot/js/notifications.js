/**
 * Toast Notification System — Team Portfolio
 * Usage:
 *   showToast('Saved successfully!')
 *   showToast('Something went wrong', 'error')
 *   showToast('Please review your input', 'warning')
 */
(function () {
  'use strict';

  function getOrCreateContainer() {
    var container = document.getElementById('toast-container');
    if (!container) {
      container = document.createElement('div');
      container.id = 'toast-container';
      container.className = 'toast-container';
      container.setAttribute('aria-live', 'polite');
      container.setAttribute('aria-atomic', 'false');
      document.body.appendChild(container);
    }
    return container;
  }

  function dismissToast(toast) {
    if (toast._timer) clearTimeout(toast._timer);
    toast.classList.add('toast-hide');
    toast.addEventListener('animationend', function () {
      if (toast.parentNode) toast.parentNode.removeChild(toast);
    }, { once: true });
  }

  /**
   * @param {string} message  - Text to display
   * @param {string} [type]   - 'success' | 'error' | 'warning'  (default: 'success')
   * @param {number} [duration] - Auto-dismiss delay in ms (default: 4000)
   */
  function showToast(message, type, duration) {
    type = type || 'success';
    duration = (duration !== undefined) ? duration : 4000;

    var iconMap = {
      success: 'fa-solid fa-circle-check',
      error: 'fa-solid fa-circle-xmark',
      warning: 'fa-solid fa-triangle-exclamation'
    };
    var colorMap = {
      success: 'var(--color-success)',
      error: 'var(--color-danger)',
      warning: 'var(--color-warning)'
    };

    var container = getOrCreateContainer();

    var toast = document.createElement('div');
    toast.className = 'toast ' + type;
    toast.setAttribute('role', 'status');

    toast.innerHTML =
      '<i class="' + (iconMap[type] || iconMap.success) + '" ' +
        'style="color:' + (colorMap[type] || colorMap.success) + '; margin-top:2px; flex-shrink:0;" ' +
        'aria-hidden="true"></i>' +
      '<span class="toast-message">' + message + '</span>' +
      '<button class="toast-close" aria-label="Close notification">' +
        '<i class="fa-solid fa-xmark" aria-hidden="true"></i>' +
      '</button>';

    container.appendChild(toast);

    toast.querySelector('.toast-close').addEventListener('click', function () {
      dismissToast(toast);
    });

    if (duration > 0) {
      toast._timer = setTimeout(function () { dismissToast(toast); }, duration);
    }
  }

  // Expose globally
  window.showToast = showToast;

})();
