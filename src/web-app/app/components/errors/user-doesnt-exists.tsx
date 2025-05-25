'use client';
import { Button } from "@heroui/react";
import { Frown, Link } from "lucide-react";
import { useTranslations } from "next-intl";

export default function UserDoesntExists() {
    const t = useTranslations('UserNotFound');

    return (
        <div className="flex flex-col items-center justify-center gap-4">
            <Frown className="w-10 text-accent-orange" />
            <h2 className="text-xl font-semibold">404 Not Found</h2>
            <p>{t('user_doesnt_exist')}</p>
            <Button
                variant="shadow"
                color="primary"
                as={Link}
                href="/"
                className="mt-4 rounded-md"
            >
                {t('go_home')}
            </Button>
        </div>
    );
}