'use client';
import { Spinner } from '@heroui/react';
import { useEffect, useState } from 'react';
import AuthService from '~/api/auth/service';
import { User } from '~/api/auth/types';
import { ProfilesService } from '~/api/profile/service';
import { Profile } from '~/api/profile/types';
import StoriesService from '~/api/story/service';
import { Story } from '~/api/story/types';
import UserProfile from '~/components/profile/user-profile';
import UserStoriesContainer from '~/components/profile/user-stories';
import UserDoesntExists from '~/components/errors/user-doesnt-exists';
import { useAuth } from '~providers/auth-provider';

type UserProfileWrapperProps = {
    slug: string;
};

export default function UserProfileWrapper({
    slug
}: UserProfileWrapperProps) {
    const [user, setUser] = useState<User | null>(null);
    const { user: authorizedUser } = useAuth();
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

                const [profileData, storiesData] = await Promise.all([
                    ProfilesService.get(userData.id),
                    StoriesService.getAllStories({ authorId: userData.id }),
                ]);

                if (!profileData) {
                    setShowNotFound(true);
                    return;
                }

                setUser(userData);
                setProfile(profileData);
                setInitialStories(storiesData);
                setIsOwner(authorizedUser?.id === userData.id);
            } catch (err) {
                setError(true);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [authorizedUser?.id, slug]);

    if (showNotFound) {
        return <UserDoesntExists />;
    }

    if (error) return <div className="p-4 text-red-500">Error loading profile</div>;
    if (loading) return <Spinner color="warning" label="Loading..." />;

    return (
        <div
            className="bg-gradient-to-br p-1 from-default-100 to-default-200 dark:from-default-50
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