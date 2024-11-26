// components/UserProfile.tsx
import Image from 'next/image';
import React from 'react';
import { Profile, User } from '@models/user/user';
import { getLinkStyle } from './utils/LinkStyle';
import Link from 'next/link';

interface UserProfileProps {
    profileImage: Blob | null;
    user: User;
    profileInfo: Profile;
}

const UserProfile: React.FC<UserProfileProps> = ({
    profileImage,
    user,
    profileInfo,
}) => {
    const imageUrl: string = profileImage
        ? URL.createObjectURL(profileImage)
        : '/profile.svg';
    return (
        <div className="page-column flex flex-col gap-5 font-thin">
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
            <div
                className="flex flex-col gap-2"
                style={{ fontFamily: 'Roboto' }}
            >
                {(profileInfo.about || profileInfo.anything) && (
                    <div className="flex flex-col gap-1">
                        {profileInfo.about && (
                            <div>
                                <h2 className="semi-transparent">About:</h2>
                                <p className="text-base text-gray-400">
                                    {profileInfo.about}
                                </p>
                            </div>
                        )}
                        {profileInfo.anything && (
                            <div>
                                <h2 className="semi-transparent">Anything:</h2>
                                <p className="text-base text-gray-400">
                                    {profileInfo.anything}
                                </p>
                            </div>
                        )}
                    </div>
                )}
                {profileInfo.interests.length > 0 && (
                    <div className="flex flex-col gap-2 w-fit">
                        <h2 className="semi-transparent">Interests:</h2>
                        <div className="flex flex-row gap-2">
                            {profileInfo.interests.map((interest) => (
                                <div
                                    key={interest}
                                    className="category-pill whitespace-nowrap text-center"
                                >
                                    {interest}
                                </div>
                            ))}
                        </div>
                    </div>
                )}
                {profileInfo.refs.length > 0 && (
                    <div className="flex flex-col gap-2 w-fit">
                        <h2 className="semi-transparent">Interests:</h2>
                        <div className="flex flex-row gap-2">
                            {profileInfo.refs.map((ref) => {
                                const linkStyle = getLinkStyle(ref);
                                return (
                                    <div
                                        key={ref}
                                        className={`border-solid border-2 ${linkStyle.linkStyle.borderColor} ref-pill whitespace-nowrap text-center`}
                                    >
                                        <a
                                            href={linkStyle.validUrl}
                                            rel="noreferrer"
                                            target="_blank"
                                        >
                                            {linkStyle.linkStyle.icon}
                                            {ref}
                                        </a>
                                    </div>
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
