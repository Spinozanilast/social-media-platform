// StoriesService.ts
import axios from 'axios';
import ApiConfigManager from '../util/configManager';
import Story, {
    CreateStoryModel,
    UpdateStoryModel,
} from '@/app/models/Stories/stories';
import { ValidationResult } from '@/app/models/validation-result';

const STORIES_API_KEYWORD = 'stories';
const GATEWAY_URL: string = ApiConfigManager.getApiConfig().baseURL!;
const STORIES_API_BASE_URL = `${GATEWAY_URL}/${STORIES_API_KEYWORD}`;

const axiosInstance = axios.create({
    baseURL: STORIES_API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true,
});

const getAllStories = async (
    tag?: string,
    authorId?: string,
    pageNumber: number = 1,
    pageSize: number = 5
): Promise<Story[]> => {
    const response = await axiosInstance.get<Story[]>(STORIES_API_BASE_URL, {
        params: { tag, authorId, pageNumber, pageSize },
    });
    return response.data;
};

const getAllStoriesCount = async (
    tag?: string,
    authorId?: string
): Promise<number> => {
    const response = await axiosInstance.get<number>(
        `${STORIES_API_BASE_URL}/count`,
        {
            params: { tag, authorId },
        }
    );
    return response.data;
};

const getStoryById = async (id: number): Promise<Story> => {
    const response = await axiosInstance.get<Story>(
        `${STORIES_API_BASE_URL}/${id}`
    );
    return response.data;
};

const createStory = async (
    createStoryModel: CreateStoryModel
): Promise<ValidationResult> => {
    const response = await axiosInstance.post<ValidationResult>(
        STORIES_API_BASE_URL,
        createStoryModel
    );
    return response.data;
};

const updateStory = async (
    id: number,
    updateStoryModel: UpdateStoryModel
): Promise<ValidationResult> => {
    const response = await axiosInstance.put<ValidationResult>(
        `${STORIES_API_BASE_URL}/${id}`,
        updateStoryModel
    );
    return response.data;
};

const deleteStory = async (id: number): Promise<void> => {
    await axiosInstance.delete(`${STORIES_API_BASE_URL}/${id}`);
};

const StoriesService = {
    getAllStories,
    getAllStoriesCount,
    getStoryById,
    createStory,
    updateStory,
    deleteStory,
};

export default StoriesService;
