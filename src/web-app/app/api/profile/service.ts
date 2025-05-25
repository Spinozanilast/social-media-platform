import apiClient from '~/lib/api/client';
import { Country, Profile } from '~api/profile/types';

const API_VERSION = '1';
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
    getAntiforgeryToken: `${BASE_PATH}/antiforgery/token`,
    get: (userId: string) => `${profileImageEndpoint(userId)}`,
    upload: (userId: string) => `${profileImageEndpoint(userId)}/upload`,
    remove: (userId: string) => `${profileImageEndpoint(userId)}/remove`,
};

export const getCountries = async (): Promise<Country[]> => {
    try {
        const response = await apiClient.get<Country[]>(
            `${BASE_PATH}/countries`
        );
        return response.data;
    } catch (error) {
        return [];
    }
};

export const ProfilesService = {
    async save(payload: Profile): Promise<boolean> {
        const response = await apiClient.put(
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
        try {
            const response = await apiClient.get(
                profileImagesEndpoints.get(userId),
                {
                    responseType: 'blob',
                }
            );
            return response.data;
        } catch (error) {
            return null;
        }
    },

    async getCsrfToken(): Promise<string> {
        const response = await apiClient.get<{ token: string }>(
            profileImagesEndpoints.getAntiforgeryToken,
            { withCredentials: true }
        );
        return response.data.token;
    },

    async upload(profileImage: Blob, userId: string): Promise<void> {
        const csrfToken = await this.getCsrfToken();
        const formData = new FormData();
        formData.append('profileImage', profileImage);
        formData.append('__RequestVerificationToken', csrfToken);

        await apiClient.post(profileImagesEndpoints.upload(userId), formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
                'X-CSRF-TOKEN': csrfToken,
            },
            withCredentials: true,
        });
    },

    async remove(userId: string): Promise<void> {
        await apiClient.delete(profileImagesEndpoints.remove(userId));
    },
};
