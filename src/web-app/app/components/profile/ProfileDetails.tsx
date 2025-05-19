import { Profile } from '@api/profile/types';
import { getLinkStyle } from '@components/special/LinkStyle';
import Link from 'next/link';
import React from 'react';
import { Roboto } from 'next/font/google';

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
        <div className={`${roboto.className} flex flex-col gap-2`}>
            {info.about && (
                <div className="grid grid-cols-2 gap-1">
                    {info.about && (
                        <div>
                            <h2 className="profile-section-header">
                                {translations.about_title}:
                            </h2>
                            <p className="text-base text-balance">
                                {info.about}
                            </p>
                        </div>
                    )}
                </div>
            )}
            {!isCollectionEmpty<string>(info.interests) && (
                <div className="flex flex-col gap-2 w-full">
                    <h2 className="profile-section-header font-light">
                        {translations.interests_title}:
                    </h2>
                    <div className="flex flex-row gap-2 flex-wrap items-center justify-center">
                        {info.interests!.map((interest) => (
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
            {!isCollectionEmpty<string>(info.references) && (
                <div className="flex flex-col gap-2 w-full">
                    <h2 className="profile-section-header font-light">
                        {translations.references_title}:
                    </h2>
                    <div className="flex gap-2 flex-wrap">
                        {info.references!.map((link) => {
                            const linkStyle = getLinkStyle(link);
                            return (
                                <Link
                                    href={linkStyle.validUrl}
                                    rel="noreferrer"
                                    target="_blank"
                                    key={link}
                                    className={`font-light border-solid border-2 ${linkStyle.linkStyle.borderColor} ref-pill
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
                </div>
            )}
        </div>
    );
};

const isCollectionEmpty = function <T>(elements?: T[]): boolean {
    return !!elements && elements.length === 0;
};
