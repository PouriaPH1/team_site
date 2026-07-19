import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    // Pick up all test files under tests/unit/, tests/pbt/, and tests/integration/
    include: ['tests/**/*.test.js'],
    // Exclude Playwright e2e tests — those run via the playwright CLI
    exclude: ['tests/e2e/**'],
    // Run in Node environment (no DOM needed for CSS/unit tests)
    environment: 'node',
    // Show verbose output for each test
    reporter: 'verbose',
  },
});
