import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  // Test files location
  testDir: 'tests/e2e',
  testMatch: '**/*.e2e.test.js',

  // Timeout per test (ms)
  timeout: 30_000,

  // Run tests in parallel
  fullyParallel: true,

  // Fail the build on CI if tests are accidentally left in focused mode
  forbidOnly: !!process.env.CI,

  // Retry on CI only
  retries: process.env.CI ? 2 : 0,

  // Reporter
  reporter: 'list',

  // Shared settings applied to all projects
  use: {
    // The running ASP.NET Core dev server
    baseURL: 'http://localhost:5000',

    // Collect traces on first retry for easier debugging
    trace: 'on-first-retry',
  },

  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],
});
