'use client';

import React from 'react';
import { LogIn, UserPlus } from 'lucide-react';
import { DropdownItem } from '@heroui/react';
import { Router } from '~/components/layout/menu/actions';

type AnonymousUserActionsProps = {
    router: Router;
    translations: {
        login_description: string;
        login: string;
        register: string;
    };
};

export default function AnonymousUserActions({
    router,
    translations,
}: AnonymousUserActionsProps) {
    return (
        <>
            <DropdownItem
                key="login"
                color="primary"
                description={translations.login_description}
                startContent={<LogIn />}
                onPress={() => router.push('/login')}
            >
                {translations.login}
            </DropdownItem>
            <DropdownItem
                key="register"
                color="primary"
                startContent={<UserPlus />}
                onPress={() => router.push('/register')}
            >
                {translations.register}
            </DropdownItem>
        </>
    );
}
