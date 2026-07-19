/* ==========================================================================
   background.js — Phase 1: Minimal stub
   
   Phase 1: No star/particle generation needed.
   Hero uses raw-vangogh-without-text.png as background-image.
   
   Phase 2 will add: character layer positioning, desk layer.
   Phase 3 will add: canvas stars, particle system.
   ========================================================================== */

(function () {
  'use strict';

  function init() {
    // Phase 1: nothing to do — painting is the background
    // Page visibility is handled by motion.js
  }

  function pause()  { /* Phase 2: pause character/star animations */ }
  function resume() { /* Phase 2: resume */ }

  window.BackgroundSystem = { init: init, pause: pause, resume: resume };

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }

}());
