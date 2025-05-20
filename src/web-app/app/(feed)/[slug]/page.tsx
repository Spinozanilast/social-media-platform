import StoriesService from '~api/story/service';
import AuthService from '~api/auth/service';
import UserProfile from '~/components/user-profile';
import UserStoriesContainer from '~/components/containers/user-stories';
import React from 'react';
import { ProfilesService } from '~api/profile/service';
import { notFound } from 'next/navigation';
import { cookies } from 'next/headers';

interface UserPageProps {
    params: Promise<{ slug: string }>;
}

export default async function Page({ params }: UserPageProps) {
    const { slug } = await params;

    const user = await AuthService.getUser(slug);
    if (!user) return notFound();

    const profile = await ProfilesService.get(user.id);
    if (!profile) return notFound();

    const cookieStore = await cookies();
    const cookieString = cookieStore
        .getAll()
        .map((c) => `${c.name}=${c.value}`)
        .join('; ');

    const authorizedUser = await AuthService.getCurrentUser(cookieString);

    const initialStories = await StoriesService.getAllStories({
        authorId: user.id,
    });

    const isOwner = !!authorizedUser && authorizedUser.id === user.id;

    return (
        <div
            className="bg-gradient-to-br p-1 from-white to-default-200 dark:from-default-50
                dark:to-black rounded-md grid grid-cols-1 md:grid-cols-2 md:grid-rows-1
                divide-y-1 md:divide-y-0 md:divide-x-1 max-w-full mx-page-part"
        >
            <div className="share-tech-mono">
                <UserProfile
                    user={user}
                    profile={profile}
                    isOwner={isOwner}
                ></UserProfile>
            </div>
            {(initialStories.length > 0 || isOwner) && (
                <div className="page-column">
                    <UserStoriesContainer
                        userId={user.id}
                        initialStories={initialStories}
                        isOwner={isOwner}
                    />
                </div>
            )}
        </div>
    );
}
