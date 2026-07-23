/**
 * hero-interactions.js
 * Handles the interactive code block in the hero section:
 *  - Run button: typewriter-animates output lines, then shows "Run Again"
 *  - Copy button: copies raw code to clipboard, shows "Copied!" tooltip for 2s
 */

(function () {
  'use strict';

  // ── Typewriter helper ──────────────────────────────────────────────────────
  /**
   * Types `lines` into `container` character-by-character.
   * @param {HTMLElement} container  - The output panel element
   * @param {string[]}    lines      - Array of lines to type
   * @param {number}      charDelay  - Milliseconds between each character
   * @param {Function}    [onDone]   - Called when all lines finish typing
   */
  function typewriterLines(container, lines, charDelay, onDone) {
    // Clear previous content
    container.textContent = '';

    let lineIndex = 0;
    let charIndex = 0;

    function typeNextChar() {
      if (lineIndex >= lines.length) {
        // All lines done
        if (typeof onDone === 'function') onDone();
        return;
      }

      const currentLine = lines[lineIndex];

      if (charIndex < currentLine.length) {
        // Append next character to current text node
        container.textContent += currentLine[charIndex];
        charIndex++;
        setTimeout(typeNextChar, charDelay);
      } else {
        // Line complete — add newline, move to next
        container.textContent += '\n';
        lineIndex++;
        charIndex = 0;
        setTimeout(typeNextChar, charDelay);
      }
    }

    typeNextChar();
  }

  // ── Raw code string (must match the visible code snippet exactly) ──────────
  const RAW_CODE =
    'const create = () => {\n' +
    "  let ideas = 'limitless';\n" +
    '  let passion = true;\n' +
    '  while (passion) {\n' +
    '    code();\n' +
    '    design();\n' +
    '  }\n' +
    '}';

  // ── Output lines ──────────────────────────────────────────────────────────
  const OUTPUT_LINES = [
    '> Running starry_create.js...',
    '[IDEAS: LIMITLESS | STATUS: CREATING...]',
    '> Innovation loop started \u2713',
    '> Output: Building something beautiful.'
  ];

  // ── Init ──────────────────────────────────────────────────────────────────
  function init() {
    const codeBlock   = document.querySelector('.hero-code-block');
    if (!codeBlock) return; // not on a page with the hero

    const runBtn      = codeBlock.querySelector('.hero-code-run');
    const copyBtn     = codeBlock.querySelector('.hero-code-copy');
    const outputPanel = codeBlock.querySelector('.hero-code-output');

    if (!runBtn || !copyBtn || !outputPanel) return;

    // ── Run button ───────────────────────────────────────────────────────────
    let isRunning = false;

    runBtn.addEventListener('click', function () {
      if (isRunning) return;
      isRunning = true;

      // Disable during animation
      runBtn.disabled = true;
      runBtn.style.opacity = '0.55';

      // Show output panel (remove hidden attribute so CSS animation triggers)
      outputPanel.removeAttribute('hidden');
      outputPanel.textContent = '';

      // Calculate per-character delay so total ≈ 2 seconds
      const totalChars = OUTPUT_LINES.reduce((sum, l) => sum + l.length, 0);
      const charDelay  = Math.round(2000 / Math.max(totalChars, 1));

      typewriterLines(outputPanel, OUTPUT_LINES, charDelay, function () {
        // Animation complete — switch to "Run Again"
        runBtn.innerHTML =
          '<svg width="10" height="10" viewBox="0 0 24 24" fill="currentColor" aria-hidden="true">' +
            '<path d="M5 3l14 9-14 9V3z"/>' +
          '</svg>' +
          ' Run Again';
        runBtn.disabled      = false;
        runBtn.style.opacity = '';
        isRunning            = false;
      });
    });

    // ── Copy button ──────────────────────────────────────────────────────────
    let copyTimeout = null;

    copyBtn.addEventListener('click', function () {
      if (!navigator.clipboard) {
        // Fallback for non-secure contexts
        try {
          const ta = document.createElement('textarea');
          ta.value = RAW_CODE;
          ta.style.cssText = 'position:fixed;opacity:0;pointer-events:none';
          document.body.appendChild(ta);
          ta.select();
          document.execCommand('copy');
          document.body.removeChild(ta);
          showCopied();
        } catch (_) { /* silently ignore */ }
        return;
      }

      navigator.clipboard.writeText(RAW_CODE).then(showCopied).catch(function () {
        // Permission denied or unavailable — ignore silently
      });
    });

    function showCopied() {
      copyBtn.classList.add('copied');
      clearTimeout(copyTimeout);
      copyTimeout = setTimeout(function () {
        copyBtn.classList.remove('copied');
      }, 2000);
    }
  }

  // Run after DOM is ready
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }
})();
