'use client';

import { Button, Link } from '@heroui/react';
import { Frown } from 'lucide-react';
import { useTranslations } from 'next-intl';

export default function NotFound() {
    const t = useTranslations('UserNotFound');

    return (
        <div className="flex flex-col items-center justify-center gap-4">
            <Frown className="w-10 text-accent-orange" />
            <h2 className="text-xl font-semibold">404 Not Found</h2>
            <p>{t('user_doesnt_exist')}</p>
            <Button
                as={Link}
                href="/"
                className="mt-4 rounded-md bg-blue-500 px-4 py-2 text-sm text-white transition-colors
                    hover:bg-blue-400"
            >
                {t('go_home')}
            </Button>
        </div>
    );
}
