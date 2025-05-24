import { Profile } from '~api/profile/types';
import { getLinkStyle } from '~/utils/link-style';
import Link from 'next/link';
import React from 'react';
import { Roboto } from 'next/font/google';
import { LinkIcon, MessageCircleHeart } from 'lucide-react';
import ProfileSectionWrapper from './profile-section-wrapper';
import { isStringEmptyOrNull } from '~/utils/helpers';
import { Badge, Chip } from '@heroui/react';

const roboto = Roboto({
    subsets: ['latin'],
    weight: ['300', '500'],
});

type ProfileDetailsProps = {
    info: Profile;

    translations: {
        about_title: string;
        interests_title: string;
        references_title: string;
    };
};

export const ProfileDetails = ({ info, translations }: ProfileDetailsProps) => {
    return (
        <div className={`${roboto.className} mt-4 flex flex-row gap-2`}>
            {!isStringEmptyOrNull(info.about) && (
                <div className="flex flex-col gap-1">
                    {info.about && (
                        <>
                            <h2 className="profile-section-header w-full">
                                {translations.about_title}:
                            </h2>
                            <p className="text-base text-balance">
                                {info.about}
                            </p>
                        </>
                    )}
                </div>
            )}
            {!isCollectionEmpty<string>(info.interests) && (
                <ProfileSectionWrapper>
                    <div className="flex flex-col gap-2 w-full">
                        <div className="flex-centered-row justify-between gap-4">
                            <h2 className="profile-section-header font-light">
                                {translations.interests_title}:
                            </h2>
                            <MessageCircleHeart className="utility-small-icon" />
                        </div>
                        <div className="flex flex-row gap-4 h-full flex-wrap items-center justify-center mt-2">
                            {info.interests!.map((interest) => (
                                <Chip variant='dot'
                                    key={interest} color="primary"
                                    className="category-pill whitespace-nowrap text-center group bg-transparent"
                                >
                                    <span className="group-hover:text-accent-orange">
                                        {interest}
                                    </span>
                                </Chip>
                            ))}
                        </div>
                    </div>
                </ProfileSectionWrapper>
            )}
            {!isCollectionEmpty<string>(info.references) && (
                <ProfileSectionWrapper>
                    <div className="flex-centered-row justify-between">
                        <h2 className="profile-section-header font-light">
                            {translations.references_title}:
                        </h2>
                        <LinkIcon className="utility-small-icon" />
                    </div>
                    <div className="flex gap-2 flex-wrap mt-2 justify-center w-full">
                        {info.references!.map((link) => {
                            const linkStyle = getLinkStyle(link);
                            return (
                                <Link
                                    href={linkStyle.validUrl}
                                    rel="noreferrer"
                                    target="_blank"
                                    key={link}
                                    className={`font-light whitespace-nowrap border-solid border-2 ${linkStyle.linkStyle.borderColor} ref-pill
                                    text-center p-2 rounded-md flex items-center justify-center transition-all
                                    duration-300 group`}
                                >
                                    <span
                                        className="transform group-hover:scale-110 group-hover:translate-y-[2px] transition-all
                                            duration-300"
                                    >
                                        {linkStyle.linkStyle.icon}
                                    </span>
                                    <span className="opacity-90 transition-opacity duration-300 group-hover:opacity-100 ml-2">
                                        {link}
                                    </span>
                                </Link>
                            );
                        })}
                    </div>
                </ProfileSectionWrapper>
            )}
        </div>
    );
};

const isCollectionEmpty = function <T>(elements?: T[]): boolean {
    return !!elements && elements.length === 0;
};
