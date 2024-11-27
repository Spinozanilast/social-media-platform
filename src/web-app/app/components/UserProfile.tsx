// components/UserProfile.tsx
import Image from 'next/image';
import React from 'react';
import { Profile, User } from '@models/user/user';
import { getLinkStyle } from './utils/LinkStyle';
import Link from 'next/link';
import { Roboto } from 'next/font/google';
import { useTranslations } from 'next-intl';

const roboto = Roboto({
    subsets: ['latin'],
    weight: ['300', '500'],
});

interface UserProfileProps {
    profileImage: Blob | null;
    user: User;
    profileInfo: Profile;
}

const ProfileTranslationSection: string = 'Profile';

const UserProfile: React.FC<UserProfileProps> = ({
    profileImage,
    user,
    profileInfo,
}) => {
    const t = useTranslations(ProfileTranslationSection);
    const imageUrl: string = profileImage
        ? URL.createObjectURL(profileImage)
        : '/profile.svg';
    return (
        <div className="page-column flex flex-col gap-5 font-thin max-w-full">
            <div className="flex flex-row gap-5 items-center">
                <Image
                    className="rounded-md p-2 shadow shadow-accent-orange cursor-pointer hover:shadow-lg hover:shadow-accent-orange"
                    src={imageUrl}
                    alt="Profile image"
                    width={48}
                    height={48}
                />
                <div className="flex flex-col gap-1">
                    <h2 className="text-2xl font-bold">
                        {user.firstName} {user.lastName}
                    </h2>
                    <h2 className="text-base text-gray-400">
                        @{user.username}
                    </h2>
                </div>
            </div>
            <div className={`${roboto.className} flex flex-col gap-2`}>
                {(profileInfo.about || profileInfo.anything) && (
                    <div className="grid grid-cols-2 gap-1">
                        {profileInfo.about && (
                            <div>
                                <h2 className=" profile-header">
                                    {t('about_title')}:
                                </h2>
                                <p className="text-base text-balance">
                                    {profileInfo.about}
                                </p>
                            </div>
                        )}
                        {profileInfo.anything && (
                            <div className="text-right">
                                <h2 className=" profile-header">
                                    {t('anything_title')}:
                                </h2>
                                <p className="text-base text-balance">
                                    {profileInfo.anything}
                                </p>
                            </div>
                        )}
                    </div>
                )}
                {profileInfo.interests.length > 0 && (
                    <div className="flex flex-col gap-2 w-full">
                        <h2 className=" profile-header font-light">
                            {t('interests_title')}:
                        </h2>
                        <div className="flex flex-row gap-2 flex-wrap">
                            {profileInfo.interests.map((interest) => (
                                <div
                                    key={interest}
                                    className="category-pill whitespace-nowrap text-center group"
                                >
                                    <span className="group-hover:text-accent-orange">
                                        {interest}
                                    </span>
                                </div>
                            ))}
                        </div>
                    </div>
                )}
                {profileInfo.refs.length > 0 && (
                    <div className="flex flex-col gap-2 w-full">
                        <h2 className=" profile-header font-light">
                            {t('references_title')}:
                        </h2>
                        <div className="flex gap-2 flex-wrap">
                            {profileInfo.refs.map((ref) => {
                                const linkStyle = getLinkStyle(ref);
                                return (
                                    <Link
                                        href={linkStyle.validUrl}
                                        rel="noreferrer"
                                        target="_blank"
                                        key={ref}
                                        className={`min-w-full font-light border-solid border-2 ${linkStyle.linkStyle.borderColor} ref-pill text-center p-2 rounded-md flex items-center justify-center transition-all duration-300 group`}
                                    >
                                        <span className="transform group-hover:scale-110 group-hover:translate-y-[2px] transition-all duration-300">
                                            {linkStyle.linkStyle.icon}
                                        </span>
                                        <span className="opacity-90 transition-opacity duration-300 group-hover:opacity-100 ml-2">
                                            {ref}
                                        </span>
                                    </Link>
                                );
                            })}
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
};

export default UserProfile;
