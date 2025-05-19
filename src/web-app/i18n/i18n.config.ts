export const locales = ['en-US', 'ru-RU'] as const;
export type Locale = (typeof locales)[number];
export const defaultLocale: Locale = locales[0];
