'use client';

import { useTranslations } from 'next-intl';
import { Button, ButtonGroup, Link } from '@heroui/react';
import UserActions from '~/components/layout/menu/actions';
import React, { useCallback } from 'react';
import {
    BookOpenText,
    LayoutDashboard,
    LucideProps,
    MessageSquare,
} from 'lucide-react';
import { usePathname, useRouter } from 'next/navigation';

export type CompleteItemData = {
    id: number;
    name: string;
    url: string;
};

export type VisualCompleteItemData = CompleteItemData & {
    icon: React.ForwardRefExoticComponent<
        Omit<LucideProps, 'ref'> & React.RefAttributes<SVGSVGElement>
    >;
};

export const PersonalUrlPagesItems: VisualCompleteItemData[] = [
    {
        id: 1,
        name: 'messages',
        url: '/dialogs',
        icon: MessageSquare,
    },
    {
        id: 2,
        name: 'stories',
        url: '/stories',
        icon: BookOpenText,
    },
    {
        id: 3,
        name: 'board',
        url: '/board',
        icon: LayoutDashboard,
    },
];

export default function NavbarButtons() {
    const t = useTranslations('PagesNavbar');
    const pathname = usePathname();

    const isProfilePage = useCallback(() => !PersonalUrlPagesItems.some(
        (item) => item.url === pathname
    ), [pathname]);

    return (
        <ButtonGroup>
            <UserActions currentlyInProfile={isProfilePage()} />
            {PersonalUrlPagesItems.map((item) => (
                <Button
                    variant="flat"
                    as={Link}
                    color={pathname === item.url ? 'primary' : 'default'}
                    href={item.url}
                    key={item.id}
                    endContent={<item.icon size={16} />}
                >
                    {t(item.name)}
                </Button>
            ))}
        </ButtonGroup>
    );
}
