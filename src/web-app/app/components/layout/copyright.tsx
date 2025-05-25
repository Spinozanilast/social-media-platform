import { useLocale, useTranslations } from "next-intl";
import { Roboto } from "next/font/google";

type CopyrightProps = {
    mt?: number;
};

const roboto = Roboto({
    subsets: ['latin'],
    weight: ['200'],
});

export default function Copyright({ mt }: CopyrightProps) {
    const locale = useLocale();
    const t = useTranslations("Copyright");

    return (
        <div className={`w-full ${mt ? `mt-${mt}` : ''}`}>
            <p className="text-center text-sm share-tech-mono "
                style={locale === 'ru-RU' ? roboto.style : { fontFamily: 'Share Tech Mono' }}
            >
                Copyright © 2025{' '}
                <a
                    href="https://github.com/Spinozanilast/social-media-platform/"
                    className="text-accent-orange font-bold text-base"
                >
                    Platform
                </a>
                . {t('copyright')}
            </p>
        </div>
    );
}
