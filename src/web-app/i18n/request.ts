import { getRequestConfig } from 'next-intl/server';
import { cookies, headers } from 'next/headers';

const StandardLocale = 'en-US';

// Load the translation file for the active locale
// on each request and make it available to our
// pages, components, etc.
export default getRequestConfig(async () => {
    const headersList = headers();
    const defaultLocale = headersList.get('accept-language');
    const locale =
        cookies().get('NEXT_LOCALE')?.value || defaultLocale || StandardLocale;
    const localeName = getLocaleName(locale);

    return {
        locale: localeName,
        messages: (await import(`./locales/${localeName}.json`)).default,
    };
});

function getLocaleName(localeHeaderValue: string) {
    return localeHeaderValue.substring(0, 5);
}
