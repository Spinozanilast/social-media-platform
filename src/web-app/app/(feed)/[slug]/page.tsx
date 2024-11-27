import ProfileService from '@services/profile';
import { Profile, User } from '@models/user/user';
import UserService from '@services/user';
import UserProfile from '@/app/components/UserProfile';

async function getProfileImage(userId: string): Promise<Blob | null> {
    const profileImage: Blob = await ProfileService.getProfileImage(userId);

    if (profileImage.size === 0) {
        return null;
    }

    return profileImage;
}

const getUser = async (userIdOrUsername: string): Promise<User> =>
    await UserService.getUser(userIdOrUsername);
const getProfileData = async (userId: string): Promise<Profile> =>
    await ProfileService.getProfileData(userId);
interface UserPageProps {
    params: { slug: string };
}

export default async function Page({ params }: UserPageProps) {
    const userId = params.slug;

    const user: User = await getUser(userId);
    const [profileInfo, profileImage] = await Promise.all([
        getProfileData(user.id),
        getProfileImage(user.id),
    ]);

    console.log(profileInfo);

    return (
        <div className="bg-background-secondary rounded-md grid grid-cols-3 max-w-full mx-page-part">
            <aside style={{ fontFamily: 'Share Tech Mono' }}>
                <UserProfile
                    profileImage={profileImage}
                    user={user}
                    profileInfo={profileInfo}
                ></UserProfile>
            </aside>
            <main className="page-column "></main>
            <aside className="page-column "></aside>
        </div>
    );
}
