import Profile from '@/app/models/Profiles/profile';
import User from '@/app/models/Users/user';
import UserProfile from '@components/UserProfile';
import UserStoriesContainer from '@components/containers/UserStoriesContainer';

interface UserProfileProps {
    user: User;
    profileInfo: Profile;
}

const ProfileTranslationSection: string = 'Profile';

const UserPage: React.FC<UserProfileProps> = ({ user, profileInfo }) => {
    return (
        <div
            className="bg-gradient-to-br p-1 from-white to-default-200 dark:from-default-50
                dark:to-black rounded-md grid grid-cols-1 md:grid-cols-3 md:grid-rows-1
                divide-y-1 md:divide-y-0 md:divide-x-1 max-w-full mx-page-part"
        >
            <aside style={{ fontFamily: 'Share Tech Mono' }}>
                <UserProfile
                    user={user}
                    profileInfo={profileInfo}
                ></UserProfile>
            </aside>
            <main className="page-column">
                <UserStoriesContainer
                    userId={user.id}
                    userName={user.userName}
                />
            </main>
            <aside className="page-column"></aside>
        </div>
    );
};

export default UserPage;
