'use client';
import { Button, Spinner } from '@heroui/react';
import { Frown } from 'lucide-react';
import { useTranslations } from 'next-intl';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';
import AuthService from '~/api/auth/service';
import { AuthResponse, User } from '~/api/auth/types';
import { ProfilesService } from '~/api/profile/service';
import { Profile } from '~/api/profile/types';
import StoriesService from '~/api/story/service';
import { Story } from '~/api/story/types';
import UserProfile from '~/components/profile/user-profile';
import UserStoriesContainer from '~/components/profile/user-stories';

type UserProfileWrapperProps = {
    slug: string;
};

export default function UserProfileWrapper({
    slug
}: UserProfileWrapperProps) {
    const router = useRouter();

    const tNotFound = useTranslations('UserNotFound');

    const [user, setUser] = useState<User | null>(null);
    const [profile, setProfile] = useState<Profile | null>(null);
    const [initialStories, setInitialStories] = useState<Story[] | []>([]);
    const [isOwner, setIsOwner] = useState(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(false);
    const [showNotFound, setShowNotFound] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            try {
                setLoading(true);
                setError(false);
                setShowNotFound(false);

                const userData = await AuthService.getUser(slug);
                if (!userData) {
                    setShowNotFound(true);
                    return;
                }

                const [profileData, storiesData, authData] = await Promise.all([
                    ProfilesService.get(userData.id),
                    StoriesService.getAllStories({ authorId: userData.id }),
                    AuthService.getCurrentUser()
                ]);

                if (!profileData) {
                    setShowNotFound(true);
                    return;
                }

                setUser(userData);
                setProfile(profileData);
                setInitialStories(storiesData);
                setIsOwner(authData?.id === userData.id);
            } catch (err) {
                setError(true);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [slug]);

    if (showNotFound) {
        return (
            <div className="flex flex-col items-center justify-center gap-4">
                <Frown className="w-10 text-accent-orange" />
                <h2 className="text-xl font-semibold">404 Not Found</h2>
                <p>{tNotFound('user_doesnt_exist')}</p>
                <Button
                    variant="shadow"
                    color="primary"
                    as={Link}
                    href="/"
                    className="mt-4 rounded-md"
                >
                    {tNotFound('go_home')}
                </Button>
            </div>
        );
    }

    if (error) return <div className="p-4 text-red-500">Error loading profile</div>;
    if (loading) return <Spinner color="warning" label="Loading..." />;

    return (
        <div
            className="bg-gradient-to-br p-1 from-white to-default-200 dark:from-default-50
                dark:to-black rounded-md flex flex-col max-w-full mt-2 mx-page-part"
        >
            <div className="share-tech-mono">
                {user && profile ? (
                    <UserProfile
                        user={user}
                        profile={profile}
                        isOwner={isOwner}
                    />) : null}
            </div>
            {(initialStories.length > 0 || isOwner) && (
                <div className="page-column">
                    {user ? (
                        <UserStoriesContainer
                            userId={user.id}
                            initialStories={initialStories}
                            isOwner={isOwner}
                        />
                    ) : null}
                </div>
            )}
        </div>
    );
}