'use client';

import AuthService from '@api/auth/service';
import React from 'react';
import { DropdownItem } from '@heroui/react';
import { User } from '@/app/api/auth/types';
import { Router } from '@/app/components/layout/menu/ActionsMenu';
import { LogOut, UserRound } from 'lucide-react';

type UserLoggedActionsProps = {
    router: Router;
    currentUser: User;
    translations: {
        profile_description: string;
        profile: string;
        logout_description: string;
        logout: string;
    };
};

const UserLoggedActions = ({
    router,
    currentUser,
    translations,
}: UserLoggedActionsProps) => {
    const handleRedirectToProfile = () => {
        const profileUrl = currentUser.userName;
        if (profileUrl) {
            router.push(profileUrl);
        }
    };

    return (
        <>
            <DropdownItem
                key="profile"
                startContent={<UserRound />}
                classNames={{
                    description: 'text-default-500 pointer-events-none',
                }}
                description={translations.profile_description}
                onPress={handleRedirectToProfile}
            >
                {translations.profile}
            </DropdownItem>
            <DropdownItem
                key="logout"
                color="danger"
                className="text-danger"
                classNames={{
                    description: 'text-default-500 pointer-events-none',
                }}
                description={translations.logout_description}
                startContent={
                    <LogOut className="text-default-500 pointer-events-none flex-shrink-0text-danger" />
                }
                onPress={async () => {
                    await AuthService.logOut();
                    router.refresh();
                }}
            >
                {translations.logout}
            </DropdownItem>
        </>
    );
};

export default UserLoggedActions;
