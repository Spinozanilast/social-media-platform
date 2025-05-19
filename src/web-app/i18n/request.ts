import { getRequestConfig } from 'next-intl/server';
import { getUserLocale } from '@/app/utils/locale';

export default getRequestConfig(async () => {
    const locale = await getUserLocale();
    const localeName = getLocaleName(locale);
    return {
        locale: localeName,
        messages: (await import(`./locales/${localeName}.json`)).default,
    };
});

function getLocaleName(localeHeaderValue: string) {
    return localeHeaderValue.substring(0, 5);
}
