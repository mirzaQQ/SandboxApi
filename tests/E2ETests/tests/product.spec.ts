import { test, expect } from '@playwright/test';

test('Full-stack health check and UI load', async ({ page, request }) => {
    const apiResponse = await request.get('http://localhost:5011/openapi/v1.json');
    expect(apiResponse.status()).toBe(200);

    await page.goto('/');
    await expect(page.locator('#root')).toBeVisible();
});