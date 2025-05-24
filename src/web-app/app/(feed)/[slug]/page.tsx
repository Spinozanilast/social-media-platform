import React from 'react';
import UserProfileWrapper from '~/components/profile/user-profile-wrapper';

interface UserPageProps {
    params: Promise<{ slug: string }>;
}

export default async function Page({ params }: UserPageProps) {
    const { slug } = await params;
    return (
        <UserProfileWrapper slug={slug} />
    );
}
