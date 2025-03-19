import { notFound } from 'next/navigation';
import UserPage from '@components/pages/UserPage';

const getUser = async (userIdOrUsername: string): Promise<User | null> =>
    await Identity.getUser(userIdOrUsername);
const getProfileData = async (userId: string): Promise<Profile | null> =>
    await ProfileService.getProfileData(userId);

interface UserPageProps {
    params: { slug: string };
}

export default async function Page({ params }: UserPageProps) {
    const userId = params.slug;

    const user = await getUser(userId);

    if (!user) {
        return notFound();
    }

    const profileInfo = await getProfileData(user.id);

    if (!profileInfo) {
        return notFound();
    }

    return <UserPage user={user} profileInfo={profileInfo} />;
}
