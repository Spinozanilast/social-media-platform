'use client';
import React, { useCallback, useEffect, useState } from 'react';
import User from '@models/Users/user';
import { getLinkStyle } from './utils/LinkStyle';
import Link from 'next/link';
import { Roboto } from 'next/font/google';
import { useTranslations } from 'next-intl';
import IdentityService from '@api/services/user';
import ProfileService from '@api/services/profile';
import {
    Button,
    Image,
    Snippet,
    useDisclosure,
    Spinner,
} from '@nextui-org/react';
import { FaEdit } from 'react-icons/fa';
import EditProfileModal from './forms/EditProfileModal';
import ImageTooltip from './common/ImageTooltip';
import Profile from '@models/Profiles/profile';

const roboto = Roboto({
    subsets: ['latin'],
    weight: ['300', '500'],
});

interface UserProfileProps {
    user: User;
    profileInfo: Profile;
    enableEdit?: boolean;
}

const ProfileTranslationSection: string = 'Profile';

const UserProfile: React.FC<UserProfileProps> = ({ user, profileInfo }) => {
    const t = useTranslations(ProfileTranslationSection);

    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [currentProfileInfo, setCurrentProfileInfo] =
        useState<Profile>(profileInfo);
    const [currentProfileImage, setCurrentProfileImage] = useState<Blob | null>(
        null
    );

    const { isOpen, onOpen, onOpenChange } = useDisclosure();

    const fetchIsAuthenticated = useCallback(async () => {
        const isUserAuthenticated = await IdentityService.checkUserIdentity(
            user.userName
        );
        setIsAuthenticated(isUserAuthenticated);
    }, [user.userName]);

    const fetchProfileImage = useCallback(async () => {
        const profileImage = await ProfileService.getProfileImage(user.id);
        setCurrentProfileImage(profileImage);
    }, [user.id]);

    useEffect(() => {
        fetchProfileImage();
        fetchIsAuthenticated();
    }, [fetchIsAuthenticated, fetchProfileImage]);

    const handleSaveProfile = async (updatedProfile: Profile) => {
        if (
            updatedProfile.birthDate &&
            !isValidDateFormat(updatedProfile.birthDate.toString())
        ) {
            updatedProfile.birthDate = formatDate(
                updatedProfile.birthDate.toString()!
            );
        }
        try {
            await ProfileService.updateProfile(updatedProfile, user.id);
            setCurrentProfileInfo(updatedProfile);
            console.log('Profile updated:', updatedProfile);
        } catch (error) {
            console.error('Error updating profile:', error);
        }
    };

    const handleImageUpload = async (uploadedImage: Blob) => {
        try {
            const userId = user.id;
            const file = new File([uploadedImage], 'profileImage', {
                type: uploadedImage.type,
            });
            await ProfileService.uploadProfileImage(file, userId);
            setCurrentProfileImage(uploadedImage);
            console.log('Image uploaded:', uploadedImage);
        } catch (error) {
            console.error('Error uploading image:', error);
        }
    };

    const imageUrl: string = currentProfileImage
        ? URL.createObjectURL(currentProfileImage)
        : '/profile.svg';

    return (
        <div className="page-column flex flex-col gap-5 font-thin max-w-full">
            <div className="flex flex-row gap-5 items-center">
                <ImageTooltip
                    width={300}
                    imageUrl={imageUrl}
                    toolTipProps={{
                        placement: 'right-start',
                        delay: 700,
                        content: <div>{currentProfileImage?.type}</div>,
                    }}
                    imageProps={{ isZoomed: true }}
                >
                    {currentProfileImage ? (
                        <Image
                            className="rounded-md min-w-16 max-w-16 shadow shadow-accent-orange cursor-pointer hover:shadow-lg hover:shadow-accent-orange"
                            src={imageUrl}
                            alt="Profile image"
                        />
                    ) : (
                        <Spinner color="primary" size="md" />
                    )}
                </ImageTooltip>
                <div className="flex flex-col gap-1">
                    <div className="flex flex-row justify-between gap-4">
                        <h2 className="text-2xl font-bold">
                            {user.firstName} {user.lastName}
                        </h2>
                        {isAuthenticated && (
                            <Button
                                size="sm"
                                className="p-4 mx-2 mb-1"
                                color="primary"
                                onPress={onOpen}
                                endContent={<FaEdit color="primary" />}
                            >
                                Edit
                            </Button>
                        )}
                    </div>
                    <Snippet
                        className="w-fit"
                        symbol="@"
                        variant="bordered"
                        tooltipProps={{
                            content: 'Copy username',
                            disableAnimation: true,
                            placement: 'right',
                            closeDelay: 0,
                        }}
                    >
                        {user.userName}
                    </Snippet>
                </div>
            </div>
            {currentProfileInfo && (
                <div className={`${roboto.className} flex flex-col gap-2`}>
                    {(currentProfileInfo.about ||
                        currentProfileInfo.anything) && (
                        <div className="grid grid-cols-2 gap-1">
                            {currentProfileInfo.about && (
                                <div>
                                    <h2 className="profile-header">
                                        {t('about_title')}:
                                    </h2>
                                    <p className="text-base text-balance">
                                        {currentProfileInfo.about}
                                    </p>
                                </div>
                            )}
                            {currentProfileInfo.anything && (
                                <div className="text-right">
                                    <h2 className="profile-header">
                                        {t('anything_title')}:
                                    </h2>
                                    <p className="text-base text-balance">
                                        {currentProfileInfo.anything}
                                    </p>
                                </div>
                            )}
                        </div>
                    )}
                    {currentProfileInfo.interests.length > 0 && (
                        <div className="flex flex-col gap-2 w-full">
                            <h2 className="profile-header font-light">
                                {t('interests_title')}:
                            </h2>
                            <div className="flex flex-row gap-2 flex-wrap items-center justify-center">
                                {currentProfileInfo.interests.map(
                                    (interest) => (
                                        <div
                                            key={interest}
                                            className="category-pill whitespace-nowrap text-center group"
                                        >
                                            <span className="group-hover:text-accent-orange">
                                                {interest}
                                            </span>
                                        </div>
                                    )
                                )}
                            </div>
                        </div>
                    )}
                    {currentProfileInfo.references.length > 0 && (
                        <div className="flex flex-col gap-2 w-full">
                            <h2 className="profile-header font-light">
                                {t('references_title')}:
                            </h2>
                            <div className="flex gap-2 flex-wrap">
                                {currentProfileInfo.references.map((ref) => {
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
            )}
            <EditProfileModal
                profileInfo={currentProfileInfo}
                image={currentProfileImage}
                isOpen={isOpen}
                onOpenChange={onOpenChange}
                onSave={handleSaveProfile}
                onImageUpload={handleImageUpload}
            />
        </div>
    );
};

const isValidDateFormat = (dateString: string) => {
    const regex = /^\d{4}-\d{2}-\d{2}$/;
    return regex.test(dateString);
};

const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toISOString().split('T')[0];
};

export default UserProfile;
