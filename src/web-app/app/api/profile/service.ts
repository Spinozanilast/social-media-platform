import apiClient from '@/app/lib/api/client';
import { Country, Profile } from './types';

const API_VERSION = '1.0';
const BASE_PATH = `/api/v${API_VERSION}/profiles`;

const userProfileEndpoint = (userId: string) => `${BASE_PATH}/${userId}`;
const profileImageEndpoint = (userId: string) =>
    `${userProfileEndpoint(userId)}/image`;

const profilesEndpoints = {
    get: (userId: string) => `${userProfileEndpoint(userId)}`,
    init: (userId: string) => `${userProfileEndpoint(userId)}/initialize`,
    update: (userId: string) => `${userProfileEndpoint(userId)}/update`,
    delete: (userId: string) => `${userProfileEndpoint(userId)}/delete`,
};

const profileImagesEndpoints = {
    get: (userId: string) => `${profileImageEndpoint(userId)}`,
    upload: (userId: string) => `${profileImageEndpoint(userId)}/upload`,
    remove: (userId: string) => `${profileImageEndpoint(userId)}/remove`,
};

export const getCountries = async (): Promise<Country[]> => {
    const response = await fetch(
        `${apiClient.getUri()}${BASE_PATH}/countries`,
        {
            cache: 'force-cache',
        }
    );

    if (response.ok) {
        return response.json();
    }

    return [];
};

export const ProfilesService = {
    async save(payload: Profile): Promise<boolean> {
        const response = await apiClient.post(
            profilesEndpoints.update(payload.userId),
            payload,
            { withCredentials: true }
        );

        return response.status === 200;
    },

    async get(userId: string): Promise<Profile | null> {
        try {
            const response = await apiClient.get(profilesEndpoints.get(userId));
            return response.data;
        } catch (error) {
            return null;
        }
    },

    async delete(userId: string): Promise<void> {
        await apiClient.delete(profilesEndpoints.delete(userId));
    },
};

export const ProfileImagesService = {
    async get(userId: string): Promise<Blob | null> {
        const response = await fetch(profileImagesEndpoints.get(userId));

        if (!response.ok) {
            return null;
        }

        return await response.json();
    },

    async upload(profileImage: Blob, userId: string): Promise<void> {
        const formData = new FormData();
        formData.append('profileImage', profileImage);

        await apiClient.post(profileImagesEndpoints.upload(userId), formData, {
            headers: { 'Content-Type': 'multipart/form-data' },
        });
    },

    async remove(userId: string): Promise<void> {
        await apiClient.delete(profileImagesEndpoints.remove(userId));
    },
};
