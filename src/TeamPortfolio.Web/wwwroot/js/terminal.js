/**
 * terminal.js — IDE Split-Panel Terminal Team Explorer
 * Redesign V3: two-panel, autocomplete, starfield, matrix, easter eggs
 */
(function () {
  'use strict';

  /* ═══════════════════════════════════════════════════════════
     1. TEAM DATA
  ═══════════════════════════════════════════════════════════ */
  const TEAM = [
    {
      id: 'leon', name: 'Leon', role: 'Full-Stack Developer & Architect',
      dept: 'backend',
      skills: [
        { name: 'C# / .NET', pct: 95 }, { name: 'React', pct: 88 },
        { name: 'Azure',     pct: 80 }, { name: 'SQL Server', pct: 85 }
      ],
      experience: '8 years', github: 'https://github.com/',
      linkedin: 'https://linkedin.com/',
      bio: 'Architect of scalable systems. Believes clean code is a form of art.'
    },
    {
      id: 'sara', name: 'Sara', role: 'Frontend Developer',
      dept: 'frontend',
      skills: [
        { name: 'Vue.js',     pct: 92 }, { name: 'TypeScript', pct: 88 },
        { name: 'CSS/SCSS',   pct: 90 }, { name: 'Figma',      pct: 80 }
      ],
      experience: '5 years', github: 'https://github.com/',
      linkedin: 'https://linkedin.com/',
      bio: 'Crafts pixel-perfect interfaces with an eye for UX and accessibility.'
    },
    {
      id: 'mike', name: 'Mike', role: 'DevOps Engineer',
      dept: 'backend',
      skills: [
        { name: 'Docker',     pct: 93 }, { name: 'Kubernetes', pct: 85 },
        { name: 'Terraform',  pct: 80 }, { name: 'Linux',      pct: 92 }
      ],
      experience: '6 years', github: 'https://github.com/',
      linkedin: 'https://linkedin.com/',
      bio: 'Keeps the infrastructure humming. Automation is his love language.'
    },
    {
      id: 'nina', name: 'Nina', role: 'Mobile Developer',
      dept: 'mobile',
      skills: [
        { name: 'Flutter',    pct: 90 }, { name: 'Kotlin',    pct: 82 },
        { name: 'Swift',      pct: 78 }, { name: 'Firebase',  pct: 85 }
      ],
      experience: '4 years', github: 'https://github.com/',
      linkedin: 'https://linkedin.com/',
      bio: 'Builds cross-platform apps that feel native. Coffee-fuelled coder.'
    },
    {
      id: 'alex', name: 'Alex', role: 'Backend Developer',
      dept: 'backend',
      skills: [
        { name: 'Node.js',    pct: 91 }, { name: 'Python',    pct: 87 },
        { name: 'PostgreSQL', pct: 88 }, { name: 'GraphQL',   pct: 80 }
      ],
      experience: '7 years', github: 'https://github.com/',
      linkedin: 'https://linkedin.com/',
      bio: 'Data architecture specialist. Optimises queries before breakfast.'
    }
  ];


  /* ═══════════════════════════════════════════════════════════
     2. CONSTANTS & STATE
  ═══════════════════════════════════════════════════════════ */
  const DEPT_ICONS = { backend: '[BE]', frontend: '[FE]', mobile: '[MOB]', uiux: '[UX]' };

  const FORTUNE_QUOTES = [
    '"First make it work. Then make it beautiful." — Unknown',
    '"Clean code always looks like it was written by someone who cares." — R.C. Martin',
    '"Any fool can write code a computer can understand.\nGood programmers write code humans can understand." — Fowler',
    '"The best error message is the one that never shows up." — Sheryl Sandberg',
    '"Simplicity is the soul of efficiency." — Austin Freeman',
    '"Programs must be written for people to read,\nand only incidentally for machines to execute." — Abelson'
  ];

  const MOTD_LINES = [
    '  Welcome to Starry Code Team Explorer v3.0',
    '  ─────────────────────────────────────────',
    '  Status:  ✔ All systems operational',
    '  Members: 5  |  Projects: 12  |  Commits: 4,271',
    ''
  ];

  let cmdHistory    = [];
  let historyIndex  = -1;
  let viewedMembers = [];
  let acSelected    = -1;
  let acItems       = [];
  let matrixActive  = false;
  let matrixTimer   = null;

  let outputEl, inputEl, suggestionsEl, autocompleteEl,
      previewIdleEl, previewProfileEl, previewLoadingEl,
      previewTitleEl, previewNameEl, previewRoleEl,
      previewExpEl, previewBioEl, previewSkillsEl,
      previewLinksEl, previewAvatarEl,
      statusMemberEl, statusCmdEl, statusTimeEl,
      clearBtnEl, splitEl;


  /* ═══════════════════════════════════════════════════════════
     3. DOM HELPERS
  ═══════════════════════════════════════════════════════════ */
  function appendLine(text, cssClass) {
    if (!outputEl) return;
    String(text).split('\n').forEach(function (line) {
      const div = document.createElement('div');
      div.className = 'terminal-output-line' + (cssClass ? ' ' + cssClass : '');
      div.textContent = line;
      outputEl.appendChild(div);
    });
    scrollToBottom();
  }

  function appendCommandLine(cmd) {
    const div = document.createElement('div');
    div.className = 'terminal-output-line line-command';
    // replicate coloured prompt inline
    div.innerHTML =
      '<span style="color:#4ADE80">starry</span>' +
      '<span style="color:rgba(212,201,168,.4)">@</span>' +
      '<span style="color:#60A5FA">team</span>' +
      '<span style="color:rgba(212,201,168,.4)">:</span>' +
      '<span style="color:#C084FC">~</span>' +
      '<span style="color:rgba(212,201,168,.6)">$ </span>' +
      '<span style="color:#F5C842">' + escapeHtml(cmd) + '</span>';
    outputEl.appendChild(div);
    scrollToBottom();
  }

  function appendBlank() { appendLine(''); }

  function scrollToBottom() {
    if (outputEl) outputEl.scrollTop = outputEl.scrollHeight;
  }

  function clearOutput() {
    if (outputEl) outputEl.innerHTML = '';
  }

  function escapeHtml(str) {
    return String(str)
      .replace(/&/g, '&amp;').replace(/</g, '&lt;')
      .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
  }

  /* Typewriter: types an array of {text, cls} objects one char at a time */
  function typeLines(lines, charDelay, lineDelay, onDone) {
    let li = 0;
    function nextLine() {
      if (li >= lines.length) { if (onDone) onDone(); return; }
      const { text, cls } = lines[li++];
      if (text === '') {
        appendBlank();
        setTimeout(nextLine, lineDelay);
        return;
      }
      const div = document.createElement('div');
      div.className = 'terminal-output-line typing' + (cls ? ' ' + cls : '');
      outputEl.appendChild(div);
      let ci = 0;
      function typeChar() {
        if (ci < text.length) {
          div.textContent += text[ci++];
          scrollToBottom();
          setTimeout(typeChar, charDelay);
        } else {
          setTimeout(nextLine, lineDelay);
        }
      }
      typeChar();
    }
    nextLine();
  }


  /* ═══════════════════════════════════════════════════════════
     4. STATUS BAR
  ═══════════════════════════════════════════════════════════ */
  function updateStatusTime() {
    if (!statusTimeEl) return;
    const now = new Date();
    statusTimeEl.textContent =
      now.getHours().toString().padStart(2, '0') + ':' +
      now.getMinutes().toString().padStart(2, '0');
  }

  function setStatusMember(name) {
    if (statusMemberEl) statusMemberEl.textContent = name || 'No profile loaded';
  }

  function setStatusCmd(cmd) {
    if (statusCmdEl) statusCmdEl.textContent = cmd || 'Ready';
  }

  /* ═══════════════════════════════════════════════════════════
     5. PREVIEW PANEL
  ═══════════════════════════════════════════════════════════ */
  function showIdle() {
    previewIdleEl.hidden    = false;
    previewLoadingEl.hidden = true;
    previewProfileEl.hidden = true;
    if (previewTitleEl) previewTitleEl.textContent = 'profile.json';
    setStatusMember(null);
  }

  function showLoading(label) {
    previewIdleEl.hidden    = true;
    previewLoadingEl.hidden = false;
    previewProfileEl.hidden = true;
    const fillEl = document.getElementById('preview-loading-fill');
    const textEl = document.getElementById('preview-loading-text');
    if (textEl) textEl.textContent = label || 'Loading profile...';
    if (fillEl) { fillEl.style.width = '0%'; }
    return fillEl;
  }

  function animateBar(fillEl, onDone) {
    let pct = 0;
    const steps = [
      { target: 30,  delay: 60  },
      { target: 55,  delay: 40  },
      { target: 80,  delay: 35  },
      { target: 100, delay: 25  }
    ];
    let si = 0;
    function step() {
      if (si >= steps.length) { if (onDone) onDone(); return; }
      const { target, delay } = steps[si++];
      const interval = setInterval(function () {
        pct++;
        if (fillEl) fillEl.style.width = pct + '%';
        if (pct >= target) {
          clearInterval(interval);
          setTimeout(step, delay);
        }
      }, 16);
    }
    step();
  }

  function showProfile(member) {
    previewIdleEl.hidden    = true;
    previewLoadingEl.hidden = true;
    previewProfileEl.hidden = false;
    if (previewTitleEl) previewTitleEl.textContent = member.id + '.json';

    // avatar initials
    previewAvatarEl.textContent = member.name.slice(0, 2).toUpperCase();
    previewAvatarEl.style.background = avatarGradient(member.id);

    previewNameEl.textContent = member.name;
    previewRoleEl.textContent = member.role;
    previewExpEl.textContent  = member.experience + ' experience';
    previewBioEl.textContent  = member.bio;

    // Skills bars
    previewSkillsEl.innerHTML = '';
    member.skills.forEach(function (sk) {
      const row = document.createElement('div');
      row.className = 'skill-row';
      row.innerHTML =
        '<span class="skill-name">' + escapeHtml(sk.name) + '</span>' +
        '<div class="skill-track"><div class="skill-fill" style="width:' + sk.pct + '%"></div></div>' +
        '<span class="skill-pct">' + sk.pct + '%</span>';
      previewSkillsEl.appendChild(row);
    });

    // Animate bars after paint
    requestAnimationFrame(function () {
      requestAnimationFrame(function () {
        previewSkillsEl.querySelectorAll('.skill-fill').forEach(function (f) {
          f.classList.add('animate');
        });
      });
    });

    // Social links
    previewLinksEl.innerHTML = '';
    var links = [
      { label: 'GitHub',   href: member.github,   icon: githubSvg()   },
      { label: 'LinkedIn', href: member.linkedin,  icon: linkedinSvg() }
    ];
    links.forEach(function (l) {
      if (!l.href) return;
      const a = document.createElement('a');
      a.href      = l.href;
      a.target    = '_blank';
      a.rel       = 'noopener noreferrer';
      a.className = 'preview-link-btn';
      a.innerHTML = l.icon + ' ' + escapeHtml(l.label);
      previewLinksEl.appendChild(a);
    });

    setStatusMember(member.name + ' — ' + member.role);
  }

  function avatarGradient(id) {
    const map = {
      leon: 'linear-gradient(135deg,#1E3A5F,#2D5A8E)',
      sara: 'linear-gradient(135deg,#3D1F5F,#6B2FA0)',
      mike: 'linear-gradient(135deg,#1A4A2E,#2E7D4F)',
      nina: 'linear-gradient(135deg,#4A1F2E,#7D2F4F)',
      alex: 'linear-gradient(135deg,#2A1A4A,#4F2FA0)'
    };
    return map[id] || 'linear-gradient(135deg,#1A2235,#2D3A55)';
  }

  function githubSvg() {
    return '<svg width="11" height="11" viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M12 0C5.37 0 0 5.37 0 12c0 5.3 3.44 9.8 8.2 11.38.6.1.82-.26.82-.58v-2.02c-3.34.72-4.04-1.6-4.04-1.6-.54-1.38-1.33-1.74-1.33-1.74-1.08-.74.08-.72.08-.72 1.2.08 1.83 1.23 1.83 1.23 1.06 1.82 2.8 1.3 3.48.99.1-.77.41-1.3.75-1.6-2.67-.3-5.47-1.33-5.47-5.93 0-1.31.47-2.38 1.24-3.22-.12-.3-.54-1.52.12-3.18 0 0 1.01-.32 3.3 1.23a11.5 11.5 0 0 1 3-.4c1.02 0 2.04.13 3 .4 2.28-1.55 3.29-1.23 3.29-1.23.66 1.66.24 2.88.12 3.18.77.84 1.24 1.91 1.24 3.22 0 4.61-2.81 5.63-5.48 5.92.43.37.81 1.1.81 2.22v3.29c0 .32.22.7.83.58C20.56 21.8 24 17.3 24 12c0-6.63-5.37-12-12-12z"/></svg>';
  }

  function linkedinSvg() {
    return '<svg width="11" height="11" viewBox="0 0 24 24" fill="currentColor" aria-hidden="true"><path d="M20.45 20.45h-3.56v-5.57c0-1.33-.03-3.04-1.85-3.04-1.85 0-2.13 1.45-2.13 2.94v5.67H9.35V9h3.41v1.56h.05c.48-.9 1.64-1.85 3.37-1.85 3.6 0 4.27 2.37 4.27 5.45v6.29zM5.34 7.43a2.07 2.07 0 1 1 0-4.14 2.07 2.07 0 0 1 0 4.14zM7.12 20.45H3.56V9h3.56v11.45zM22.22 0H1.77C.79 0 0 .77 0 1.72v20.56C0 23.23.79 24 1.77 24h20.45c.98 0 1.78-.77 1.78-1.72V1.72C24 .77 23.2 0 22.22 0z"/></svg>';
  }


  /* ═══════════════════════════════════════════════════════════
     6. COMMANDS
  ═══════════════════════════════════════════════════════════ */
  const COMMANDS = {

    help: function () {
      appendLine('');
      appendLine('  Available commands:', 'line-info');
      appendLine('');
      var cmds = [
        ['ls',          'List team members'],
        ['ls -la',      'Detailed list with roles'],
        ['cat [name]',  'Load member profile'],
        ['tree',        'Show team structure'],
        ['top',         'Team activity dashboard'],
        ['neofetch',    'System info (hacker style)'],
        ['pwd',         'Current directory'],
        ['history',     'Recently viewed members'],
        ['fortune',     'Random quote'],
        ['git log',     'Team timeline'],
        ['status',      'Sprint progress'],
        ['clear',       'Clear terminal'],
        ['help',        'Show this help']
      ];
      cmds.forEach(function (c) {
        appendLine('  ' + c[0].padEnd(16) + '— ' + c[1]);
      });
      appendLine('');
      appendLine('  Easter eggs: coffee  |  make awesome  |  sudo hire-us  |  sudo rm bugs  |  matrix', 'line-muted');
      appendLine('');
    },

    ls: function (args) {
      var detailed = args && args[0] === '-la';
      appendLine('');
      // group by dept
      var depts = {};
      TEAM.forEach(function (m) {
        if (!depts[m.dept]) depts[m.dept] = [];
        depts[m.dept].push(m);
      });
      Object.keys(depts).sort().forEach(function (dept) {
        appendLine('  ' + (DEPT_ICONS[dept] || dept.toUpperCase()), 'line-accent');
        depts[dept].forEach(function (m) {
          if (detailed) {
            appendLine('  drwxr-xr-x  ' + m.experience.padEnd(10) + m.id.padEnd(10) + m.role);
          } else {
            appendLine('  ' + m.id);
          }
        });
        appendLine('');
      });
    },

    tree: function () {
      appendLine('');
      appendLine('  /team/starry', 'line-accent');
      var depts = {};
      TEAM.forEach(function (m) {
        if (!depts[m.dept]) depts[m.dept] = [];
        depts[m.dept].push(m);
      });
      var deptKeys = Object.keys(depts).sort();
      deptKeys.forEach(function (dept, di) {
        var isLastDept = di === deptKeys.length - 1;
        appendLine('  ' + (isLastDept ? '└── ' : '├── ') + dept + '/', 'line-info');
        depts[dept].forEach(function (m, mi) {
          var isLastMember = mi === depts[dept].length - 1;
          appendLine('  ' + (isLastDept ? '    ' : '│   ') + (isLastMember ? '└── ' : '├── ') + m.id);
        });
      });
      appendLine('');
    },

    top: function () {
      appendLine('');
      appendLine('  Team Activity Monitor', 'line-info');
      appendLine('  ' + '─'.repeat(36), 'line-muted');
      var activities = ['Coding', 'Reviewing PR', 'Design', 'DevOps', 'Testing'];
      TEAM.forEach(function (m) {
        var act  = activities[Math.floor(Math.random() * activities.length)];
        var bars = Math.floor(Math.random() * 8) + 4;
        var bar  = '█'.repeat(bars) + '░'.repeat(10 - bars);
        appendLine('  ' + m.name.padEnd(8) + ' ' + act.padEnd(13) + ' ' + bar + ' ' + (bars * 10) + '%');
      });
      appendLine('');
    },

    neofetch: function () {
      appendLine('');
      var lines = [
        { text: '        ★          ', cls: 'line-warn' },
        { text: '    .       .      ', cls: 'line-muted' },
        { text: '  .   STARRY  .    ', cls: 'line-warn' },
        { text: '    .       .      ', cls: 'line-muted' },
        { text: '        ★          ', cls: 'line-warn' }
      ];
      var info = [
        { key: 'OS',      val: 'TeamPortfolio v3.0' },
        { key: 'Kernel',  val: 'Creativity 6.0'     },
        { key: 'Host',    val: 'Dream Team'          },
        { key: 'CPU',     val: '∞ Coffee'            },
        { key: 'RAM',     val: 'Unlimited Ideas'     },
        { key: 'Members', val: TEAM.length + ' engineers' },
        { key: 'Projects',val: '12 shipped'          },
        { key: 'GitHub',  val: 'github.com/starry'   }
      ];
      // Print side by side using two columns
      var max = Math.max(lines.length, info.length);
      for (var i = 0; i < max; i++) {
        var left  = lines[i]  ? lines[i].text  : '                   ';
        var right = info[i]   ? '\x1b[0m' + info[i].key.padEnd(10) + ': ' + info[i].val : '';
        appendLine('  ' + left + '   ' + (info[i] ? info[i].key.padEnd(10) + ': ' + info[i].val : ''),
          lines[i] ? lines[i].cls : '');
      }
      appendLine('');
    },

    pwd: function () {
      appendLine('');
      appendLine('  /team/starry/core', 'line-success');
      appendLine('');
    },

    history: function () {
      appendLine('');
      if (viewedMembers.length === 0) {
        appendLine('  No members viewed yet. Try: cat leon', 'line-muted');
      } else {
        appendLine('  Recently viewed:', 'line-info');
        viewedMembers.slice().reverse().forEach(function (id, i) {
          appendLine('  ' + (i + 1) + '  ' + id);
        });
      }
      appendLine('');
    },

    fortune: function () {
      var q = FORTUNE_QUOTES[Math.floor(Math.random() * FORTUNE_QUOTES.length)];
      appendLine('');
      appendLine('  ┌─ Fortune ─────────────────────────────┐', 'line-muted');
      q.split('\n').forEach(function (line) {
        appendLine('  │  ' + line, 'line-info');
      });
      appendLine('  └───────────────────────────────────────┘', 'line-muted');
      appendLine('');
    },

    'git log': function () {
      appendLine('');
      appendLine('  commit history — team timeline', 'line-accent');
      appendLine('');
      var log = [
        { year: '2025', msg: 'Launched TeamPortfolio v3.0', hash: 'a1b2c3d' },
        { year: '2024', msg: 'Added AI-powered search',     hash: 'f4e5d6c' },
        { year: '2024', msg: 'Won Regional Dev Hackathon',  hash: 'b7a8c9d' },
        { year: '2023', msg: 'Shipped mobile app v1',       hash: 'e0f1a2b' },
        { year: '2023', msg: 'Team founded — 5 members',    hash: 'c3d4e5f' }
      ];
      log.forEach(function (entry) {
        appendLine('  commit ' + entry.hash, 'line-warn');
        appendLine('  Date:   ' + entry.year);
        appendLine('');
        appendLine('      ' + entry.msg);
        appendLine('');
      });
    },

    status: function () {
      appendLine('');
      appendLine('  Sprint Status', 'line-info');
      appendLine('  ' + '─'.repeat(32), 'line-muted');
      var sprint = 80;
      var bar = '█'.repeat(Math.round(sprint / 10)) + '░'.repeat(10 - Math.round(sprint / 10));
      appendLine('  Current Sprint    ' + bar + '  ' + sprint + '%');
      appendLine('  Next Release      in 2 days', 'line-success');
      appendLine('  Open PRs          3');
      appendLine('  Issues closed     14 this week', 'line-success');
      appendLine('');
    },

    cat: function (args, raw) {
      if (!args || !args[0]) {
        appendLine('');
        appendLine('  Usage: cat [name]    e.g. cat leon', 'line-warn');
        appendLine('');
        return;
      }
      var member = TEAM.find(function (m) { return m.id === args[0].toLowerCase(); });
      if (!member) {
        appendLine('');
        appendLine('  cat: ' + args[0] + ': No such team member', 'line-error');
        // suggest closest
        var close = TEAM.find(function (m) {
          return m.id.startsWith(args[0].slice(0, 2).toLowerCase());
        });
        if (close) {
          appendLine('  Did you mean: cat ' + close.id + ' ?', 'line-warn');
        } else {
          appendLine('  Run ls to see all members.', 'line-muted');
        }
        appendLine('');
        return;
      }

      // Track history
      if (viewedMembers[viewedMembers.length - 1] !== member.id) {
        viewedMembers.push(member.id);
        if (viewedMembers.length > 10) viewedMembers.shift();
      }

      // Show loading in terminal
      appendLine('');
      appendLine('  Opening profile...', 'line-muted');

      var fillEl = showLoading('Decrypting ' + member.name + '...');
      var termLines = [
        { text: '  [████░░░░░░]  Loading...', cls: 'line-muted' },
        { text: '  [██████░░░░]  Found member...', cls: 'line-muted' },
        { text: '  [████████░░]  Decrypting...', cls: 'line-muted' },
        { text: '  [██████████]  Done.', cls: 'line-success' },
        { text: '' },
        { text: '  ✔ Profile loaded — see preview panel →', cls: 'line-success' },
        { text: '' }
      ];

      setStatusCmd('cat ' + member.id);

      animateBar(fillEl, function () {
        typeLines(termLines, 15, 60, function () {
          showProfile(member);
          setStatusMember(member.name + ' — ' + member.role);
          hideSuggestions();
        });
      });
    },

    /* ── Easter Eggs ── */
    coffee: function () {
      appendLine('');
      appendLine('  Brewing motivation...', 'line-muted');
      setTimeout(function () {
        appendLine('');
        appendLine('  ( (     ', 'line-warn');
        appendLine('   ) )    ', 'line-warn');
        appendLine('  ______  ', 'line-warn');
        appendLine(' |      | ', 'line-warn');
        appendLine(' | BREW | ', 'line-warn');
        appendLine(' |______| ', 'line-warn');
        appendLine('');
        appendLine('  Done. Now get back to work.', 'line-success');
        appendLine('');
      }, 600);
    },

    'make awesome': function () {
      appendLine('');
      appendLine('  Building awesome...', 'line-muted');
      var steps = [
        '  [██░░░░░░░░]  Collecting ideas...',
        '  [████░░░░░░]  Writing clean code...',
        '  [██████░░░░]  Fixing last-minute bugs...',
        '  [████████░░]  Polishing UI...',
        '  [██████████]  Deployed to production!'
      ];
      var i = 0;
      var iv = setInterval(function () {
        if (i >= steps.length) {
          clearInterval(iv);
          appendLine('');
          appendLine('  ✔ Success! Awesome delivered.', 'line-success');
          appendLine('');
          return;
        }
        appendLine(steps[i++], 'line-info');
        scrollToBottom();
      }, 320);
    },

    'sudo hire-us': function () {
      appendLine('');
      appendLine('  [sudo] password for visitor: ********', 'line-muted');
      setTimeout(function () {
        appendLine('');
        appendLine('  Permission granted.', 'line-success');
        appendLine('  Redirecting to contact page...', 'line-info');
        appendLine('');
        setTimeout(function () {
          window.location.href = '/Contact';
        }, 1400);
      }, 800);
    },

    'sudo rm bugs': function () {
      appendLine('');
      appendLine('  rm: cannot remove \'bugs\': Permission denied', 'line-error');
      appendLine('  Bugs are immortal. They only transform.', 'line-muted');
      appendLine('');
    },

    matrix: function () {
      appendLine('');
      appendLine('  Initiating Matrix...', 'line-success');
      appendLine('');
      setTimeout(function () { startMatrix(); }, 400);
    },

    motd: function () {
      appendLine('');
      MOTD_LINES.forEach(function (l) { appendLine(l, 'line-info'); });
    },

    whoami: function () {
      appendLine('');
      appendLine('  visitor — guest access', 'line-success');
      appendLine('  You are exploring the Starry Team portfolio.', 'line-muted');
      appendLine('  Run \'sudo hire-us\' to upgrade permissions.', 'line-warn');
      appendLine('');
    },

    clear: null  /* handled in executeCommand */
  };


  /* ═══════════════════════════════════════════════════════════
     7. COMMAND EXECUTION
  ═══════════════════════════════════════════════════════════ */
  function executeCommand(raw) {
    var trimmed = raw.trim();
    if (!trimmed) return;

    // History
    if (cmdHistory[0] !== trimmed) {
      cmdHistory.unshift(trimmed);
      if (cmdHistory.length > 50) cmdHistory.pop();
    }
    historyIndex = -1;

    appendCommandLine(trimmed);
    setStatusCmd(trimmed);
    closeAutocomplete();

    var parts   = trimmed.split(/\s+/);
    var cmdKey  = parts[0].toLowerCase();
    var args    = parts.slice(1);

    // Multi-word keys: 'ls -la', 'git log', 'make awesome', 'sudo hire-us', 'sudo rm bugs'
    var fullKey = trimmed.toLowerCase();
    // Check full string first, then first word
    var handler = null;
    if (COMMANDS.hasOwnProperty(fullKey)) {
      handler = COMMANDS[fullKey];
      if (fullKey === 'clear') { clearOutput(); showIdle(); return; }
      if (typeof handler === 'function') { handler(args, trimmed); return; }
    }
    if (COMMANDS.hasOwnProperty(cmdKey)) {
      if (cmdKey === 'clear') { clearOutput(); showIdle(); return; }
      handler = COMMANDS[cmdKey];
      if (typeof handler === 'function') { handler(args, trimmed); return; }
    }

    // Unknown command — smart error
    appendLine('');
    appendLine('  Hmm... No command named \'' + trimmed + '\'', 'line-error');
    var close = Object.keys(COMMANDS).find(function (k) {
      return k !== 'clear' && k !== 'cat' && k.startsWith(cmdKey.slice(0, 2));
    });
    if (close) {
      appendLine('  Did you mean: ' + close + ' ?', 'line-warn');
    } else {
      appendLine('  Type \'help\' for available commands.', 'line-muted');
    }
    appendLine('');
  }

  /* ═══════════════════════════════════════════════════════════
     8. AUTOCOMPLETE
  ═══════════════════════════════════════════════════════════ */
  var ALL_COMPLETIONS = [
    { cmd: 'ls',           desc: 'List team members'         },
    { cmd: 'ls -la',       desc: 'Detailed list'             },
    { cmd: 'cat ',         desc: 'Load member profile'       },
    { cmd: 'tree',         desc: 'Show team tree'            },
    { cmd: 'top',          desc: 'Team activity'             },
    { cmd: 'neofetch',     desc: 'System info'               },
    { cmd: 'pwd',          desc: 'Current path'              },
    { cmd: 'history',      desc: 'Recently viewed'           },
    { cmd: 'fortune',      desc: 'Random quote'              },
    { cmd: 'git log',      desc: 'Team timeline'             },
    { cmd: 'status',       desc: 'Sprint progress'           },
    { cmd: 'whoami',       desc: 'Who am I?'                 },
    { cmd: 'motd',         desc: 'Message of the day'        },
    { cmd: 'coffee',       desc: '☕'                         },
    { cmd: 'make awesome', desc: 'Easter egg'                },
    { cmd: 'sudo hire-us', desc: 'Easter egg'                },
    { cmd: 'matrix',       desc: 'Easter egg'                },
    { cmd: 'clear',        desc: 'Clear terminal'            },
    { cmd: 'help',         desc: 'Show help'                 }
  ];
  // Add 'cat [name]' entries
  TEAM.forEach(function (m) {
    ALL_COMPLETIONS.push({ cmd: 'cat ' + m.id, desc: m.role });
  });

  function buildAutocomplete(val) {
    closeAutocomplete();
    if (!val) return;
    var lower = val.toLowerCase();
    var matches = ALL_COMPLETIONS.filter(function (c) {
      return c.cmd.toLowerCase().startsWith(lower) && c.cmd.toLowerCase() !== lower;
    }).slice(0, 6);

    if (!matches.length) return;
    acItems    = matches;
    acSelected = -1;

    matches.forEach(function (m, i) {
      var div = document.createElement('div');
      div.className = 'autocomplete-item';
      div.setAttribute('role', 'option');
      div.innerHTML =
        '<span class="autocomplete-item-cmd">' + escapeHtml(m.cmd) + '</span>' +
        '<span class="autocomplete-item-desc">' + escapeHtml(m.desc) + '</span>';
      div.addEventListener('mousedown', function (e) {
        e.preventDefault();
        inputEl.value = m.cmd;
        closeAutocomplete();
        inputEl.focus();
      });
      autocompleteEl.appendChild(div);
    });
  }

  function closeAutocomplete() {
    autocompleteEl.innerHTML = '';
    acItems    = [];
    acSelected = -1;
  }

  function selectAcItem(dir) {
    var items = autocompleteEl.querySelectorAll('.autocomplete-item');
    if (!items.length) return false;
    if (acSelected >= 0) items[acSelected].classList.remove('selected');
    acSelected += dir;
    if (acSelected < 0)           acSelected = items.length - 1;
    if (acSelected >= items.length) acSelected = 0;
    items[acSelected].classList.add('selected');
    inputEl.value = acItems[acSelected].cmd;
    return true;
  }


  /* ═══════════════════════════════════════════════════════════
     9. SMART SUGGESTIONS BAR
  ═══════════════════════════════════════════════════════════ */
  var activeHintIdx = 0;
  var hintTimer     = null;

  function startHintCycle() {
    if (hintTimer) return;
    var chips = suggestionsEl ? suggestionsEl.querySelectorAll('.suggestion-chip') : [];
    if (!chips.length) return;
    chips[activeHintIdx % chips.length].classList.add('active-hint');
    hintTimer = setInterval(function () {
      chips.forEach(function (c) { c.classList.remove('active-hint'); });
      activeHintIdx = (activeHintIdx + 1) % chips.length;
      chips[activeHintIdx].classList.add('active-hint');
    }, 2200);
  }

  function stopHintCycle() {
    clearInterval(hintTimer);
    hintTimer = null;
    if (suggestionsEl) {
      suggestionsEl.querySelectorAll('.suggestion-chip').forEach(function (c) {
        c.classList.remove('active-hint');
      });
    }
  }

  function showSuggestions() {
    if (suggestionsEl) suggestionsEl.classList.remove('hidden');
    startHintCycle();
  }

  function hideSuggestions() {
    if (suggestionsEl) suggestionsEl.classList.add('hidden');
    stopHintCycle();
  }

  /* ═══════════════════════════════════════════════════════════
     10. MATRIX EASTER EGG
  ═══════════════════════════════════════════════════════════ */
  function startMatrix() {
    if (matrixActive) return;
    matrixActive = true;

    var overlay = document.createElement('div');
    overlay.className = 'matrix-overlay';
    var canvas  = document.createElement('canvas');
    canvas.className = 'matrix-canvas';
    overlay.appendChild(canvas);
    splitEl.appendChild(overlay);

    var W = splitEl.offsetWidth;
    var H = splitEl.offsetHeight;
    canvas.width  = W;
    canvas.height = H;
    var ctx   = canvas.getContext('2d');
    var cols  = Math.floor(W / 14);
    var drops = Array(cols).fill(1);
    var chars = 'アイウエオカキクケコサシスセソタチツテトナニヌネノABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#$%^&*';

    function drawMatrix() {
      ctx.fillStyle = 'rgba(0,0,0,0.05)';
      ctx.fillRect(0, 0, W, H);
      ctx.fillStyle = '#00FF41';
      ctx.font = '13px JetBrains Mono, monospace';
      drops.forEach(function (y, i) {
        var c = chars[Math.floor(Math.random() * chars.length)];
        ctx.fillText(c, i * 14, y * 14);
        if (y * 14 > H && Math.random() > 0.975) drops[i] = 0;
        drops[i]++;
      });
    }

    var raf;
    function loop() { drawMatrix(); raf = requestAnimationFrame(loop); }
    loop();

    matrixTimer = setTimeout(function () {
      cancelAnimationFrame(raf);
      overlay.remove();
      matrixActive = false;
      appendLine('  Matrix mode terminated.', 'line-muted');
      appendLine('');
    }, 5000);
  }

  /* ═══════════════════════════════════════════════════════════
     11. STARFIELD BACKGROUND
  ═══════════════════════════════════════════════════════════ */
  function initStarfield() {
    var canvas = document.getElementById('terminal-starfield');
    if (!canvas) return;
    var section = canvas.parentElement;
    var ctx;

    function resize() {
      canvas.width  = section.offsetWidth;
      canvas.height = section.offsetHeight;
    }
    resize();
    window.addEventListener('resize', resize);

    ctx = canvas.getContext('2d');

    // Stars
    var stars = Array.from({ length: 140 }, function () {
      return {
        x:     Math.random() * canvas.width,
        y:     Math.random() * canvas.height,
        r:     Math.random() * 1.2 + 0.2,
        alpha: Math.random() * 0.6 + 0.1,
        speed: Math.random() * 0.12 + 0.03,
        twinkle: Math.random() * Math.PI * 2
      };
    });

    // Meteors
    var meteors = [];
    function spawnMeteor() {
      meteors.push({
        x:     Math.random() * canvas.width * 1.2,
        y:     Math.random() * canvas.height * 0.4,
        len:   Math.random() * 80 + 40,
        speed: Math.random() * 3 + 2,
        alpha: 0.7,
        life:  1.0
      });
      setTimeout(spawnMeteor, Math.random() * 4000 + 2500);
    }
    spawnMeteor();

    var t = 0;
    function draw() {
      ctx.clearRect(0, 0, canvas.width, canvas.height);
      t += 0.016;

      // Stars
      stars.forEach(function (s) {
        s.twinkle += 0.018;
        var a = s.alpha * (0.7 + 0.3 * Math.sin(s.twinkle));
        ctx.beginPath();
        ctx.arc(s.x, s.y, s.r, 0, Math.PI * 2);
        ctx.fillStyle = 'rgba(248,244,232,' + a + ')';
        ctx.fill();
        // Drift slightly
        s.y += s.speed * 0.06;
        if (s.y > canvas.height) { s.y = 0; s.x = Math.random() * canvas.width; }
      });

      // Meteors
      meteors = meteors.filter(function (m) { return m.life > 0; });
      meteors.forEach(function (m) {
        m.life -= 0.018;
        m.x    -= m.speed * 1.5;
        m.y    += m.speed * 0.6;
        ctx.save();
        ctx.globalAlpha = m.life * m.alpha;
        var grad = ctx.createLinearGradient(m.x, m.y, m.x + m.len, m.y - m.len * 0.4);
        grad.addColorStop(0, 'rgba(245,200,66,0)');
        grad.addColorStop(1, 'rgba(245,200,66,0.9)');
        ctx.strokeStyle = grad;
        ctx.lineWidth   = 1.5;
        ctx.beginPath();
        ctx.moveTo(m.x, m.y);
        ctx.lineTo(m.x + m.len, m.y - m.len * 0.4);
        ctx.stroke();
        ctx.restore();
      });

      requestAnimationFrame(draw);
    }
    draw();
  }


  /* ═══════════════════════════════════════════════════════════
     12. KEYBOARD HANDLER
  ═══════════════════════════════════════════════════════════ */
  function onKeyDown(e) {
    if (e.key === 'Enter') {
      e.preventDefault();
      var val = inputEl.value;
      inputEl.value = '';
      closeAutocomplete();
      showSuggestions();
      executeCommand(val);

    } else if (e.key === 'Tab') {
      e.preventDefault();
      // Autocomplete via Tab
      var val = inputEl.value.trim();
      if (!val) return;
      var matches = ALL_COMPLETIONS.filter(function (c) {
        return c.cmd.toLowerCase().startsWith(val.toLowerCase()) &&
               c.cmd.toLowerCase() !== val.toLowerCase();
      });
      if (matches.length === 1) {
        inputEl.value = matches[0].cmd;
        closeAutocomplete();
      } else if (matches.length > 1) {
        buildAutocomplete(val);
        selectAcItem(1);
      }

    } else if (e.key === 'ArrowUp') {
      e.preventDefault();
      // If autocomplete open → navigate it
      if (autocompleteEl.children.length) { selectAcItem(-1); return; }
      // Else history
      if (!cmdHistory.length) return;
      historyIndex = Math.min(historyIndex + 1, cmdHistory.length - 1);
      inputEl.value = cmdHistory[historyIndex];
      setTimeout(function () {
        inputEl.selectionStart = inputEl.selectionEnd = inputEl.value.length;
      }, 0);

    } else if (e.key === 'ArrowDown') {
      e.preventDefault();
      if (autocompleteEl.children.length) { selectAcItem(1); return; }
      if (historyIndex <= 0) { historyIndex = -1; inputEl.value = ''; return; }
      historyIndex--;
      inputEl.value = cmdHistory[historyIndex];

    } else if (e.key === 'Escape') {
      closeAutocomplete();
    }
  }

  function onInput() {
    var val = inputEl.value;
    if (!val.trim()) {
      closeAutocomplete();
      showSuggestions();
    } else {
      hideSuggestions();
      buildAutocomplete(val);
    }
  }

  /* ═══════════════════════════════════════════════════════════
     13. BOOT SEQUENCE
  ═══════════════════════════════════════════════════════════ */
  function runBoot() {
    var bootLines = [
      { text: '  Starry Team Explorer  v3.0.0', cls: 'line-info' },
      { text: '  ' + '─'.repeat(36), cls: 'line-muted' },
      { text: '  Status:  ✔ All systems operational', cls: 'line-success' },
      { text: '  Members: ' + TEAM.length + '  |  Projects: 12  |  Commits: 4,271' },
      { text: '' },
      { text: '  Type \'help\' for all commands.', cls: 'line-muted' },
      { text: '  Tip: press Tab for autocomplete.', cls: 'line-muted' },
      { text: '' }
    ];
    typeLines(bootLines, 18, 55, function () {
      inputEl.focus();
      showSuggestions();
    });
  }

  /* ═══════════════════════════════════════════════════════════
     14. INIT
  ═══════════════════════════════════════════════════════════ */
  function init() {
    outputEl        = document.getElementById('terminal-output');
    inputEl         = document.getElementById('terminal-input');
    suggestionsEl   = document.getElementById('terminal-suggestions');
    autocompleteEl  = document.getElementById('terminal-autocomplete');
    previewIdleEl   = document.getElementById('preview-idle');
    previewProfileEl= document.getElementById('preview-profile');
    previewLoadingEl= document.getElementById('preview-loading');
    previewTitleEl  = document.getElementById('preview-title');
    previewNameEl   = document.getElementById('preview-name');
    previewRoleEl   = document.getElementById('preview-role');
    previewExpEl    = document.getElementById('preview-exp');
    previewBioEl    = document.getElementById('preview-bio');
    previewSkillsEl = document.getElementById('preview-skills');
    previewLinksEl  = document.getElementById('preview-links');
    previewAvatarEl = document.getElementById('preview-avatar');
    statusMemberEl  = document.getElementById('statusbar-current-member');
    statusCmdEl     = document.getElementById('statusbar-last-cmd');
    statusTimeEl    = document.getElementById('statusbar-time');
    clearBtnEl      = document.getElementById('terminal-clear-btn');
    splitEl         = document.querySelector('.terminal-split');

    if (!outputEl || !inputEl) return; // not on this page

    // Input events
    inputEl.addEventListener('keydown', onKeyDown);
    inputEl.addEventListener('input',   onInput);

    // Click anywhere in terminal body → focus input
    outputEl.addEventListener('click', function () { inputEl.focus(); });

    // Clear button
    if (clearBtnEl) {
      clearBtnEl.addEventListener('click', function () {
        clearOutput();
        showIdle();
        inputEl.focus();
      });
    }

    // Suggestion chips
    if (suggestionsEl) {
      suggestionsEl.querySelectorAll('.suggestion-chip').forEach(function (chip) {
        chip.addEventListener('click', function () {
          var cmd = chip.getAttribute('data-cmd');
          if (cmd) {
            inputEl.value = '';
            closeAutocomplete();
            executeCommand(cmd);
          }
          inputEl.focus();
        });
      });
    }

    // Status bar clock
    updateStatusTime();
    setInterval(updateStatusTime, 30000);

    // Starfield
    initStarfield();

    // Boot
    runBoot();
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }

})();
