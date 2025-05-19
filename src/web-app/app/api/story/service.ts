import apiClient from '@/app/lib/api/client';
import { CreateStoryModel, Story, UpdateStoryModel } from './types';

const API_VERSION = '1.0';
const BASE_PATH = `/api/v${API_VERSION}/stories`;

const storiesEndpoints = {
    getAll: BASE_PATH,
    getCount: `${BASE_PATH}/count`,
    create: BASE_PATH,
    getById: (id: number) => `${BASE_PATH}/${id}`,
    update: (id: number) => `${BASE_PATH}/${id}`,
    delete: (id: number) => `${BASE_PATH}/${id}`,
};

const StoriesService = {
    async getAllStories(filters?: {
        tag?: string;
        authorId?: string;
        pageNumber?: number;
        pageSize?: number;
    }): Promise<Story[]> {
        try {
            const response = await apiClient.get(storiesEndpoints.getAll, {
                params: filters,
                withCredentials: true,
            });
            return response.data;
        } catch (error) {
            return [];
        }
    },

    async getAllStoriesCount(filters?: {
        tag?: string;
        authorId?: string;
    }): Promise<number> {
        const response = await apiClient.get(storiesEndpoints.getCount, {
            params: filters,
            withCredentials: true,
        });

        return response.data;
    },

    async getStoryById(id: number): Promise<Story | null> {
        try {
            const response = await apiClient.get(storiesEndpoints.getById(id), {
                withCredentials: true,
            });
            return response.data;
        } catch (error) {
            return null;
        }
    },

    async createStory(payload: CreateStoryModel): Promise<Story | null> {
        try {
            const response = await apiClient.post(
                storiesEndpoints.create,
                payload,
                { withCredentials: true }
            );
            return response.data;
        } catch (error) {
            return null;
        }
    },

    async updateStory(
        id: number,
        payload: UpdateStoryModel
    ): Promise<Story | null> {
        try {
            const response = await apiClient.put(
                storiesEndpoints.update(id),
                payload,
                { withCredentials: true }
            );
            return response.data;
        } catch (error) {
            return null;
        }
    },

    async deleteStory(id: number): Promise<boolean> {
        try {
            await apiClient.delete(storiesEndpoints.delete(id), {
                withCredentials: true,
            });
            return true;
        } catch (error) {
            return false;
        }
    },
};

export default StoriesService;
