'use client';
import Image from 'next/image';
import React from 'react';
import IdentityService from '../../api/services/user';
import {
    Button,
    cn,
    Dropdown,
    DropdownItem,
    DropdownMenu,
    DropdownSection,
    DropdownTrigger,
    Link,
} from '@heroui/react';
import { useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';
import { EllipsisVertical, LogOut, UserRound } from 'lucide-react';

const CurrentUserTranslationSection: string = 'CurrentUser';

const CurrentUser = () => {
    const router = useRouter();
    const t = useTranslations(CurrentUserTranslationSection);
    const iconClasses = 'text-default-500 pointer-events-none flex-shrink-0';
    const imageUrl: string = '/profile.svg';

    const handleLogout = async () => {
        console.log(await IdentityService.logOut());
    };

    const handleRedirectToProfile = () => {
        const profileUrl = IdentityService.getCurrentUser()?.userName;
        if (profileUrl) {
            router.push(profileUrl);
        }
    };

    return (
        <Dropdown placement="left-start">
            <DropdownTrigger>
                <Button variant="bordered">
                    <Image
                        className="rounded-md p-2"
                        src={imageUrl}
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
                        key="profile"
                        startContent={<UserRound />}
                        classNames={{
                            description: 'text-default-500 pointer-events-none',
                        }}
                        description={t('profile_description')}
                        onPress={handleRedirectToProfile}
                    >
                        {t('profile')}
                    </DropdownItem>
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
                    <DropdownItem
                        key="logout"
                        color="danger"
                        className="text-danger"
                        classNames={{
                            description: 'text-default-500 pointer-events-none',
                        }}
                        description={t('logout_description')}
                        startContent={
                            <LogOut
                                className={cn(iconClasses, 'text-danger')}
                            />
                        }
                        onPress={handleLogout}
                    >
                        {t('logout')}
                    </DropdownItem>
                </DropdownSection>
            </DropdownMenu>
        </Dropdown>
    );
};

export default CurrentUser;
