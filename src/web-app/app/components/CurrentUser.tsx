'use client';
import Image from 'next/image';
import React from 'react';
import { FaUser } from 'react-icons/fa';
import { BsThreeDots } from 'react-icons/bs';
import { CiLogout } from 'react-icons/ci';
import IdentityService from '../api/services/user';
import {
    cn,
    Dropdown,
    DropdownItem,
    DropdownMenu,
    DropdownSection,
    DropdownTrigger,
    Link,
} from '@nextui-org/react';
import { useTranslations } from 'next-intl';
import { useRouter } from 'next/navigation';

const CurrentUserTranslationSection: string = 'CurrentUser';

const CurrentUser = () => {
    const router = useRouter();
    const t = useTranslations(CurrentUserTranslationSection);
    const iconClasses =
        'text-xl text-default-500 pointer-events-none flex-shrink-0';
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
        <Dropdown
            placement="left-start"
            showArrow
            classNames={{
                base: 'before:bg-default-200',
                content:
                    'py-1 px-1 border border-default-200 bg-gradient-to-br from-white to-default-200 dark:from-default-50 dark:to-black',
            }}
        >
            <DropdownTrigger>
                <Image
                    className="rounded-md p-2 shadow-accent-orange shadow-inner hover:shadow-lg hover:border-l-2 hover:border-accent-orange hover:border-solid"
                    src={imageUrl}
                    alt="curret user profile image"
                    width={36}
                    height={36}
                />
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
                        startContent={<FaUser />}
                        classNames={{
                            description: 'text-default-500 pointer-events-none',
                        }}
                        description={t('profile_description')}
                        onClick={handleRedirectToProfile}
                    >
                        {t('profile')}
                    </DropdownItem>
                    <DropdownItem
                        key="settings"
                        description={t('settings_description')}
                        classNames={{
                            description: 'text-default-500 pointer-events-none',
                        }}
                        startContent={<BsThreeDots />}
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
                            <CiLogout
                                className={cn(iconClasses, 'text-danger')}
                            />
                        }
                        onClick={handleLogout}
                    >
                        {t('logout')}
                    </DropdownItem>
                </DropdownSection>
            </DropdownMenu>
        </Dropdown>
    );
};

export default CurrentUser;
