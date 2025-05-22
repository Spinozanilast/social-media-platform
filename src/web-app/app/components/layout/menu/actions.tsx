'use client';

import AuthService from '~api/auth/service';
import Image from 'next/image';
import React, { useEffect } from 'react';
import {
    Button,
    Dropdown,
    DropdownItem,
    DropdownMenu,
    DropdownSection,
    DropdownTrigger,
} from '@heroui/react';
import { useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { EllipsisVertical } from 'lucide-react';
import { User } from '~api/auth/types';
import UserLoggedActions from '~/components/layout/menu/user-logged-actions';
import AnonymousUserActions from '~/components/layout/menu/anonymous-actions';

export default function UserActions() {
    const t = useTranslations('CurrentUser');

    const router = useRouter();
    const [currentUser, setCurrentUser] = React.useState<User | null>(null);

    useEffect(() => {
        AuthService.getCurrentUser().then((user) => setCurrentUser(user));
    }, []);

    const isUserLogged = currentUser !== null;

    return (
        <Dropdown placement="left-start">
            <DropdownTrigger>
                <Button variant="bordered">
                    <Image
                        className="rounded-md p-2"
                        src="/profile.svg"
                        alt="current user profile image"
                        width={36}
                        height={36}
                    />
                </Button>
            </DropdownTrigger>
            <DropdownMenu
                aria-label="Profile Menu"
                id="account-menu"
                variant="faded"
            >
                <DropdownSection
                    title={t('profile_actions')}
                    aria-label={t('profile_actions')}
                    showDivider
                >
                    <DropdownItem
                        key="settings"
                        description={t('settings_description')}
                        classNames={{
                            description: 'text-default-500 pointer-events-none',
                        }}
                        startContent={<EllipsisVertical />}
                    >
                        {t('settings')}
                    </DropdownItem>
                </DropdownSection>
                <DropdownSection
                    title={t('account_actions')}
                    aria-label="Account actions"
                >
                    {isUserLogged
                        ? UserLoggedActions({
                              router: router,
                              currentUser: currentUser,
                              translations: {
                                  profile_description: t('profile_description'),
                                  profile: t('profile'),
                                  logout_description: t('logout_description'),
                                  logout: t('logout'),
                              },
                          })
                        : AnonymousUserActions({
                              router: router,
                              translations: {
                                  login_description: t('login_description'),
                                  login: t('login'),
                                  register: t('register'),
                              },
                          })}
                </DropdownSection>
            </DropdownMenu>
        </Dropdown>
    );
}

export type Router = ReturnType<typeof useRouter>;
