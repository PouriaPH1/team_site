/**
 * terminal.js — Interactive Terminal Team Explorer
 * Visual Upgrade V2 — Req 4
 *
 * Commands: help, ls, ls -la, cat [name], whoami, clear
 * Features: boot typewriter, command history (ArrowUp/Down), Enter to execute
 */

(function () {
  'use strict';

  /* ── Team Data ─────────────────────────────────────────── */
  const TEAM = [
    {
      id: 'leon',
      name: 'Leon',
      role: 'Full-Stack Developer & Architect',
      skills: ['C#', 'ASP.NET Core', 'React', 'SQL Server', 'Azure'],
      experience: '8 years',
      bio: 'Architect of scalable systems. Believes clean code is a form of art.'
    },
    {
      id: 'sara',
      name: 'Sara',
      role: 'Frontend Developer',
      skills: ['Vue.js', 'TypeScript', 'CSS', 'Figma', 'React'],
      experience: '5 years',
      bio: 'Crafts pixel-perfect interfaces with an eye for UX and accessibility.'
    },
    {
      id: 'mike',
      name: 'Mike',
      role: 'DevOps Engineer',
      skills: ['Docker', 'Kubernetes', 'CI/CD', 'Linux', 'Terraform'],
      experience: '6 years',
      bio: 'Keeps the infrastructure running smooth. Automation enthusiast.'
    },
    {
      id: 'nina',
      name: 'Nina',
      role: 'Mobile Developer',
      skills: ['Flutter', 'Kotlin', 'Swift', 'Firebase', 'REST APIs'],
      experience: '4 years',
      bio: 'Builds cross-platform apps that feel native. Coffee-fueled coder.'
    },
    {
      id: 'alex',
      name: 'Alex',
      role: 'Backend Developer',
      skills: ['Node.js', 'Python', 'PostgreSQL', 'Redis', 'GraphQL'],
      experience: '7 years',
      bio: 'Data architecture specialist. Optimizes queries before breakfast.'
    }
  ];

  /* ── Command Handlers ──────────────────────────────────── */
  const COMMANDS = {
    help: () =>
      'Available commands:\n' +
      '  ls         — List all team members\n' +
      '  ls -la     — List all team members (detailed)\n' +
      '  cat [name] — View member profile\n' +
      '  whoami     — Team philosophy\n' +
      '  clear      — Clear terminal',

    ls: () =>
      TEAM.map(m => '  ' + m.id.padEnd(10) + ' ' + m.role).join('\n'),

    'ls -la': () => COMMANDS.ls(),

    cat: (args) => {
      if (!args || !args[0]) {
        return 'Usage: cat [name]\nExample: cat leon';
      }
      const member = TEAM.find(m => m.id === args[0].toLowerCase());
      if (!member) {
        return 'cat: ' + args[0] + ': No such team member\nTry: ls';
      }
      return (
        'Name:       ' + member.name + '\n' +
        'Role:       ' + member.role + '\n' +
        'Exp:        ' + member.experience + '\n' +
        'Skills:     ' + member.skills.join(', ') + '\n\n' +
        member.bio
      );
    },

    whoami: () => {
      const quotes = [
        '"The best code is written with passion." — Starry Team',
        '"We don\'t just build software, we craft experiences." — Starry Team',
        '"Clean code always looks like it was written by someone who cares." — Robert C. Martin',
        '"Any fool can write code that a computer can understand. Good programmers write code that humans can understand." — Martin Fowler'
      ];
      return quotes[Math.floor(Math.random() * quotes.length)];
    },

    clear: null // handled specially in executeCommand
  };

  /* ── State ─────────────────────────────────────────────── */
  let history = [];
  let historyIndex = -1;
  let outputEl = null;
  let inputEl = null;

  /* ── DOM Helpers ───────────────────────────────────────── */
  function appendLine(text, cssClass) {
    if (!outputEl) return;

    // Handle multi-line output: split on \n
    const lines = String(text).split('\n');
    lines.forEach(function (line) {
      const div = document.createElement('div');
      div.className = 'terminal-output-line' + (cssClass ? ' ' + cssClass : '');
      div.textContent = line;
      outputEl.appendChild(div);
    });
    scrollToBottom();
  }

  function appendCommandLine(cmd) {
    appendLine('$ ' + cmd, 'line-command');
  }

  function appendBlankLine() {
    appendLine('');
  }

  function scrollToBottom() {
    if (outputEl) {
      outputEl.scrollTop = outputEl.scrollHeight;
    }
  }

  function clearOutput() {
    if (outputEl) {
      outputEl.innerHTML = '';
    }
  }

  /* ── Command Execution ─────────────────────────────────── */
  function executeCommand(raw) {
    const trimmed = raw.trim();
    if (!trimmed) return;

    // Store in history (avoid consecutive duplicates)
    if (history[0] !== trimmed) {
      history.unshift(trimmed);
      if (history.length > 50) history.pop();
    }
    historyIndex = -1;

    // Echo the command
    appendCommandLine(trimmed);

    // Parse command and args
    const parts = trimmed.split(/\s+/);
    const cmd = parts[0].toLowerCase();
    const args = parts.slice(1);

    // Special: ls -la
    const fullCmd = (cmd === 'ls' && args[0] === '-la') ? 'ls -la' : cmd;

    if (fullCmd === 'clear') {
      clearOutput();
      return;
    }

    if (COMMANDS.hasOwnProperty(fullCmd)) {
      const handler = COMMANDS[fullCmd];
      if (typeof handler === 'function') {
        const result = handler(args);
        if (result !== null && result !== undefined) {
          appendBlankLine();
          appendLine(result);
          appendBlankLine();
        }
      }
    } else {
      appendBlankLine();
      appendLine('command not found: ' + trimmed + '. Type \'help\' for available commands.', 'line-error');
      appendBlankLine();
    }
  }

  /* ── Typewriter Boot Sequence ──────────────────────────── */
  function typewriterBoot(lines, charDelay, lineDelay, onDone) {
    let lineIndex = 0;

    function typeLine(lineText, callback) {
      const div = document.createElement('div');
      div.className = 'terminal-output-line';
      outputEl.appendChild(div);

      let charIndex = 0;

      function typeChar() {
        if (charIndex < lineText.length) {
          div.textContent += lineText[charIndex];
          charIndex++;
          scrollToBottom();
          setTimeout(typeChar, charDelay);
        } else {
          callback();
        }
      }

      typeChar();
    }

    function nextLine() {
      if (lineIndex >= lines.length) {
        if (typeof onDone === 'function') onDone();
        return;
      }
      const line = lines[lineIndex++];
      if (line === '') {
        // blank line — append immediately
        const div = document.createElement('div');
        div.className = 'terminal-output-line';
        outputEl.appendChild(div);
        setTimeout(nextLine, lineDelay);
      } else {
        typeLine(line, function () {
          setTimeout(nextLine, lineDelay);
        });
      }
    }

    nextLine();
  }

  /* ── Keyboard Handler ──────────────────────────────────── */
  function onKeyDown(e) {
    if (e.key === 'Enter') {
      e.preventDefault();
      const value = inputEl.value;
      inputEl.value = '';
      executeCommand(value);
    } else if (e.key === 'ArrowUp') {
      e.preventDefault();
      if (history.length === 0) return;
      historyIndex = Math.min(historyIndex + 1, history.length - 1);
      inputEl.value = history[historyIndex];
      // Move cursor to end
      setTimeout(function () {
        inputEl.selectionStart = inputEl.selectionEnd = inputEl.value.length;
      }, 0);
    } else if (e.key === 'ArrowDown') {
      e.preventDefault();
      if (historyIndex <= 0) {
        historyIndex = -1;
        inputEl.value = '';
        return;
      }
      historyIndex--;
      inputEl.value = history[historyIndex];
    }
  }

  /* ── Click on terminal body focuses input ──────────────── */
  function onBodyClick() {
    if (inputEl) {
      inputEl.focus();
    }
  }

  /* ── Init ──────────────────────────────────────────────── */
  function init() {
    outputEl = document.getElementById('terminal-output');
    inputEl = document.getElementById('terminal-input');

    if (!outputEl || !inputEl) return; // terminal not on this page

    // Attach event listeners
    inputEl.addEventListener('keydown', onKeyDown);

    // Clicking anywhere in the terminal body focuses the input
    var bodyEl = outputEl.closest('.terminal-body') || outputEl;
    bodyEl.addEventListener('click', onBodyClick);

    // Boot sequence
    var welcomeLines = [
      'Welcome to Starry Code Team Explorer v1.0',
      'Type \'help\' to see available commands.',
      ''
    ];

    typewriterBoot(welcomeLines, 30, 80, function () {
      // After boot, focus input
      inputEl.focus();
    });
  }

  /* ── Wait for DOM ──────────────────────────────────────── */
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }

})();
