'use client';
import React, { useEffect, useState } from 'react';
import { useTranslations } from 'next-intl';
import { useDisclosure } from '@heroui/react';
import EditProfileModal from '@components/modals/EditProfileModal';
import { User } from '@api/auth/types';
import { Profile } from '@api/profile/types';
import { ProfileImagesService, ProfilesService } from '@api/profile/service';
import { ProfileHeader } from '@components/profile/ProfileHeader';
import { ProfileDetails } from '@components/profile/ProfileDetails';

interface UserProfileProps {
    user: User;
    profile: Profile;
    isOwner: boolean;
}

const UserProfile: React.FC<UserProfileProps> = ({
    user,
    profile,
    isOwner,
}) => {
    const t = useTranslations('Profile');

    const [imageUrl, setImageUrl] = useState('/profile.svg');
    const [profileInfo, setProfileInfo] = useState(profile);

    const { isOpen, onOpen, onOpenChange } = useDisclosure();

    useEffect(() => {
        ProfileImagesService.get(user.id).then((data) => {
            if (data) {
                setImageUrl(URL.createObjectURL(data));
            }
        });
    }, [user.id]);

    const handleSaveProfile = async (updatedProfile: Profile) => {
        try {
            await ProfilesService.save(updatedProfile);
            setProfileInfo(updatedProfile);
        } catch (error) {
            console.error('Profile update failed:', error);
        }
    };

    const handleImageUpload = async (image: Blob) => {
        try {
            await ProfileImagesService.upload(image, user.id);
            setImageUrl(URL.createObjectURL(image));
        } catch (error) {
            console.error('Image upload failed:', error);
        }
    };

    return (
        <div className="page-column content-center flex-col gap-5 font-thin max-w-full">
            <ProfileHeader
                firstName={user.firstName}
                lastName={user.lastName}
                userName={user.userName}
                imageUrl={imageUrl}
                isOwner={isOwner}
                onModalOpen={onOpen}
                translations={{ edit: t('edit') }}
            />
            <ProfileDetails
                info={profileInfo}
                translations={{
                    about_title: t('about_title'),
                    interests_title: t('interests_title'),
                    references_title: t('references_title'),
                }}
            />
            <EditProfileModal
                profileInfo={profileInfo}
                imageUrl={imageUrl}
                isOpen={isOpen}
                onOpenChange={onOpenChange}
                onSave={handleSaveProfile}
                onImageUpload={handleImageUpload}
            />
        </div>
    );
};

export default UserProfile;
